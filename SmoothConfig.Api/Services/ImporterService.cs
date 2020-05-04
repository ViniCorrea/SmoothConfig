using SmoothConfig.Api.Importer;
using SmoothConfig.Api.Model;
using SmoothConfig.Api.Repositories;
using SmoothConfig.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Services
{
    public class ImporterService : IImporterService
    {
        //private readonly IMapper _mapper;
        private readonly IConfigRepository _configRepository;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public ImporterService(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public void ImportConfig(ImportConfigViewModel importConfigViewModel)
        {
            var importer = new ConfigImporter(importConfigViewModel.File.OpenReadStream());
            var settings = importer.GetSettings();

            var config = _configRepository.GetConfigByApplicationDistributionAndEnvironment(
                    importConfigViewModel.Application,
                    importConfigViewModel.Distribution,
                    importConfigViewModel.Environment
                );

            if(config is null)
            {
                var newConfig = new Config()
                {
                    Name = importConfigViewModel.Name,
                    Application = importConfigViewModel.Application,
                    Distribution = importConfigViewModel.Distribution,
                    Environment = importConfigViewModel.Environment,
                    Settings = settings
                };

                _configRepository.NewConfig(newConfig);
            }
            else
            {
                _configRepository.SaveConfig(config.Id, settings);
            }
        }
            
    }
}

