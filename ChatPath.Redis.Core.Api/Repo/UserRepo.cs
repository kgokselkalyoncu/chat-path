using ChatPath.Redis.Core.Api.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Repo
{
    public class UserRepo : IUserRepo
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _mongoDB = null;
        private IMongoCollection<User> _userCollection = null;

        public UserRepo()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDB = _mongoClient.GetDatabase("ChatPath");
            _userCollection = _mongoDB.GetCollection<User>("Users");
        }

        public string Delete(string userId)
        {
            DeleteResult deleteResult = _userCollection.DeleteOne(r => r.Id == userId);
            return "Deleted " + deleteResult.DeletedCount;
        }

        public User Get(string userId)
        {
            return _userCollection.Find(r => r.Id == userId).FirstOrDefault();
        }

        public List<User> Gets()
        {
            return _userCollection.Find(FilterDefinition<User>.Empty).ToList();
        }

        public User Save(User user)
        {
            var userObj = _userCollection.Find(u => u.Id == user.Id).FirstOrDefault();

            if (userObj == null)
            {
                _userCollection.InsertOne(user);
            }
            else
            {
                _userCollection.ReplaceOne(u => u.Id == user.Id, user);
            }

            return user;
        }
    }
}
