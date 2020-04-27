using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Setting
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Setting> Childrens { get; set; }
    }
}
