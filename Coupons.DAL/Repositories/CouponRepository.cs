using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupons.DAL.Interfaces;

namespace Coupons.DAL.Repositories
{
   public class CouponRepository : BaseRepository, ICouponRepository
    {
        public Coupons Get(long id)
        {
            return dbContext.Coupons.FirstOrDefault(c => c.Id == id);
        }

        public Coupons GetByNameAndCompanyId(string name ,long companyId)
        {
            return dbContext.Coupons.FirstOrDefault(c => c.Name == name && c.CompanyId==companyId);
        }

        public List<Coupons> GetCoupons()
        {
            return dbContext.Coupons.ToList();
        }

        public void Delete(long id)
        {
            var Coupon = Get(id);
            dbContext.Coupons.Remove(Coupon);
            dbContext.SaveChanges();
        }

        public void Create(Coupons coupon)
        {
            dbContext.Coupons.Add(coupon);
            dbContext.SaveChanges();
        }

        public void Update(Coupons user)
        {
            var couponToUpdate = Get(user.Id);
            couponToUpdate = user;
            dbContext.SaveChanges();
        }

        public void UpdateOnCompanyDeletion(long companyId)
        {
            var couponsToUpdate = dbContext.Coupons.Where(c => c.CompanyId == companyId);

            foreach(var coupon in couponsToUpdate)
            {
                coupon.CompanyId = -1;
            }

            if(couponsToUpdate !=null)
            {
                dbContext.SaveChanges();
            }  
        }
    }
}
