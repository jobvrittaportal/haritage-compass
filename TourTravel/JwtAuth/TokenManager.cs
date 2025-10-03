using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TourTravel.JwtAuth
{
    public class TokenManager : ITokenManager
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] secretKey;
        public TokenManager()
        {
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes("shreyasingh");
        }

        public string NewToken(string ID)
        {
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity([new Claim("ID", ID)]),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtString = tokenHandler.WriteToken(token);
            return jwtString;
        }
    }
}
