using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Environment
    {
        public string Name { get; set; }
        public Config Config { get; set; }
    }
}
