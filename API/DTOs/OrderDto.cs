using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites.OrderAggregate;

namespace API.DTOs
{
    public class OrderDto
    {
        public string BasketID{set;get;}
        public int deliveryMethodId{set;get;}
        public AddressDto ShipToAddress{set;get;}
    }
}