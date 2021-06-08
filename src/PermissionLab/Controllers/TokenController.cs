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
    public partial class TokenController : Controller
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
    public partial class TokenController : Controller
    {
        // Methods
        [AllowAnonymous]
        public ActionResult<GetTokenResultModel> GetToken([FromBody] GetTokenActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // GetToken
            return (new GetTokenResultModel()
            {
                Token = _jwtHelper.CreateToken(actionModel.UserId, actionModel.Username, new List<string>() { "Admin", "User" })
            });
        }


        // Class
        public class GetTokenActionModel
        {
            // Properties
            public string UserId { get; set; }

            public string Username { get; set; }
        }

        public class GetTokenResultModel
        {
            // Properties
            public string Token { get; set; }
        }
    }
}
