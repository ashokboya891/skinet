

namespace Core.Entites
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id{set;get;}
        public List<BasketItem> Items{set;get;}=new List<BasketItem>();
        public int? DeliveryMethodId{set;get;}
        public string  ClientSecret{set;get;}
        public string PaymentIntentId{set;get;}
        public decimal ShippingPrice{set;get;}

        
    }

}