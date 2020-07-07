using System.Text;
using Dapper;
using Sikiro.Dapper.Extension.Extension;
using Sikiro.Dapper.Extension.Model;

namespace Sikiro.Dapper.Extension.Expressions
{
    public class UpdateEntityWhereExpression
    {
        #region sql指令

        private readonly StringBuilder _sqlCmd;

        /// <summary>
        /// sql指令
        /// 
        /// </summary>
        public string SqlCmd => _sqlCmd.Length > 0 ? $" WHERE {_sqlCmd} " : "";

        private readonly ProviderOption _providerOption;

        private readonly char _parameterPrefix;

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
        public UpdateEntityWhereExpression(object obj, ProviderOption providerOption)
        {
            _sqlCmd = new StringBuilder(100);
            Param = new DynamicParameters();
            _providerOption = providerOption;
            _parameterPrefix = _providerOption.ParameterPrefix;
            _obj = obj;
        }

        #endregion

        public void Resolve()
        {
            var propertyInfo = _obj.GetKeyPropertity();
            var fieldName = _providerOption.CombineFieldName(propertyInfo.GetColumnAttributeName());
            _sqlCmd.Append(fieldName);
            _sqlCmd.Append(" = ");
            SetParam(propertyInfo.Name, propertyInfo.GetValue(_obj));
        }

        private void SetParam(string fileName, object value)
        {
            if (value != null)
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    _sqlCmd.Append(_parameterPrefix + fileName);
                    Param.Add(fileName, value);
                }
            }
            else
            {
                _sqlCmd.Append("NULL");
            }
        }
    }
}
