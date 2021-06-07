using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuthenticationLab
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // Methods  
        public async Task<ActionResult> Login(string username = null, string returnUrl = @"/")
        {
            // Require
            if (string.IsNullOrEmpty(username) == true) return View();
            if (string.IsNullOrEmpty(returnUrl) == true) returnUrl = @"/";
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // ClaimsPrincipal
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, username));
            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
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
}
