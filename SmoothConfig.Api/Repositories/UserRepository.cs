using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SmoothConfig.Api.Model;

namespace SmoothConfig.Api.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            var result = DataContext.User.Find(user => user.UserName == username && user.Password == password).SingleOrDefault<User>();
            return result;
        }
    }
}
