using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entites.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string PaymentIntentId) : base(o=>o.PaymentIntentId==PaymentIntentId)
        {
        }
    }
}