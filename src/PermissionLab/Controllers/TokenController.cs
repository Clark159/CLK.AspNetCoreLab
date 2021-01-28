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
}
