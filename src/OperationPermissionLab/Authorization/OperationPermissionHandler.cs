using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OperationPermissionLab
{
    public class OperationPermissionHandler : AuthorizationHandler<OperationPermissionRequirement>
    {
        // Fields
        private OperationPermissionRepository _operationPermissionRepository = null;


        // Constructors
        public OperationPermissionHandler(OperationPermissionRepository operationPermissionRepository)
        {
            #region Contracts

            if (operationPermissionRepository == null) throw new ArgumentException();

            #endregion

            // Default
            _operationPermissionRepository = operationPermissionRepository;
        }


        // Methods
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationPermissionRequirement requirement)
        {
            #region Contracts

            if (context == null) throw new ArgumentException();
            if (requirement == null) throw new ArgumentException();

            #endregion

            // 純粹展示用，實際專案可以用RBAC+快取等等機制，來減少資料庫查詢
            // Require
            if (context.User == null) return Task.CompletedTask;
            if (context.User.Identity == null) return Task.CompletedTask;
            if (context.User.Identity.IsAuthenticated == false) return Task.CompletedTask;

            // UserName
            var userName = context.User.Identity.Name;
            if (string.IsNullOrEmpty(userName) == true) throw new InvalidOperationException($"{nameof(userName)}=null");

            // OperationPermissionList
            var operationPermissionList = _operationPermissionRepository.FindAllByUserName(userName);
            if (operationPermissionList == null) throw new InvalidOperationException($"{nameof(operationPermissionList)}=null");

            // OperationPermissionRequirement
            var operationPermission = operationPermissionList.FirstOrDefault(o => o.OperationName == requirement.OperationName);
            if (operationPermission != null) context.Succeed(requirement);

            // Return
            return Task.CompletedTask;
        }
    }
}
