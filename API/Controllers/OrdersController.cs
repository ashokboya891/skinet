using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entites.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;

        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email=HttpContext.User.RetrieveEmailFromPrinciple();
            var address=_mapper.Map<AddressDto,Address>(orderDto.ShipToAddress);
            var order=await _orderService.CreateOrderAsync(email,orderDto.DeliveryMethodId,orderDto.BasketID,address);
            if(order==null ) return BadRequest(new ApiResponse(400,"Problem creating Order"));
            return Ok(order);
            
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email=HttpContext.User.RetrieveEmailFromPrinciple();
            var orders=await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email=HttpContext.User.RetrieveEmailFromPrinciple();
            var orders=await _orderService.GetOrderByIdAsync(id,email);
            if(orders==null) return NotFound(new ApiResponse(404));
            return _mapper.Map<OrderToReturnDto>(orders);
        }
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetdeliveryMethods(int id)
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

    }
}