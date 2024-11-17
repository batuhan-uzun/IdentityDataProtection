using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityDataProtection.Jwt
{
    public class JwtHelper
    {
        public static string GenerateJwt(JwtDto jwtInfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id",jwtInfo.Id.ToString()),
                new Claim("Email",jwtInfo.Email),
                new Claim("UserRole",jwtInfo.UserRole.ToString()),
                new Claim(ClaimTypes.Role, jwtInfo.UserRole.ToString()) // => For authentication admin, user etc.
            };

            var tokenExpireTime = DateTime.Now.AddMinutes(jwtInfo.ExpireMinutes);

            var tokenDescriptor = new JwtSecurityToken(jwtInfo.Issuer, jwtInfo.Audience, claims, null, tokenExpireTime, credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
