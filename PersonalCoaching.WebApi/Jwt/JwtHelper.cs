using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalCoaching.WebApi.Jwt
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtinfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtinfo.SecretKey));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtClaimNames.Id, jwtinfo.Id.ToString()),
                new Claim(JwtClaimNames.FirstName, jwtinfo.FirstName),
                new Claim(JwtClaimNames.LastName, jwtinfo.LastName),
                new Claim(JwtClaimNames.Email, jwtinfo.Email),
                new Claim(JwtClaimNames.UserType, jwtinfo.UserType.ToString()),

                new Claim(ClaimTypes.Role, jwtinfo.UserType.ToString())

            };

            var expireTime = DateTime.Now.AddMinutes(jwtinfo.ExpireMinutes);

            var tokenDescriptor = new JwtSecurityToken(jwtinfo.Issuer, jwtinfo.Audience, claims, null, expireTime, credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
