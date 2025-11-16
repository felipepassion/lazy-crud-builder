using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;

public static class LambdaToJsonStringExtension
{
    public static string LambdaToJsonString<T>(this Expression<Func<T, bool>> expression)
    {
        var queryParams = new Dictionary<string, object>();
        BuildJsonString(expression.Body, queryParams);
        return JsonConvert.SerializeObject(queryParams);
    }

    public static string LambdaToQueryString<T>(this Expression<Func<T, bool>> expression)
    {
        var queryStringBuilder = new StringBuilder();
        BuildQueryString(expression.Body, queryStringBuilder);
        return queryStringBuilder.ToString();
    }

    private static void BuildQueryString(Expression expression, StringBuilder queryStringBuilder)
    {
        if (expression is BinaryExpression binaryExpr)
        {
            var leftMemberExpr = GetMemberExpression(binaryExpr.Left);
            queryStringBuilder.Append(leftMemberExpr.Member.Name);
            queryStringBuilder.Append(InferSuffix(binaryExpr.NodeType, leftMemberExpr.Type));
            queryStringBuilder.Append('=');
            BuildQueryString(binaryExpr.Right, queryStringBuilder);
        }
        else
        {
            queryStringBuilder.Append(GetExpressionValue(expression));
        }
    }

    private static void BuildJsonString(Expression expression, Dictionary<string, object> queryParams)
    {
        if (expression is BinaryExpression binaryExpr)
        {
            var leftMemberExpr = GetMemberExpression(binaryExpr.Left);
            var key = leftMemberExpr.Member.Name + InferSuffix(binaryExpr.NodeType, leftMemberExpr.Type);
            queryParams.Add(key, GetExpressionValue(binaryExpr.Right));
        }
        else
        {
            throw new NotSupportedException($"Tipo de expressão não suportado: {expression.GetType()}");
        }
    }

    private static MemberExpression GetMemberExpression(Expression expression)
    {
        return expression switch
        {
            MemberExpression memberExpr => memberExpr,
            UnaryExpression unaryExpr => unaryExpr.Operand as MemberExpression,
            _ => throw new NotSupportedException($"Tipo de expressão não suportado: {expression.GetType()}")
        };
    }

    private static string InferSuffix(ExpressionType operatorType, Type propertyType)
    {
        TypeCode typeCode = Type.GetTypeCode(propertyType);

        if (typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal || propertyType == typeof(DateTime))
        {
            return operatorType switch
            {
                ExpressionType.Equal => "Equal",
                ExpressionType.NotEqual => "NotEqual",
                ExpressionType.GreaterThan => "GreaterThan",
                ExpressionType.GreaterThanOrEqual => "GreaterThanOrEqual",
                ExpressionType.LessThan => "LessThan",
                ExpressionType.LessThanOrEqual => "LessThanOrEqual",
                _ => throw new NotSupportedException($"Operador não suportado: {operatorType}")
            };
        }
        if (propertyType == typeof(string))
        {
            return operatorType == ExpressionType.Equal ? "Equal" : "NotEqual";
        }
        if (propertyType == typeof(bool))
        {
            return "Equal";
        }

        return string.Empty;
    }

    private static string GetExpressionValue(Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpr:
                var objectMember = Expression.Convert(memberExpr, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                return getter().ToString();

            case ConstantExpression constantExpr:
                return constantExpr.Value.ToString();

            default:
                throw new NotSupportedException($"Tipo de expressão não suportado: {expression.GetType()}");
        }
    }
}
