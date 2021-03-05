using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingEndpointsLab
{
    public class OutputMiddleware
    {
        // Fields
        private readonly RequestDelegate _next = null;

        private readonly string _outputName = null;


        // Constructors
        public OutputMiddleware(RequestDelegate next, string outputName)
        {
            #region Contracts

            if (next == null) throw new ArgumentException(nameof(next));
            if (string.IsNullOrEmpty(outputName)==true) throw new ArgumentException(nameof(outputName));

            #endregion

            // Default
            _next = next;
            _outputName = outputName;
        }


        // Methods
        public async Task InvokeAsync(HttpContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Endpoint
            var endpoint = context.GetEndpoint();
            if (endpoint == null) await context.Response.WriteAsync($"OutputMiddleware: {_outputName}, endpoint=null. \r\n");
            if (endpoint != null) await context.Response.WriteAsync($"OutputMiddleware: {_outputName}, endpoint={endpoint.DisplayName}. \r\n");

            // Next
            await _next(context);
        }
    }

    public static class OutputMiddlewareExtensions
    {
        // Methods
        public static void UseOutput(this IApplicationBuilder app, string outputName)
        {
            #region Contracts

            if (app == null) throw new ArgumentException(nameof(app));
            if (string.IsNullOrEmpty(outputName) == true) throw new ArgumentException(nameof(outputName));

            #endregion

            // Add
            app.UseMiddleware<OutputMiddleware>(outputName);
        }
    }
}
