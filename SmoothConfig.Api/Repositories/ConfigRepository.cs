using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;

namespace SmoothConfig.Api.Repositories
{
    public class ConfigRepository : BaseRepository, IConfigRepository
    {
        public ConfigRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public Config GetConfigByApplicationDistributionAndEnvironment(string application, string distribution, string environment)
        {
            var result = DataContext.Config.Find(
                config => config.Application == application
                && config.Distribution == distribution
                && config.Environment == environment).
                SingleOrDefault<Config>();

            return result;
        }

        public Config GetConfigByNameApplicationDistributionAndEnvironment(string name, string application, string distribution, string environment)
        {
            var result = DataContext.Config.Find(
                config => config.Name == name
                && config.Application == application
                && config.Distribution == distribution
                && config.Environment == environment).
                SingleOrDefault<Config>();

            return result;
        }

        public void NewConfig(Config config)
        {
            DataContext.Config.InsertOne(config);
        }

        public bool SaveConfig(ObjectId configId, string application, string distribution, string environment, List<Setting> settings)
        {
            var filter = Builders<Config>.Filter.Eq(config => config.Id, configId);
            var update = Builders<Config>.Update
                .Set(config => config.Application, application)
                .Set(config => config.Distribution, distribution)
                .Set(config => config.Environment, environment)
                .Set(config => config.Settings, settings);

            var result = DataContext.Config.UpdateOne(filter, update);
            return result.ModifiedCount == 1;
        }

        public bool SaveConfig(ObjectId configId, List<Setting> settings)
        {
            var filter = Builders<Config>.Filter.Eq(config => config.Id, configId);
            var update = Builders<Config>.Update
                .Set(config => config.Settings, settings);

            var result = DataContext.Config.UpdateOne(filter, update);
            return result.ModifiedCount == 1;
        }
    }
}
