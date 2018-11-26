using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopWithAuthen.Data.Infrastructure
{
    public interface IDatabaseFactory
    {
        ApplicationDbContext Get();
    } 

    public class DatabaseFactory:IDatabaseFactory
    {
        private ApplicationDbContext dataContext;
        public ApplicationDbContext Get()
        {
            return dataContext ?? (dataContext = new ApplicationDbContext());
        }
    }
}
