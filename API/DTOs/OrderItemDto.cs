using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class OrderItemDto
    {
        public int ProductId{set;get;}
        public string ProductName{set;get;}
        public string PictureUrl{set;get;}
        public decimal Price{set;get;} 
       public int Quantity{set;get;} 

        
    }
}