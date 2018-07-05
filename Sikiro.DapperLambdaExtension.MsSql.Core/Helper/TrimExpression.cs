using System;
using System.Linq.Expressions;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Helper
{
    /// <inheritdoc />
    /// <summary>
    /// 修树
    /// </summary>
    internal class TrimExpression : ExpressionVisitor
    {
        internal static Expression Trim(Expression expression)
        {
            return new TrimExpression().Visit(expression);
        }

        private Expression Sub(Expression expression)
        {
            var type = expression.Type;
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    if (expression.Type == type)
                        return expression;
                    else if (TypeHelper.GetNonNullableType(expression.Type) == TypeHelper.GetNonNullableType(type))
                        return Expression.Constant(((ConstantExpression)expression).Value, type);
                    break;

                case ExpressionType.MemberAccess:
                    var mExpression = expression as MemberExpression;
                    var root = mExpression.GetRootMember();
                    if (root != null)
                    {
                        var value = mExpression.MemberToValue(root);
                        return Expression.Constant(value, type);
                    }
                    return mExpression;

                case ExpressionType.Convert:
                    var u = (UnaryExpression)expression;
                    if (TypeHelper.GetNonNullableType(u.Operand.Type) == TypeHelper.GetNonNullableType(type))
                    {
                        expression = u.Operand;
                        return expression;
                    }

                    if (u.Operand.Type.IsEnum && u.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var value = Convert.ChangeType((u.Operand as MemberExpression).MemberToValue(), type);
                        return Expression.Constant(value, type);
                    }
                    break;
                case ExpressionType.AndAlso:
                    var b = (BinaryExpression)expression;
                    if (b.Left.NodeType == ExpressionType.Constant)
                        return b.Right;
                    if (b.Right.NodeType == ExpressionType.Constant)
                        return b.Left;
                    break;
            }

            return expression;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            exp = Sub(exp);
            return base.Visit(exp);
        }

    }
}
