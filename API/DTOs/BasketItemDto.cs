using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id{set;get;}
        [Required]

        public string ProductName{set;get;}
        [Required]
        [Range(0.1,Double.MaxValue,ErrorMessage ="price must be greater than 0")]
        public decimal Price{set;get;}
        [Required]
        [Range(1,Double.MaxValue,ErrorMessage ="Quantity must be 1")]

        public int Quantity{set;get;}
        [Required]

        public string PictureUrl{set;get;}
        [Required]

        public string Brand{set;get;}
        [Required]

        public string Type{set;get;}
        
        
    }
}