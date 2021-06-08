#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    public partial class PolicyAuthenticationDefaults
    {
        // Properties
        public const string AuthenticationScheme = "Policy";
    }

    public partial class PolicyAuthenticationDefaults
    {
        // Properties
        internal const string AuthenticatePolicyScheme = "__AuthenticatePolicy";

        internal const string ChallengePolicyScheme = "__ChallengePolicy";

        internal const string ForbidPolicyScheme = "__ForbidPolicy";

        internal const string SignInPolicyScheme = "__SignInPolicy";

        internal const string SignOutPolicyScheme = "__SignOutPolicy";
    }
}
