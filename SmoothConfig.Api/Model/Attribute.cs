using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Attribute
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
