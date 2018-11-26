using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShopWithAuthen.Web.ViewModel
{
    public class ShippingDetailViewModel
    {
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wrong phone number format")]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Wrong email format")]
        public string Email { get; set; }
    }
}