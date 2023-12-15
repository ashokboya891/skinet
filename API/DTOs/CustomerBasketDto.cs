using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id{set;get;}
        public List<BasketItemDto> Items{set;get;}=new List<BasketItemDto>();
    }
}