using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OperationPermissionLab
{
    public class AccountController : Controller
    {
        // Methods
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = @"/")
        {
            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Return
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string userName, string returnUrl = @"/")
        {
            #region Contracts

            if (string.IsNullOrEmpty(userName) == true) throw new ArgumentException(nameof(userName));

            #endregion

            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // ClaimsPrincipal
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

            // SignIn
            await this.HttpContext.SignInAsync(claimsPrincipal);

            // Redirect
            return this.Redirect(returnUrl);
        }

        [AllowAnonymous]
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
}
