using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthLab
{
    // Class
    public class JwtHelper
    {
        // Methods
        public string CreateToken(string userId, string userName, List<string> roleList, int expireMinutes = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(userName) == true) throw new ArgumentException(nameof(userName));
            if (roleList == null) throw new ArgumentException(nameof(roleList));

            #endregion

            // Config
            var issuer = "JwtAuthLab";
            var signKey = "12345678901234567890123456789012";

            // Claim
            var claims = new List<Claim>();
            {
                // JwtRegisteredClaimNames
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // JWT ID
                claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer)); // Issuer
                
                // ClaimTypes
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId)); // UserId
                claims.Add(new Claim(ClaimTypes.Name, userName)); // UserName
                {
                    // Role
                    foreach(var role in roleList)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
            }

            // TokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claims),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(expireMinutes), // 逾期時間

                // Signing
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)),
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                ),                
            };

            // SerializeToken
            var tokenHandler = new JwtSecurityTokenHandler();
            var serializeToken = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            // Return
            return serializeToken;
        }
    }
}
