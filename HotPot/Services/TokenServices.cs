using HotPot.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotPot.Interfaces;

namespace HotPot.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly string _keyString;
        private readonly SymmetricSecurityKey _key;

        public TokenServices(IConfiguration configuration)
        {
            _keyString = configuration["SecretKey"].ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyString));
        }

        public async Task<string> GenerateToken(LoginUserDTO loginUser)
        {
            string token = string.Empty;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,loginUser.UserName),
                new Claim(ClaimTypes.Role,loginUser.Role)
            };

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Today.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(myToken);

            return token;
        }
    }
}