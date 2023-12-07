using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entites.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Core.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            
        }

        public string CreateToken(AppUser user)
        {
            var claims=new List<Claim>
           {
             new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.GivenName,user.DisplayName),
           };
           
           var creds=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
       
           var tokenDescriptor=new SecurityTokenDescriptor
           {
                Subject =new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(7),
                SigningCredentials=creds,
                Issuer=_config["Token:Issuer"]
           };
           var tokenhandler=new JwtSecurityTokenHandler();
           var token = tokenhandler.CreateToken(tokenDescriptor);
           return tokenhandler.WriteToken(token);
        }
    }
}