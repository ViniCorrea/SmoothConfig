using MongoDB.Bson;
using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUsernameAndPassword(string username, string password);
        bool SaveToken(ObjectId userId, string accessToken, string refreshToken, DateTime expiration);
    }
}
