using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareLab
{
    public class CustomMiddleware
    {
        // Fields
        private readonly RequestDelegate _next = null;


        // Constructors
        public CustomMiddleware(RequestDelegate next)
        {
            #region Contracts

            if (next == null) throw new ArgumentException(nameof(next));

            #endregion

            // Default
            _next = next;
        }


        // Methods
        public async Task InvokeAsync(HttpContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Execute
            await context.Response.WriteAsync("CustomMiddleware Start \r\n");
            await _next(context);
            await context.Response.WriteAsync("CustomMiddleware End \r\n");
        }
    }

    public static class CustomMiddlewareExtensions
    {
        // Methods
        public static void UseCustom(this IApplicationBuilder app)
        {
            #region Contracts

            if (app == null) throw new ArgumentException(nameof(app));

            #endregion

            // Add
            app.UseMiddleware<CustomMiddleware>();
        }
    }
}
