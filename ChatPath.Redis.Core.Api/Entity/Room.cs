using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatPath.Redis.Core.Api.Entity
{
    public class Room
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string RoomName { get; set; }
        public string CreatedNickName { get; set; }
        public DateTime Created { get; set; }
    }
}
