using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
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
            services.AddAuthentication(options =>
            {
                // DefaultScheme
                options.DefaultScheme = ApplicationCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(ApplicationCookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Action
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/Login");
            })
            .AddCookie(ExternalCookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Action
                options.AccessDeniedPath = new PathString("/Account/Login");
            })
            .AddGoogle(GoogleOAuthAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Client
                options.ClientId = @"";                                                // Google-Developers Site : Get ClientId
                options.ClientSecret = @"";                                            // Google-Developers Site : Get ClientSecret
                options.CallbackPath = new PathString("/Account/Google-OAuth-SignIn"); // Google-Developers Site : Set RedirectUri = https://localhost:44311/Account/Google-OAuth-SignIn

                // Scheme
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddFacebook(FacebookOAuthAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Client
                options.ClientId = @"";                                                  // Facebook-Developers Site : Get ClientId
                options.ClientSecret = @"";                                              // Facebook-Developers Site : Get ClientSecret
                options.CallbackPath = new PathString("/Account/Facebook-OAuth-SignIn"); // Facebook-Developers Site : Set RedirectUri = https://localhost:44311/Account/Facebook-OAuth-SignIn

                // Scheme
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            });

            // UserContext
            services.AddSingleton<UserContext>();
            services.AddSingleton<UserRepository, MockUserRepository>();
            services.AddSingleton<UserLoginRepository, MockUserLoginRepository>();
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
