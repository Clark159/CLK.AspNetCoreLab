using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JwtAuthLab
{
    public partial class TokenController : ControllerBase
    {
        // Fields
        private readonly JwtHelper _jwtHelper;


        // Constructors
        public TokenController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }        
    }

    // GetToken
    public partial class TokenController : ControllerBase
    {
        // Methods
        [AllowAnonymous]
        public GetTokenResultModel GetToken([FromBody] GetTokenActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // GetToken
            return (new GetTokenResultModel()
            {
                Token = _jwtHelper.CreateToken(actionModel.UserId, actionModel.UserName, new List<string>() { "Admin", "User" })
            });
        }


        // Class
        public class GetTokenActionModel
        {
            // Properties
            public string UserId { get; set; }

            public string UserName { get; set; }
        }

        public class GetTokenResultModel
        {
            // Properties
            public string Token { get; set; }
        }
    }


    // GetUser
    public partial class TokenController : ControllerBase
    {
        // Methods
        //[Authorize]
        public GetUserResultModel GetUser([FromBody] GetUserActionModel actionModel)
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
            user.RoleList = (this.User.Identity as ClaimsIdentity)?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y=> y.Value).ToList();
           
            // GetUser
            return (new GetUserResultModel()
            {
                User = user
            }) ;
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
