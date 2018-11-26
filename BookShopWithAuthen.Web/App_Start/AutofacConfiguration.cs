using Autofac;
using Autofac.Integration.Mvc;
using BookShopWithAuthen.Data;
using BookShopWithAuthen.Data.Infrastructure;
using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace BookShopWithAuthen.Web.App_Start
{
    public static class AutofacConfiguration
    {
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper

        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterGeneric(typeof(BaseRepository<>))
   .As(typeof(IBaseRepository<>)).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(BookService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerRequest();

            builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                .As<UserManager<ApplicationUser>>().InstancePerRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}