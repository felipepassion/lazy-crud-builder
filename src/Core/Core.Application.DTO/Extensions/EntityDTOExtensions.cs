using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Lazy.Crud.Core.Application.DTO.Extensions;

public static class EntityDTOExtensions
{
    public static string GetFieldPlaceholder<T>(this T model, Expression<Func<T, object>> filter)
        where T : EntityDTO
    {
        try
        {
            var property = model.GetPropertyInfo(filter);

            //MemberInfo property = model.GetType().GetProperty(filter.Name) ?? throw new KeyNotFoundException(filter.Name);
            if (property == null)
            {
                return string.Empty;
            }

            var dd = property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;

            return dd?.DisplayName ?? property.Name;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        where TSource : class
    {
        Type type = typeof(TSource);

        MemberExpression member = propertyLambda.Body as MemberExpression;
        if (member == null)
            return null;

        PropertyInfo propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
            return null;

        //if (type != propInfo.ReflectedType &&
        //    !type.IsSubclassOf(propInfo.ReflectedType))
        //    throw new ArgumentException(string.Format(
        //        "Expression '{0}' refers to a property that is not from type {1}.",
        //        propertyLambda.ToString(),
        //        type));

        return propInfo;
    }
}
