using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class AccessToken
    {
        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("refresh_token")]
        public string RefreshToken { get; set; }

        [BsonElement("expiration")]
        [DataType(DataType.DateTime)]
        public DateTime Expiration { get; set; }
    }
}
