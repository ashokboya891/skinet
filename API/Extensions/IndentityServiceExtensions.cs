
using System.Text;
using Core.Entites.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IndentityServiceExtensions
    {
      public static IServiceCollection AddIdentityServices(this IServiceCollection services,
      IConfiguration config)
      {
        
        services.AddDbContext<AppIdentityDbContext>(opt=>
        {
            opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
        });
        services.AddIdentityCore<AppUser>(opt=>{
          // we can identity options here like email ,password length,timeoutlogin ,attempts failer more

        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager<SignInManager<AppUser>>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(Options =>
          {
            Options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
              ValidIssuer = config["Token:Issuer"],
              ValidateIssuer=true,
              ValidateAudience = false,

            };

          });

      services.AddAuthorization();
        return services;
      }
    }
}