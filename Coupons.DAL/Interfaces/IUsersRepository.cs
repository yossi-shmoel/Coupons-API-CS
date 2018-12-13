using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupons.DAL.Interfaces
{
    public interface IUsersRepository : IRepository
    {
        Users Get(long id);
        Users GetUserByEmail(string email);
        List<Users> GetUsers();
        Users GetUserByToken(string token);
        void AddNewAuthUser(long createUserId, string encryptedPassword);
        void Create(Users users);
        void Update(Users users);
        void Delete(long id);
    }
}
