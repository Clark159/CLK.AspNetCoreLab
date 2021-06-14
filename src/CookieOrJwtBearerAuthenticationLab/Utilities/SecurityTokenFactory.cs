using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CookieOrJwtBearerAuthenticationLab
{
    public class SecurityTokenFactory
    {
        // Fields
        private readonly JwtSecurityTokenHandler _tokenHandler = null;

        private readonly string _signKey = null;

        private readonly string _issuer = null;

        private readonly int _expireMinutes = 30;


        // Constructors
        public SecurityTokenFactory(string issuer, string signKey, int expireMinutes = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(issuer) == true) throw new ArgumentException(nameof(issuer));
            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException(nameof(signKey));

            #endregion

            // Default
            _tokenHandler = new JwtSecurityTokenHandler();
            _issuer = issuer;
            _signKey = signKey;
            _expireMinutes = expireMinutes;
        }


        // Methods
        public string CreateEncodedJwt(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // ClaimList
            var claimList = new List<Claim>(identity.Claims);
            claimList.Add(new Claim(ClaimTypes.Name, identity.Name)); // UserName

            // CreateEncodedJwt
            return this.CreateEncodedJwt(claimList);
        }
        
        public string CreateEncodedJwt(IEnumerable<Claim> claims)
        {
            #region Contracts

            if (claims == null) throw new ArgumentException(nameof(claims));

            #endregion

            // ClaimList
            var claimList = new List<Claim>(claims);
            {
                // JWT ID
                claimList.RemoveAll(claim => claim.Type == JwtRegisteredClaimNames.Jti);
                claimList.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                // Issuer
                claimList.RemoveAll(claim => claim.Type == JwtRegisteredClaimNames.Iss);
                claimList.Add(new Claim(JwtRegisteredClaimNames.Iss, _issuer)); 
            }

            // TokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claimList),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(_expireMinutes), // 逾期時間

                // Signing
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signKey)),
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                ),
            };

            // TokenString
            var tokenString = _tokenHandler.CreateEncodedJwt(tokenDescriptor);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return tokenString;
        }
    }
}
