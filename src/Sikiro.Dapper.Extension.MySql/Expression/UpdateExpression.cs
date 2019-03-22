using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Sikiro.Dapper.Extension.Extension;
using Sikiro.Dapper.Extension.Helper;

namespace Sikiro.Dapper.Extension.MySql.Expression
{
    internal sealed class UpdateExpression : ExpressionVisitor
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        private const string Prefix = "UPDATE_";
        /// <summary>
        /// sql指令
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" SET {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        #endregion

        #region 执行解析
        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public UpdateExpression(LambdaExpression expression)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();

            Visit(expression);
        }
        #endregion

        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            var memberInitExpression = node;

            var entity = ((ConstantExpression)TrimExpression.Trim(memberInitExpression)).Value;

            var properties = memberInitExpression.Type.GetProperties();

            foreach (var item in properties)
            {
                if (item.CustomAttributes.Any(b => b.AttributeType == typeof(KeyAttribute)))
                    continue;

                if (_sqlCmd.Length > 0)
                    _sqlCmd.Append(",");

                var paramName = item.Name;
                var value = item.GetValue(entity);
                var c = item.GetColumnAttributeName();
                SetParam(c, paramName, value);
            }

            return node;
        }


        protected override System.Linq.Expressions.Expression VisitMemberInit(MemberInitExpression node)
        {
            var memberInitExpression = node;

            foreach (var item in memberInitExpression.Bindings)
            {
                var memberAssignment = (MemberAssignment)item;

                if (_sqlCmd.Length > 0)
                    _sqlCmd.Append(",");

                var paramName = memberAssignment.Member.Name;
                var c = memberAssignment.Member.GetColumnAttributeName();
                var constantExpression = (ConstantExpression)memberAssignment.Expression;
                SetParam(c, paramName, constantExpression.Value);
            }

            return node;
        }

        private void SetParam(string sqlParamName, string paramName, object value)
        {
            var n = $"@{Prefix}{paramName}";
            _sqlCmd.AppendFormat(" {0}={1} ", sqlParamName, n);
            Param.Add(n, value);
        }
    }
}
