using MongoDB.Bson;
using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Repositories
{
    public interface IConfigRepository
    {
        Config GetConfigByNameApplicationDistributionAndEnvironment(string name, string application, string distribution, string environment);
        Config GetConfigByApplicationDistributionAndEnvironment(string application, string distribution, string environment);
        bool SaveConfig(ObjectId configId, string application, string distribution, string environment, List<Setting> settings);
        bool SaveConfig(ObjectId configId, List<Setting> settings);
        void NewConfig(Config config);
    }
}
