using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameDb.Api.Configuration;

public static class SwaggerGenOptionsExtensions
{
    public static void AddJwtSecurityScheme(this SwaggerGenOptions options)
    {
        const string schemeName = "Bearer";
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "GameDB API",
            Version = "v1"
        });

        options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Bearer token."
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeName
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
