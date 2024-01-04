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
        public int? DeliveryMethodId{set;get;}
        public string  ClientSecret{set;get;}
        public string PaymentIntentId{set;get;}
        public decimal shippingPrice{set;get;}

    }
}