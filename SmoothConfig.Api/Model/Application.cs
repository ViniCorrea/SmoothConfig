using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Application
    {
        public string Name { get; set; }
        public List<Environment> Environments { get; set; }
    }
}
