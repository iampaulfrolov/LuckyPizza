using System;
using System.Linq.Expressions;
using System.Reflection;
using CourseProject.Data;

namespace CourseProject.Extensions;

public static class ExpressionExtension
{
    public static string GetTablePropertyName<TEntity, TProperty>(
        this Expression<Func<TEntity, TProperty>> propertyExpression, TableInfo<TEntity> tableInfo)
        where TEntity : new()
    {
        var member = propertyExpression.Body as MemberExpression;
        var propertyInfo = member?.Member as PropertyInfo;
        var propertyName = propertyInfo?.Name;
        var memberTypeName = member?.Expression.Type.Name;
        if (memberTypeName == tableInfo.EntityType.Name)
            return propertyName ?? string.Empty;
        return memberTypeName + "_Id";
    }
}