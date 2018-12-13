using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupons.DAL.Interfaces
{
    public interface ICouponRepository : IRepository
    {
        Coupons Get(long id);
        Coupons GetByNameAndCompanyId(string name, long companyId);
        List<Coupons> GetCoupons();
        void Create(Coupons coupons);
        void Update(Coupons coupons);
        void Delete(long id);

        void UpdateOnCompanyDeletion(long companyId);
    }
}
