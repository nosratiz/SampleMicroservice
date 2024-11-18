using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Frontliners.Common.InfraStructure.Extensions;

public static class HttpContextExtensions
{
    public static async Task WriteError(this HttpContext httpContext, object error)
    {
        httpContext.Response.StatusCode = 500;
        httpContext.Response.Headers.Append("Content-Type", "application/json");

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(error));
    }

    public static async Task WriteError(this HttpContext httpContext, object error, int statusCode)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.Headers.Append("Content-Type", "application/json");

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(error));
    }

    public static async Task WriteJsonAsync(this HttpContext httpContext, object obj)
    {
        httpContext.Response.StatusCode = 200;
        httpContext.Response.Headers.Append("Content-Type", "application/json");

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(obj));
    }



    public static bool HasFileCount(this HttpContext context) =>
        !string.IsNullOrEmpty(context.Request.Headers["X-MultiSelect"]);



    public static bool HasAuthorization(this HttpContext context) =>
        !string.IsNullOrEmpty(context.Request.Headers["Authorization"]);

    public static string GetAuthorizationToken(this HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"])) return string.Empty;

        const string authenticationSchema = "Bearer";

        var userToken = context.Request.Headers["Authorization"].ToString();

        return userToken.Replace($"{authenticationSchema} ", "");
    }



}