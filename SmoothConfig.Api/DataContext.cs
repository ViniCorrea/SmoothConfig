using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api
{
    public class DataContext
    {
        private readonly IMongoClient _client;
        public readonly IMongoDatabase Db = null;

        public DataContext(string connectionString, string dbname)
        {
            _client = new MongoClient(connectionString);
            Db = _client.GetDatabase(dbname);
        }

        public IMongoClient GetClient() => _client;

        public IMongoCollection<User> User
        {
            get
            {
                return Db.GetCollection<User>("User");
            }
        }
    }
}
