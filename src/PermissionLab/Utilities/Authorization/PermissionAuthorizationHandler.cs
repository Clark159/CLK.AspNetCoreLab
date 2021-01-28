using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PermissionLab
{
    public class PermissionAuthorizationHandler : IAuthorizationHandler
    {
        // Fields
        private readonly PermissionRepository _permissionRepository = null;


        // Constructors
        public PermissionAuthorizationHandler(PermissionRepository permissionRepository)
        {
            #region Contracts

            if (permissionRepository == null) throw new ArgumentException(nameof(permissionRepository));

            #endregion

            // Default
            _permissionRepository = permissionRepository;
        }


        // Methods
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // IsAuthenticated
            if (context.User.Identity.IsAuthenticated == false) return Task.CompletedTask;

            // UserId
            var userId = (context.User.Identity as ClaimsIdentity)?.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId) == true) { context.Fail(); return Task.CompletedTask; }

            // PermissionList
            var permissionList = _permissionRepository.FindAllByUserId(userId);
            if (permissionList == null) throw new InvalidOperationException("permissionList=null");

            // Request
            HttpRequest request = null;
            if (request == null) request = (context.Resource as HttpContext)?.Request;
            if (request == null) request = (context.Resource as AuthorizationFilterContext)?.HttpContext?.Request;

            // Path
            var path = request.Path.Value;
            if (string.IsNullOrEmpty(path) == true) path = string.Empty;

            // IsWildcardMatch
            foreach (var permission in permissionList)
            {
                if (path.IsWildcardMatch(permission.PathPatten) == true)
                {
                    return Task.CompletedTask;
                }
            }

            // Return
            { context.Fail(); return Task.CompletedTask; }
        }
    }
}
