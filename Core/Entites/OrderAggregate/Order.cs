using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order( IReadOnlyList<OrderItem> orderItems,string buyerEmail, Address shipToAddress,
        DeliveryMethod deliveryMethod, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string BuyerEmail{set;get;}
        public DateTime OrderDate{set;get;}=DateTime.UtcNow;
        public Address ShipToAddress{set;get;}
        public DeliveryMethod DeliveryMethod{set;get;}
        public IReadOnlyList<OrderItem> OrderItems{set;get;}
        public decimal Subtotal{set;get;}
        public OrderStatus Status{set;get;}=OrderStatus.Pending;
        public string PaymentIntentId{set;get;}
        public decimal GetTotal()
        {
            return Subtotal+DeliveryMethod.Price;
        }


    }
}