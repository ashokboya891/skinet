

using Core.Entites.Identity;
using Core.Entites.OrderAggregate;

namespace API.DTOs
{
    public class OrderToReturnDto
    {
        public int Id{set;get;}
         public string BuyerEmail{set;get;}
        public DateTime OrderDate{set;get;}=DateTime.UtcNow;
        public Core.Entites.OrderAggregate.Address ShipToAddress{set;get;}
        public string DeliveryMethod{set;get;}
        public decimal ShippingPrice{set;get;}
        public IReadOnlyList<OrderItemDto> OrderItems{set;get;}
        public decimal Subtotal{set;get;}
        public decimal Total{set;get;}
        public string Status{set;get;}
        
    }
}