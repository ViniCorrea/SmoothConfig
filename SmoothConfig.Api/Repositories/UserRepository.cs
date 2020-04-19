using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SmoothConfig.Api.Model;
using System;

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

        public bool SaveToken(ObjectId userId, string accessToken, string refreshToken, DateTime expiration)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Id, userId);
            var update = Builders<User>.Update
                .Set(user => user.AccessToken.Token, accessToken)
                .Set(user => user.AccessToken.RefreshToken, refreshToken)
                .Set(user => user.AccessToken.Expiration, expiration);

            var result = DataContext.User.UpdateOne(filter, update);
            return result.ModifiedCount == 1;
        }
    }
}
