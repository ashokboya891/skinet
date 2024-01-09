using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Entites;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController:BaseApiController
    {
        private const string WhSecret="whsec_788f2e2a57b5979bdf8c7ac2b382a61e20436b35e165534afe341fd7ba9dd944";
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger1;
        public PaymentsController(IPaymentService paymentService ,ILogger<PaymentsController> logger1)
        {
            _logger1 = logger1;
            _paymentService = paymentService;
            
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreatedOrUpdatedPaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400, "Problme with your basket"));
            return basket;
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json=await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeEvent=EventUtility.ConstructEvent(json,
            Request.Headers["Stripe-Signature"],WhSecret);

            PaymentIntent intent;
            Order order;
            switch(stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                intent=(PaymentIntent) stripeEvent.Data.Object;
                _logger1.LogInformation("Payment succeeded",intent.Id);
                order=await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                //to do update the order with new status
                _logger1.LogInformation("Order Updated to payment received",order.Id);
                break;
                case "payment_intent.payment_failed":
                intent=(PaymentIntent) stripeEvent.Data.Object;
                _logger1.LogInformation("Payment failed",intent.Id);
                order=await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                //to do update the order with new status
                _logger1.LogInformation("Order Updated to payment failed",order.Id);
                break;
            

            }
            return new EmptyResult();

        }
    }
}