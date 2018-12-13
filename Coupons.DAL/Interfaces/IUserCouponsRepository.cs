

namespace Coupons.DAL.Interfaces
{
    public interface IUserCouponsRepository : IRepository
    {
        //void AddUserCoupons(long userId, long couponId);
        void RemoveUserCoupons(long userId, long couponId);
        void RemoveAllByUser(long userId);
        void RemoveAllByCoupon(long couponId);
        bool purchaseCoupon(long userId, long couponId);
    }
}