using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class Config
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("application")]
        public string Application { get; set; }

        [BsonElement("environment")]
        public string Environment { get; set; }

        [BsonElement("distribution")]
        public string Distribution { get; set; }

        [BsonElement("settings")]
        public List<Setting> Settings { get; set; }
    }
}
