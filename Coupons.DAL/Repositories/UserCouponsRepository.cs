using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupons.DAL.Interfaces;

namespace Coupons.DAL.Repositories
{
    public class UserCouponsRepository : BaseRepository, IUserCouponsRepository
    {
        

        public void RemoveUserCoupons(long userId, long couponId)
        {
            var userCoupon = dbContext.UserCoupons.FirstOrDefault(c => c.UserId == userId && c.CouponId == couponId);
            dbContext.SaveChanges();
        }

        public void RemoveAllByUser(long userId)
        {
            var userCoupon = dbContext.UserCoupons.FirstOrDefault(c => c.UserId == userId);
            dbContext.SaveChanges();
        }

        public void RemoveAllByCoupon(long couponId)
        {
            var userCoupon = dbContext.UserCoupons.FirstOrDefault(c => c.CouponId == couponId);
            dbContext.SaveChanges();
        }

        public bool purchaseCoupon(long userId, long couponId)
        {
            if (GetRemainingCoupons(couponId) > 0)
            {
                AddUserCoupons(userId, couponId);
                return true;
            }
            return false;
        }

        #region Private
        private void AddUserCoupons(long userId, long couponId)
        {
            var newUserCoupon = new UserCoupons
            {
                CouponId = couponId,
                UserId = userId
            };

            dbContext.UserCoupons.Add(newUserCoupon);
            dbContext.SaveChanges();
        }

        private int GetRemainingCoupons(long couponId)
        {
            var couponAmount = dbContext.Coupons.Count(c => c.Id == couponId);
            var usedCoupons = dbContext.UserCoupons.Count(c => c.CouponId == couponId);

            return couponAmount - usedCoupons;
        }
        #endregion Private
    }
}