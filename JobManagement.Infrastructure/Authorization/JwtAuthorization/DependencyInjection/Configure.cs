using JobManagement.Infrastructure.Authorization.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace JobManagement.Infrastructure.Authorization.JwtAuthorization.DependencyInjection;

public static class Configure
{

    public const string Key = "Jwt";

    public static IServiceCollection ConfigureJwtAuthorization(
        this IServiceCollection services,
        IConfiguration config)
    {
        config = config.GetRequiredSection(Key);

        services.AddTransient<
            IAuthorizationProvider,
            JwtAuthorizationProvider>();

        services.Configure<JwtAuthorizationOptions>(config);
        JwtAuthorizationOptions jwtOptions = config.Get<JwtAuthorizationOptions>()!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.PrivateKey))
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static SwaggerGenOptions AddSwaggerGenUsers(this SwaggerGenOptions options, IConfiguration configuration)
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Manager API", Version = "v1" });
        options.AddSecurityDefinition(IAuthorizationProvider.Name, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }});

        return options;
    }

}
