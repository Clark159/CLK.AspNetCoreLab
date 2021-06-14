using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    public class Startup
    {
        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Config
            var issuer = "AuthenticationLab";
            var signKey = "12345678901234567890123456789012";

            // Mvc
            services.AddMvc();

            // Authentication   
            services.AddAuthentication(options =>
            {
                // DefaultScheme
                options.DefaultScheme = PolicyAuthenticationDefaults.AuthenticationScheme;
            })
            .AddPolicy(options =>
            {
                // DefaultScheme
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // AuthenticateSchemePolicy
                options.AuthenticateSchemePolicy = context =>
                {
                    // JwtBearer
                    var authorization = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (string.IsNullOrEmpty(authorization) == false && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }

                    // Cookie
                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCookie(options =>
            {
                // Action
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
                options.AccessDeniedPath = options.LoginPath;
            })
            .AddJwtBearer(options =>
            {
                // Validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Setting
                    AuthenticationType = "JwtBearer",

                    // Issuer
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    // Audience
                    ValidateAudience = false,
                    ValidAudience = null,

                    // Lifetime
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    // Signing                        
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)),
                };
            });

            // Service
            services.AddSingleton<SecurityTokenFactory>(new SecurityTokenFactory(issuer, signKey));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Contracts

            if (app == null) throw new ArgumentException(nameof(app));
            if (env == null) throw new ArgumentException(nameof(env));

            #endregion

            // Development
            if (env.IsDevelopment() == true)
            {
                app.UseDeveloperExceptionPage();
            }

            // StaticFile
            app.UseStaticFiles();

            // Authentication
            app.UseAuthentication();

            // Routing
            app.UseRouting();
            {

            }

            // Authorization
            app.UseAuthorization();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                // Default
                endpoints.MapControllerRoute
                (
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}"
                );

                // Area
                endpoints.MapControllerRoute
                (
                    name: "Area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}"
                );
            });
        }
    }
}
