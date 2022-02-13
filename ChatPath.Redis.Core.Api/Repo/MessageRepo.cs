using ChatPath.Redis.Core.Api.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Repo
{
    public class MessageRepo : IMessageRepo
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _mongoDB = null;
        private IMongoCollection<Message> _messageCollection = null;

        public MessageRepo()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDB = _mongoClient.GetDatabase("ChatPath");
            _messageCollection = _mongoDB.GetCollection<Message>("Messages");
        }

        public string Delete(string messageId)
        {
            DeleteResult deleteResult = _messageCollection.DeleteOne(m => m.Id == messageId);
            return "Deleted " + deleteResult.DeletedCount;
        }

        public Message Get(string messageId)
        {
            return _messageCollection.Find(m => m.Id == messageId).FirstOrDefault();
        }

        public List<Message> Gets()
        {
            return _messageCollection.Find(FilterDefinition<Message>.Empty).ToList();
        }

        public Message Save(Message message)
        {
            var messageObj = _messageCollection.Find(m => m.Id == message.Id).FirstOrDefault();

            if (messageObj == null)
            {
                _messageCollection.InsertOne(message);
            }
            else
            {
                _messageCollection.ReplaceOne(m => m.Id == message.Id, message);
            }

            return message;
        }
    }
}
