using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        //after adding uow,Iuow we commented     
        // private readonly IGenericRepository<Product> _productRepo;
        // private readonly IGenericRepository<Order> _orderRepo;
        // private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            
        }
        //after adding uow,Iuow we commented        
        // public OrderService(IGenericRepository<Order> orderRepo,IGenericRepository<DeliveryMethod> dmRepo,
        // IGenericRepository<Product> productRepo,IBasketRepository basketRepository)
        // {
        //     _dmRepo = dmRepo;
        //     _orderRepo = orderRepo;
        //     _productRepo = productRepo;
        //     _basketRepository = basketRepository;
            
        // }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingaddress)
        {
            //get basket from repo
            var basket=await _basketRepository.GetBasketAsync(basketId);
            //get items from the product repo
            var items=new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem= await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered=new ProductItemOrdered(productItem.Id,productItem.Name,
                productItem.PictureUrl);
                var orderItem=new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            }
            //get delivery method for repo
            var deliverymethod=await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //calc subtotal
            var subtotal=items.Sum(items=>items.Price*items.Quantity);


            //check to see if order exists
            var spec=new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order=await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(order!=null)
            {
                order.ShipToAddress=shippingaddress;
                order.DeliveryMethod=deliverymethod;
                order.Subtotal=subtotal;
                _unitOfWork.Repository<Order>().Update(order);
            }
            else{

            //creater order,
            order= new Order(items,buyerEmail,shippingaddress,deliverymethod,subtotal,basket.PaymentIntentId);
            _unitOfWork.Repository<Order>().Add(order);
            }



            //TODO:---save to db,
            var result=await _unitOfWork.Complete();
            if(result<=0) return null;


            //delete basket after adding into db
            // await _basketRepository.DeleteBasketAsync(basketId);

            //return order
            return order;


        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
          return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public  async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec=new OrdersWithItemsAndOrderingSpecification(id,buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec=new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}