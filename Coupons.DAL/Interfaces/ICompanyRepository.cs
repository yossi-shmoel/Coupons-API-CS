using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupons.DAL.Interfaces
{
   public interface ICompanyRepository : IRepository
    {
        Companies Get(long id);
        List<Companies> GetCompanies();
        Companies GetByEmail(string email);
        void Create(Companies company);
        void Update(Companies company);
        void Delete(long id);
        bool IsCompanyExsists(long id);
        void tester();
    }
}
