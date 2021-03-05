using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareLab
{
    public class Startup
    {
        // Methods
        public void Configure(IApplicationBuilder app)
        {
            #region Contracts

            if (app == null) throw new ArgumentException(nameof(app));

            #endregion

            // Custom
            app.UseCustom();
            
            // Run
            app.Run(async context =>
            {
                await context.Response.WriteAsync("EndMiddleware Run \r\n");
            });
        }
    }
}
