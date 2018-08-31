using System;
using System.Collections.Generic;
using System.Text;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Core
{
    public class LambdaExtensionException : ApplicationException
    {
        public LambdaExtensionException(string msg) : base(msg)
        {

        }
    }
}
