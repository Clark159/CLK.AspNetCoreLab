using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace JwtAuthLab
{
    public class Startup
    {
        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Services
            services.AddSingleton<JwtHelper>();

            // Mvc
            var mvcBuilder = services.AddMvc();
            {
                // JsonOptions
                mvcBuilder.AddJsonOptions(options =>
                {
                    // Encoder
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
                });
            }            

            // Authentication
            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            {
                // Config
                var issuer = "JwtAuthLab";
                var signKey = "12345678901234567890123456789012";

                // JwtBearer
                authenticationBuilder.AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Setting
                        AuthenticationType = "JWT",

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
            }
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

            // Routing
            app.UseRouting();
            {

            }

            // Auth
            app.UseAuthentication();
            app.UseAuthorization();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                // Default
                endpoints.MapControllerRoute
                (
                    name: "Default",
                    pattern: "{controller=Token}/{action=SignIn}"
                );
            });
        }
    }
}
