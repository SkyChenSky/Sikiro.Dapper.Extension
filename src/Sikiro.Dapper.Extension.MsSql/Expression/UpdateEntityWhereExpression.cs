using System.Text;
using Dapper;
using Sikiro.Dapper.Extension.Extension;

namespace Sikiro.Dapper.Extension.MsSql.Expression
{
    internal class UpdateEntityWhereExpression
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// 
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        public DynamicParameters Param { get; }

        private readonly object _obj;

        #endregion

        #region 执行解析

        /// <inheritdoc />
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public UpdateEntityWhereExpression(object obj)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _obj = obj;
        }
        #endregion

        public void Resolve()
        {
            var propertyInfo = _obj.GetKeyPropertity();
            _sqlCmd.Append(propertyInfo.GetColumnAttributeName());
            _sqlCmd.Append(" = ");
            SetParam(propertyInfo.Name, propertyInfo.GetValue(_obj));
        }

        private void SetParam(string fileName, object value)
        {
            if (value != null)
            {
                _sqlCmd.Append("@" + fileName);
                Param.Add(fileName, value);
            }
            else
            {
                _sqlCmd.Append("NULL");
            }
        }
    }
}
