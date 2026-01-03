using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CreditAppManager.Api.Filters;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using Business.Api.Application;
using Business.Api.Outbound.Persistence;
using Data.Contracts;

namespace CreditAppManager.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddDataContracts(_configuration);
        
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", p => 
                p.WithOrigins("http://localhost:4200")
                 .AllowAnyMethod()
                 .AllowAnyHeader());
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<ModelValidationFilter>();
        })
        .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var jwt = _configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? string.Empty;
        var issuer = jwt["Issuer"] ?? "UsuariosAPI";
        var audience = jwt["Audience"] ?? "UsuariosAPI";

        if (!string.IsNullOrEmpty(key))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Usuarios API",
                Version = "v1",
                Description = "API para gestión de usuarios con arquitectura por capas",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Auth using Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        if (env.IsDevelopment())
        {
            // dev specific middleware if needed
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAngular");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", ctx =>
            {
                ctx.Response.Redirect("/swagger");
                return Task.CompletedTask;
            }).WithMetadata(new Microsoft.AspNetCore.Mvc.ApiExplorerSettingsAttribute { IgnoreApi = true });
        });
    }
}
