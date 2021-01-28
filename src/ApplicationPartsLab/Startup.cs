using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApplicationPartsLab
{
    public class Startup
    {
        // Fields
        private readonly string _moduleName = @"ApplicationPartsLab.Module";

        private readonly Assembly _moduleAssembly = null;

        private readonly Assembly _moduleViewAssembly = null;


        // Constructors
        public Startup()
        {
            // EntryDirectory
            var entryDirectory = AppContext.BaseDirectory;
            if (Directory.Exists(entryDirectory) == false) throw new InvalidOperationException($"{nameof(entryDirectory)}=null");

            // ModuleAssembly
            _moduleAssembly = Assembly.LoadFile(Path.Combine(entryDirectory, $"{_moduleName}.dll"));
            if (_moduleAssembly == null) throw new InvalidOperationException($"{nameof(_moduleAssembly)}=null");

            // ModuleViewAssembly
            _moduleViewAssembly = Assembly.LoadFile(Path.Combine(entryDirectory, $"{_moduleName}.Views.dll"));
            if (_moduleViewAssembly == null) throw new InvalidOperationException($"{nameof(_moduleViewAssembly)}=null");
        }


        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            
            // MvcBuilder
            var mvcBuilder = services.AddMvc();
            {
                // ApplicationPart                
                mvcBuilder.AddApplicationPart(_moduleAssembly);
                mvcBuilder.AddApplicationPart(_moduleViewAssembly);
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

            // StaticFiles
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new CompositeFileProvider
                (
                   env.WebRootFileProvider,
                   new EmbeddedWebAssetsFileProvider(_moduleAssembly)
                )
            });

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

                // Page
                endpoints.MapRazorPages();
            });
        }
    }
}
