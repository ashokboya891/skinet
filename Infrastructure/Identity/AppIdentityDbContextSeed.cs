
using Core.Entites.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user=new AppUser
                {
                    DisplayName="Ram",
                    Email="ram@bha.com",
                    UserName="ram@bha.com",
                    Address=new Address
                    {
                        FirstName="Ram",
                        LastName="seeta",
                        Street="rajamandiram",
                        City="ayodya",
                        State="UP",
                        ZipCode="218343"
                    }

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}