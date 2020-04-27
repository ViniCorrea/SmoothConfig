using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Config
    {
        public string Name { get; set; }
        public List<Setting> Settings { get; set; }
    }
}
