using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RazorViewLocationLab
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

            // RazorViewEngine
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // ViewLocationFormats
                {
                    // Add
                    options.ViewLocationFormats.Insert(0, "/Modules/Views/{1}/{0}.cshtml");

                    // Display
                    Debug.WriteLine("\n\n\n");
                    foreach (var viewLocationFormat in options.ViewLocationFormats)
                    {
                        Debug.WriteLine(viewLocationFormat);
                    }
                    Debug.WriteLine("\n\n\n");
                }
            });
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
