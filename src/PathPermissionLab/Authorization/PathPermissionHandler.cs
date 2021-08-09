using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PathPermissionLab
{
    public class PathPermissionHandler : IAuthorizationHandler
    {
        // Fields
        private PathPermissionRepository _pathPermissionRepository = null;


        // Constructors
        public PathPermissionHandler(PathPermissionRepository pathPermissionRepository)
        {
            #region Contracts

            if (pathPermissionRepository == null) throw new ArgumentException();

            #endregion

            // Default
            _pathPermissionRepository = pathPermissionRepository;
        }


        // Methods
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // 純粹展示用，實際專案可以用RBAC+快取等等機制，來減少資料庫查詢
            // Require
            if (context.User == null) return Task.CompletedTask;
            if (context.User.Identity == null) return Task.CompletedTask;
            if (context.User.Identity.IsAuthenticated == false) return Task.CompletedTask;

            // UserName
            var userName = context.User.Identity.Name;
            if (string.IsNullOrEmpty(userName) == true) throw new InvalidOperationException($"{nameof(userName)}=null");

            // PathPermissionList
            var pathPermissionList = _pathPermissionRepository.FindAllByUserName(userName);
            if (pathPermissionList == null) throw new InvalidOperationException($"{nameof(pathPermissionList)}=null");

            // Request
            HttpRequest request = null;
            if (request == null) request = (context.Resource as HttpContext)?.Request;
            if (request == null) request = (context.Resource as AuthorizationFilterContext)?.HttpContext?.Request;
            if (request == null) throw new InvalidOperationException($"{nameof(request)}=null");

            // Path
            var path = request.Path.Value;
            if (string.IsNullOrEmpty(path) == true) path = string.Empty;

            // IsMatch
            foreach (var pathPermission in pathPermissionList)
            {
                if (pathPermission.IsMatch(path) == true)
                {
                    return Task.CompletedTask;
                }
            }

            // NotMatch
            { context.Fail(); return Task.CompletedTask; }
        }
    }
}
