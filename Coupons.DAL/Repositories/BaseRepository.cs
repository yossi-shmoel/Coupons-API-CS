using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupons.DAL.Repositories
{
    public class BaseRepository
    {
        protected readonly CouponsEntities dbContext;

        public BaseRepository()
        {
            dbContext = new CouponsEntities();
        }
        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
