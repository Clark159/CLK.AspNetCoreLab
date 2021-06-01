using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthLab
{
    public class CustomAuthenticationHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        // Constants
        private const string _cookieName = "custom-cookie";


        // Fields
        private AuthenticationScheme _scheme = null;

        private HttpContext _httpContext = null;


        // Constructors
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            #region Contracts

            if (scheme == null) throw new ArgumentException(nameof(scheme));
            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Default
            _scheme = scheme;
            _httpContext = context;

            // Return
            return Task.CompletedTask;
        }


        // Methods
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties = null)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion
                        
            // Cookie
            _httpContext.Response.Cookies.Append(_cookieName, user.Identity.Name);

            // Return
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties = null)
        {
            // Cookie
            _httpContext.Response.Cookies.Delete(_cookieName);

            // Return
            return Task.CompletedTask;
        }


        public Task<AuthenticateResult> AuthenticateAsync()
        {
            // Name
            var name = _httpContext.Request.Cookies[_cookieName];
            if (string.IsNullOrEmpty(name) ==true) return Task.FromResult(AuthenticateResult.NoResult());

            // ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, name)
            }
            , _scheme.Name);

            // Ticket
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), _scheme.Name);

            // Return
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public Task ChallengeAsync(AuthenticationProperties properties = null)
        {
            // Redirect
            _httpContext.Response.Redirect("/login");

            // Return
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties = null)
        {
            // StatusCode
            _httpContext.Response.StatusCode = 403;

            // Return
            return Task.CompletedTask;
        }
    }
}
