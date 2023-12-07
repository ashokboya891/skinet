using Core.Entites.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    };
}