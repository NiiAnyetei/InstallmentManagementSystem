using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace IMS.Config
{
    public class AuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for AllowAnonymous attribute

            var declaringType = context.MethodInfo.DeclaringType;
            if (declaringType is null) return;

            var allowAnonymous = (declaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() && context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()) || (!declaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() && !context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            if (allowAnonymous) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme,
                    },
                }] = Array.Empty<string>()
            });
        }
    }
}
