using System.Collections;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Sikiro.Dapper.Extension.Exception;
using Sikiro.Dapper.Extension.Extension;
using Sikiro.Dapper.Extension.Helper;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.MsSql.Expression
{
    internal sealed class WhereExpression : ExpressionVisitor
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        private string _tempFieldName;

        private string TempFieldName
        {
            get => _prefix + _tempFieldName;
            set => _tempFieldName = value;
        }

        private string ParamName => _parameterPrefix + TempFieldName;

        private readonly string _prefix;

        private readonly char _parameterPrefix;

        private readonly char _closeQuote;

        private readonly char _openQuote;

        #endregion

        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="prefix">字段前缀</param>
        /// <param name="providerOption"></param>
        /// <returns></returns>
        public WhereExpression(LambdaExpression expression, string prefix, ProviderOption providerOption)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();

            _prefix = prefix;
            _parameterPrefix = providerOption.ParameterPrefix;
            _openQuote = providerOption.OpenQuote;
            _closeQuote = providerOption.CloseQuote;
            var exp = TrimExpression.Trim(expression);
            Visit(exp);
        }
        #endregion

        #region 访问成员表达式

        /// <inheritdoc />
        /// <summary>
        /// 访问成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            _sqlCmd.Append(_openQuote + node.Member.GetColumnAttributeName() + _closeQuote);
            TempFieldName = node.Member.Name;

            return node;
        }

        #endregion

        #region 访问二元表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitBinary(BinaryExpression node)
        {
            _sqlCmd.Append("(");
            Visit(node.Left);

            _sqlCmd.Append(node.GetExpressionType());

            Visit(node.Right);
            _sqlCmd.Append(")");

            return node;
        }
        #endregion

        #region 访问常量表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression node)
        {
            SetParam(node.Value);

            return node;
        }
        #endregion

        #region 访问方法表达式
        /// <inheritdoc />
        /// <summary>
        /// 访问方法表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Contains" && typeof(IEnumerable).IsAssignableFrom(node.Method.DeclaringType) &&
                node.Method.DeclaringType != typeof(string))
                In(node);
            else
                Like(node);

            return node;
        }

        #endregion

        private void SetParam(object value)
        {
            if (value != null)
            {
                _sqlCmd.Append(ParamName);
                Param.Add(TempFieldName, value);
            }
            else
            {
                _sqlCmd.Append("NULL");
            }
        }

        private void Like(MethodCallExpression node)
        {
            Visit(node.Object);
            _sqlCmd.AppendFormat(" LIKE {0}", ParamName);

            switch (node.Method.Name)
            {
                case "StartsWith":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFieldName, argumentExpression.Value + "%");
                    }
                    break;
                case "EndsWith":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFieldName, "%" + argumentExpression.Value);
                    }
                    break;
                case "Contains":
                    {
                        var argumentExpression = (ConstantExpression)node.Arguments[0];
                        Param.Add(TempFieldName, "%" + argumentExpression.Value + "%");
                    }
                    break;
                default:
                    throw new DapperExtensionException("the expression is no support this function");
            }
        }

        private void In(MethodCallExpression node)
        {
            var arrayValue = (IList)((ConstantExpression)node.Object).Value;
            if (arrayValue.Count == 0)
            {
                _sqlCmd.Append(" 1 = 2");
                return;
            }
            Visit(node.Arguments[0]);
            _sqlCmd.AppendFormat(" IN {0}", ParamName);
            Param.Add(TempFieldName, arrayValue);
        }
    }
}
