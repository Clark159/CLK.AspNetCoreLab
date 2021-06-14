using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public class ApplicationIdentity : ClaimsIdentity
    {
        // Constants
        public const string DefaultUserIdClaimType = ClaimTypes.NameIdentifier;

        public const string DefaultUserNameClaimType = ClaimTypes.Name;

        public const string DefaultNickNameClaimType = "​http://openid-custom/identity/claims/nickname";


        // Constructors
        public ApplicationIdentity(string authenticationType) : base(authenticationType) { }

        public ApplicationIdentity(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType) { }


        // Properties
        public string UserIdType { get; set; } = DefaultUserIdClaimType;

        public string UserNameType { get; set; } = DefaultUserNameClaimType;

        public string NickNameType { get; set; } = DefaultNickNameClaimType;


        public virtual string UserId { get { return this.FindFirstValue(this.UserIdType); } }

        public virtual string UserName { get { return this.FindFirstValue(this.UserNameType); } }

        public virtual string NickName { get { return this.FindFirstValue(this.NickNameType); } }


        // Methods
        public string FindFirstValue(string type)
        {
            #region Contracts

            if (string.IsNullOrEmpty(type) == true) throw new ArgumentException(nameof(type));

            #endregion

            // Find
            var claim = this.FindFirst(this.UserIdType);
            if (claim == null) return null;

            // Return
            return claim.Value;
        }
    }
}
