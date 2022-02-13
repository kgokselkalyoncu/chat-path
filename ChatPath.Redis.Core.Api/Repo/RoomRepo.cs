using ChatPath.Redis.Core.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ChatPath.Redis.Core.Api.Repo
{
    public class RoomRepo : IRoomRepo
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _mongoDB = null;
        private IMongoCollection<Room> _roomCollection = null;

        public RoomRepo()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDB = _mongoClient.GetDatabase("ChatPath");
            _roomCollection = _mongoDB.GetCollection<Room>("Rooms");
        }

        public string Delete(string roomId)
        {
            DeleteResult deleteResult = _roomCollection.DeleteOne(r => r.Id == roomId);
            return "Deleted " + deleteResult.DeletedCount;
        }

        public Room Get(string roomId)
        {
            return _roomCollection.Find(r => r.Id == roomId).FirstOrDefault();
        }

        public List<Room> Gets()
        {
            return _roomCollection.Find(FilterDefinition<Room>.Empty).ToList();
        }

        public Room Save(Room room)
        {
            var roomObj = _roomCollection.Find(r => r.Id == room.Id).FirstOrDefault();

            if(roomObj == null)
            {
                _roomCollection.InsertOne(room);
            }
            else
            {
                _roomCollection.ReplaceOne(r => r.Id == room.Id, room);
            }

            return room;
        }
    }
}
