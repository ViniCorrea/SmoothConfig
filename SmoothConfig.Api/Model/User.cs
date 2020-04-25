using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Model
{
    public class User
    {
        public User()
        {
            AccessToken = new AccessToken();
            Roles = new List<string>();
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BsonElement("access_token")]
        public AccessToken AccessToken { get; set; }

        [BsonElement("roles")]
        public List<string> Roles { get; set; }
    }
}
