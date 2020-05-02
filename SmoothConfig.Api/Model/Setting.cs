using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Setting
    {
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("attributes")]
        public List<Attribute> Attributes { get; set; }

        [BsonElement("childrens")]
        public List<Setting> Childrens { get; set; }
    }
}
