using BookShopWithAuthen.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookShopWithAuthen.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public enum StatusUser : int
    {
        Active,
        Blocked
    }

}
