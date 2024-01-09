
using Core.Entites;
using Core.Entites.OrderAggregate;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreatedOrUpdatedPaymentIntent(string basketId);
        Task<Order> UpdateOrderPaymentSucceeded(string payment_intentId);
        Task<Order> UpdateOrderPaymentFailed(string payment_intentId);


    }
}