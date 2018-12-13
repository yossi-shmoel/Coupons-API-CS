using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupons.DAL.Interfaces;

namespace Coupons.DAL.Repositories
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public Companies Get(long id)
        {
            return dbContext.Companies.FirstOrDefault(c => c.Id == id);
        }

        public List<Companies> GetCompanies()
        {
            return dbContext.Companies.ToList();
        }

        public Companies GetByEmail(string email)
        {
            return dbContext.Companies.FirstOrDefault(c => c.Email == email);
        }

        public void Create(Companies company)
        {
            dbContext.Companies.Add(company);
            dbContext.SaveChanges();
        }

        public void Update(Companies company)
        {
            var dbResponse = Get(company.Id);
            dbResponse = company;
            dbContext.SaveChanges();
        }

        public void Delete(long id)
        {
            var dbResponse = Get(id);
            dbContext.Companies.Remove(dbResponse);
            dbContext.SaveChanges();
        }

        public bool IsCompanyExsists(long id)
        {
            var dbResponse = dbContext.Companies.FirstOrDefault(c => c.Id == id);
            if(dbResponse ==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void tester()
        {
           
        }
    }
}
