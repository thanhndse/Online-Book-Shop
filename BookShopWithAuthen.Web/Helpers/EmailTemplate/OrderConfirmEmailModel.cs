using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Web.ViewModel;
using System.Collections.Generic;

namespace BookShopWithAuthen.Web.Helpers.EmailTemplate
{
    public class OrderConfirmEmailModel
    {
        public Order Order { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetailViewModels { get; set; }
    }
}