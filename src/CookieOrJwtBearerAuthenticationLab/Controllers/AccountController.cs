using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    [AllowAnonymous]
    public partial class AccountController : Controller
    {
        // Fields
        private readonly SecurityTokenFactory _tokenFactory;


        // Constructors
        public AccountController(SecurityTokenFactory tokenFactory)
        {
            #region Contracts

            if (tokenFactory == null) throw new ArgumentException(nameof(tokenFactory));

            #endregion

            // Default
            _tokenFactory = tokenFactory;
        }


        // Methods  
        public async Task<ActionResult> Login(string username = null, string password = null, string returnUrl = @"/")
        {
            // Require
            if (string.IsNullOrEmpty(username) == true) return View();
            if (string.IsNullOrEmpty(returnUrl) == true) returnUrl = @"/";
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Validate
            // ...

            // ClaimsPrincipal
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, username));
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

            // SignIn
            await this.HttpContext.SignInAsync(claimsPrincipal);

            // Redirect
            return this.Redirect(returnUrl);
        }

        public async Task<ActionResult> Logout()
        {
            // Require
            if (this.User.Identity.IsAuthenticated == false) return this.Redirect(@"/");

            // SignIn
            await this.HttpContext.SignOutAsync();

            // Redirect
            return this.Redirect(@"/");
        }
    }

    // GetUser
    public partial class AccountController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult<GetUserResultModel> GetUser([FromBody] GetUserActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // UserModel
            var user = new UserModel();
            user.Username = this.User.Identity.Name;
            user.AuthenticationType = this.User.Identity.AuthenticationType;

            // Return
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
            public UserModel User { get; set; }
        }

        public class UserModel
        {
            // Properties
            public string Username { get; set; }

            public string AuthenticationType { get; set; }
        }
    }

    // GetToken
    public partial class AccountController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult<GetTokenResultModel> GetToken([FromBody] GetTokenActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // ClaimIdentity
            var claimIdentity = this.User.Identity as ClaimsIdentity;
            if (claimIdentity == null) throw new InvalidOperationException($"{nameof(claimIdentity)}=null");
            
            // TokenString
            var tokenString = _tokenFactory.CreateEncodedJwt(claimIdentity);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return (new GetTokenResultModel()
            {
                Token = tokenString
            });
        }


        // Class
        public class GetTokenActionModel
        {
            // Properties
           
        }

        public class GetTokenResultModel
        {
            // Properties
            public string Token { get; set; }
        }
    }

    // GetTokenByPassword
    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        public ActionResult<GetTokenByPasswordResultModel> GetTokenByPassword([FromBody] GetTokenByPasswordActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Require
            if (string.IsNullOrEmpty(actionModel.Username) == true) throw new InvalidOperationException($"{nameof(actionModel.Username)}=null");
            if (string.IsNullOrEmpty(actionModel.Password) == true) throw new InvalidOperationException($"{nameof(actionModel.Password)}=null");

            // Validate
            // ...

            // ClaimIdentity
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, actionModel.Username));

            // TokenString
            var tokenString = _tokenFactory.CreateEncodedJwt(claimIdentity.Claims);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return (new GetTokenByPasswordResultModel()
            {
                Token = tokenString
            });
        }


        // Class
        public class GetTokenByPasswordActionModel
        {
            // Properties
            public string Username { get; set; }

            public string Password { get; set; }
        }

        public class GetTokenByPasswordResultModel
        {
            // Properties
            public string Token { get; set; }
        }
    }
}
