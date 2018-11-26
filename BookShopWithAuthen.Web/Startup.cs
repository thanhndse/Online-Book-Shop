using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookShopWithAuthen.Web.Startup))]
namespace BookShopWithAuthen.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
