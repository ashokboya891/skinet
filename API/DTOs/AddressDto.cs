

using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDto
    {
        [Required]
        
        public string FirstName{set;get;}
        [Required]

        public string LastName{set;get;}
        [Required]

        public string Street{set;get;}
        [Required]

        public string City{set;get;}
        [Required]

        public string State{set;get;}
        [Required]

        public string Zipcode{set;get;}

    }
}