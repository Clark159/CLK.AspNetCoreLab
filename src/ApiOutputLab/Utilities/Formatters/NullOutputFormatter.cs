using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOutputLab
{
    public class NullOutputFormatter : IOutputFormatter
    {
        // Methods
        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Void
            if (context.ObjectType == typeof(void) || context.ObjectType == typeof(Task))
            {
                return true;
            }

            // Null
            if(context.Object == null)
            {
                return true;
            }

            // Return
            return false;
        }

        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Content
            context.HttpContext.Response.ContentLength = 0;

            // Return
            return Task.CompletedTask;
        }
    }
}
