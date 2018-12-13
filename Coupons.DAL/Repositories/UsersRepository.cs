using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupons.DAL.Interfaces;
using Coupons.DAL;

namespace Coupons.DAL.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public Users Get(long id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public Users GetUserByEmail(string email)
        {
            return dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<Users> GetUsers()
        {
            return dbContext.Users.ToList();
        }

        public Users GetUserByToken(string token)
        {
            return dbContext.Users.FirstOrDefault(u => u.Token == token);
        }

        public void AddNewAuthUser(long createUserId, string encryptedPassword)
        {
            dbContext.AuthUsers.Add(new AuthUsers
            {
                UserId = createUserId,
                EncryptedPassword = encryptedPassword,
                LatsUpdatedTimeUtc = DateTime.UtcNow
            });
            dbContext.SaveChanges();
        }

        public void Delete(long id)
        {
            var user = Get(id);
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }

        public void Create(Users user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void Update(Users user)
        {
            var userToUpdate = Get(user.Id);
            userToUpdate = user;
            dbContext.SaveChanges();
        }
    }
}
