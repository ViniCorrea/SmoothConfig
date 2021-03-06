﻿using FluentValidation.Results;
using SmoothConfig.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Services
{
    public interface IImporterService
    {
        void ImportConfig(ImportConfigViewModel importConfigViewModel);
    }
}
