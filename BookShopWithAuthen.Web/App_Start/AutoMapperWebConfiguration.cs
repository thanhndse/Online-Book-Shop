using AutoMapper;
using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Web.ViewModel;

namespace BookShopWithAuthen.Web.App_Start
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles("BookShopWithAuthen.Web");
            });
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, ShippingDetailViewModel>();
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<ShippingDetailViewModel, ApplicationUser>();
            CreateMap<UserViewModel, ApplicationUser>();
        }
    }
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailViewModel>();
            CreateMap<OrderDetailViewModel, OrderDetail>();
        }
    }
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingDetailViewModel, Order>();
            CreateMap<Order, ShippingDetailViewModel>();
        }
    }
}