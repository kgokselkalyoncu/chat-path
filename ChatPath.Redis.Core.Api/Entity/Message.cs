using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Entity
{
    public class Message
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string NickName { get; set; }
        public string ConId { get; set; }
        public string RoomName { get; set; }
        public string SendMessage { get; set; }
        public DateTime Created { get; set; }
    }
}
