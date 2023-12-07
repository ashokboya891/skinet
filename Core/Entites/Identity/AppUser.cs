using Microsoft.AspNetCore.Identity;

namespace Core.Entites.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName{set;get;}
        public Address Address{set;get;}
    }
}