using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public class DeliveryMethod:BaseEntity
    {
        public string ShortName{set;get;}
        public string DeliveryTime{set;get;}
        public string Description{set;get;}
        public decimal Price{set;get;}


    }
}