using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;

namespace Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        public OrderService(IGenericRepository<Order> orderRepo,IGenericRepository<DeliveryMethod> dmRepo,
        IGenericRepository<Product> productRepo,IBasketRepository basketRepository)
        {
            _dmRepo = dmRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _basketRepository = basketRepository;
            
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingaddress)
        {
            //get basket from repo
            var basket=await _basketRepository.GetBasketAsync(basketId);
            //get items from the product repo
            var items=new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem= await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered=new ProductItemOrdered(productItem.Id,productItem.Name,
                productItem.PictureUrl);
                var orderItem=new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            }
            //get delivery method for repo
            var deliverymethod=await _dmRepo.GetByIdAsync(deliveryMethodId);
            //calc subtotal
            var subtotal=items.Sum(items=>items.Price*items.Quantity);
            //creater order,
            var order= new Order(items,buyerEmail,shippingaddress,deliverymethod,subtotal);
            //TODO:---save to db,


            //return order
            return order;


        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}