using Microsoft.AspNetCore.Http;
using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.ViewModel
{
    public class ImportConfigViewModel
    {
        public string Name { get; set; }
        public string Application { get; set; }
        public string Environment { get; set; }
        public string Distribution { get; set; }
        public IFormFile File { get; set; }
    }
}
