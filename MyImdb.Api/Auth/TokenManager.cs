
using Microsoft.IdentityModel.Tokens;
using MyImdb.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyImdb.Api.Auth
{
    public class TokenManager : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<GenerateTokenResponse> GenerateTokenAsync(AppUser user, List<string> roles)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]));

            var now = DateTime.UtcNow;

            var claims = new List<Claim>()
            {
             new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
             new Claim(ClaimTypes.Name,user.UserName),
             new Claim(ClaimTypes.Email,user.Email)
            };

            foreach (var item in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }


            JwtSecurityToken jwt = new JwtSecurityToken(
             issuer: _configuration["AppSettings:ValidIssuer"],
             audience: _configuration["AppSettings:ValidAudience"],
             claims: claims,
             notBefore: now,
             expires: now.Add(TimeSpan.FromDays(7)),
             signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new GenerateTokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = now.Add(TimeSpan.FromDays(7))
            });
        }
    }
}
