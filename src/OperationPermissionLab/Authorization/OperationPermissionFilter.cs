using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationPermissionLab
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OperationPermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        // Constructors
        public OperationPermissionFilter(string operationName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(operationName) == true) throw new ArgumentException();

            #endregion

            // Default
            this.OperationName = operationName;
        }


        // Properties
        public string OperationName { get; set; }


        // Methods
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException();

            #endregion

            // AuthorizationService
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            if (authorizationService == null) throw new InvalidOperationException($"{nameof(authorizationService)}=null");

            // AuthorizeAsync
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new OperationPermissionRequirement() { OperationName = this.OperationName });
            if (authorizationResult.Succeeded == false) context.Result = new ForbidResult();
        }
    }
}
