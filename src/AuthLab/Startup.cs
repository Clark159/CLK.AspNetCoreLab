using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthLab
{
    public class Startup
    {
        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Mvc
            services.AddMvc();

            // Authentication
            services.AddAuthenticationCore(options => options.AddScheme<CustomAuthenticationHandler>("CustomScheme", "CustomScheme"));
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

            // 3.Login
            app.Map("/login", builder => builder.Use(next =>
            {
                return async (context) =>
                {
                    // ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Clark")
                    }
                    , "CustomScheme");

                    // SignIn
                    await context.SignInAsync("CustomScheme", new ClaimsPrincipal(claimsIdentity));

                    // Redirect
                    context.Response.Redirect("/");
                };
            }));
                     
            // 4.Authenticate
            app.Use(async (context, next) =>
            {
                // User
                var result = await context.AuthenticateAsync("CustomScheme");
                if (result?.Principal != null) context.User = result.Principal;

                // Next
                await next();
            });

            // 1.Authorization
            app.Use(async (context, next) =>
            {
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    if (context.User.Identity.Name == "Clark")
                    {
                        // 5.Next
                        await next();                        
                    }
                    else
                    {
                        // Forbid
                        await context.ForbidAsync("CustomScheme");
                    }
                }
                else
                {
                    // 2.Challenge
                    await context.ChallengeAsync("CustomScheme");
                }
            });

            // 6.Logout
            app.Use(async (context, next) =>
            {
                // SignOut
                await context.SignOutAsync("CustomScheme");

                // Next
                await next();
            });

            // 7.Endpoints
            app.UseEndpoints(endpoints =>
            {
                // Default
                endpoints.MapControllerRoute
                (
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}"
                );
            });
        }
    }
}
