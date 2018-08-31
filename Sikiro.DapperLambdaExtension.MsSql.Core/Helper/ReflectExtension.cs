using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Sikiro.DapperLambdaExtension.MsSql.Core.Core;

namespace Sikiro.DapperLambdaExtension.MsSql.Core.Helper
{
    internal static class ReflectExtension
    {
        internal static PropertyInfo[] GetProperties(this object obj)
        {
            return obj.GetType().GetProperties().Where(f => !Attribute.IsDefined(f, typeof(DatabaseGeneratedAttribute))).ToArray();
        }

        internal static PropertyInfo GetKeyPropertity(this object obj)
        {
            var properties = obj.GetType().GetProperties().Where(a => a.GetCustomAttribute<KeyAttribute>() != null).ToArray();

            if (!properties.Any())
                throw new LambdaExtensionException($"the {nameof(obj)} entity with no KeyAttribute Propertity");

            if (properties.Length > 1)
                throw new LambdaExtensionException($"the {nameof(obj)} entity with greater than one KeyAttribute Propertity");

            return properties.First();
        }
    }
}
