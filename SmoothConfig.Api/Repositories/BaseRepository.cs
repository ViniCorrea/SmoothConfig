using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Repositories
{
    public abstract class BaseRepository
    {
        protected DataContext DataContext = null;
        protected Task<IClientSessionHandle> Session = null;

        public BaseRepository(IConfiguration configuration)
        {
            DataContext = new DataContext(configuration.GetSection("MongoConnection:ConnectionString").Value, configuration.GetSection("MongoConnection:Database").Value);
            //Session = DataContext.GetClient().StartSessionAsync();
        }
    }
}
