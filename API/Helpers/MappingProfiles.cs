

using API.DTOs;
using AutoMapper;
using Core.Entites;
using Core.Entites.Identity;
using Core.Entites.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
            .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
            .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
            .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductUrlResolver>());  //this line resposnible for attching http url for images
            CreateMap<Core.Entites.Identity.Address,AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>();
            CreateMap<AddressDto,Core.Entites.OrderAggregate.Address>();
            CreateMap<Order,OrderToReturnDto>()
            .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))  //below condition after mapping it will again asign values into orderretunrdto to deliveryemthodshortname,and etc below
            .ForMember(d=>d.ShippingPrice,o=>o.MapFrom(s=>s.DeliveryMethod.Price));
            CreateMap<OrderItem,OrderItemDto>()
            .ForMember(d=>d.ProductId,o=>o.MapFrom(s=>s.ItemOrdered.ProductItemId))   //below condition after mapping it will again asign values into orderitemdto to ItemOrdered.properties mentioned on left,and etc below
            .ForMember(d=>d.ProductName,o=>o.MapFrom(s=>s.ItemOrdered.ProductName))
            .ForMember(d=>d.PictureUrl,o=>o.MapFrom(s=>s.ItemOrdered.PictureUrl))
            .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemUrlResolver>());
            


        }
        
    }
}