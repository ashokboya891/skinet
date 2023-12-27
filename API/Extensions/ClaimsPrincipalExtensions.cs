using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static String RetrieveEmailFromPrinciple(this ClaimsPrincipal user)
        {
            // return user?.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email).Value; instead of this line we can use below line also
            return user.FindFirstValue(ClaimTypes.Email);

        }
        
    }
}