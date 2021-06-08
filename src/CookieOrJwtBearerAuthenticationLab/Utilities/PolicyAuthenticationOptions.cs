#nullable enable

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    public class PolicyAuthenticationOptions
    {
        // Properties
        public string? DefaultScheme { get; set; }

        public Func<HttpContext, string>? AuthenticateSchemePolicy { get; set; }

        public Func<HttpContext, string>? ChallengeSchemePolicy { get; set; }

        public Func<HttpContext, string>? ForbidSchemePolicy { get; set; }

        public Func<HttpContext, string>? SignInSchemePolicy { get; set; }

        public Func<HttpContext, string>? SignOutSchemePolicy { get; set; }
    }
}
