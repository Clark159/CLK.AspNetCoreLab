using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PermissionLab
{
    public partial class HomeController : ControllerBase
    {
        // Methods
        [Authorize]
        public GetUserResultModel GetUser001([FromBody] GetUserActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Return
            return this.GetUser(actionModel);
        }

        [Authorize]
        public GetUserResultModel GetUser002([FromBody] GetUserActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Return
            return this.GetUser(actionModel);
        }

        private GetUserResultModel GetUser(GetUserActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // MyUser
            var user = new MyUser();
            user.IsAuthenticated = this.User.Identity.IsAuthenticated;
            user.AuthenticationType = this.User.Identity.AuthenticationType;
            user.UserId = (this.User.Identity as ClaimsIdentity)?.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            user.UserName = (this.User.Identity as ClaimsIdentity)?.Name;
            user.RoleList = (this.User.Identity as ClaimsIdentity)?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value).ToList();

            // GetUser
            return (new GetUserResultModel()
            {
                User = user
            });
        }


        // Class
        public class GetUserActionModel
        {
            // Properties
            
        }

        public class GetUserResultModel
        {
            // Properties
            public MyUser User { get; set; }
        }

        public class MyUser
        {
            // Properties
            public bool IsAuthenticated { get; set; }

            public string? AuthenticationType { get; set; }

            public string? UserId { get; set; }

            public string? UserName { get; set; }

            public List<string> RoleList { get; set; }


            // Methods
            public bool IsInRole(string role)
            {
                #region Contracts

                if (string.IsNullOrEmpty(role) == true) throw new ArgumentException(nameof(role));

                #endregion

                // Contains
                if (RoleList.Contains(role) == true)
                {
                    return true;
                }

                // Return
                return false;
            }
        }
    }
}
