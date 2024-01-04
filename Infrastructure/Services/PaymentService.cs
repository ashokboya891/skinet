using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entites.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        public PaymentService(IUnitOfWork unitOfWork,IBasketRepository basketRepository,IConfiguration configuration)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task<CustomerBasket> CreatedOrUpdatedPaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey=_configuration["StripeSettings:SecretKey"];
            var basket= await _basketRepository.GetBasketAsync(basketId);
            var  shippingPrice=0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod=await _unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice=DeliveryMethod.Price;
                Console.WriteLine("inside if"+basket.DeliveryMethodId);

            }
            foreach(var item in basket.Items)
            {
                var productItem=await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if(item.Price!=productItem.Price)
                {
                    item.Price=productItem.Price;
                }
            }
            var service=new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options=new PaymentIntentCreateOptions
                {
                    Amount=(long)basket.Items.Sum(i=>i.Quantity* (i.Price* 100))+(long)shippingPrice*100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>{"card"}

                };
                intent=await service.CreateAsync(options);
                basket.PaymentIntentId=intent.Id;
                basket.ClientSecret=intent.ClientSecret;
            }
            else{
                var options=new PaymentIntentUpdateOptions
                {
                    Amount=(long)basket.Items.Sum(i=>i.Quantity* (i.Price* 100))+(long)shippingPrice*100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>{"card"}               
                };
                await service.UpdateAsync(basket.PaymentIntentId,options);

            }
            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }
    }
}