using ChatPath.Redis.Core.Api.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;
using ChatPath.Redis.Core.Api.Repo;
using ChatPath.Redis.Core.Api.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatPath.Redis.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IRoomRepo _roomRepo = null;
        private IUserRepo _userRepo = null;
        private IMessageRepo _messageRepo = null;
        public ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse("127.0.0.1:6379"));

        public ChatController(IRoomRepo roomRepo, IUserRepo userRepo, IMessageRepo messageRepo)
        {
            _roomRepo = roomRepo;
            _userRepo = userRepo;
            _messageRepo = messageRepo;
        }

        [HttpPost]
        public void Post(SendMessage sendMessage)
        {
            string channelName = "ChatRoom";
            string sendMessageJson = JsonConvert.SerializeObject(sendMessage);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = sendMessageJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            Message message = new Message();
            message.NickName = sendMessage.NickName;
            message.ConId = sendMessage.ConId;
            message.Created = DateTime.Now;
            message.RoomName = sendMessage.RoomName;
            message.SendMessage = sendMessage.Message;

            message = _messageRepo.Save(message);

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }

        [HttpPost]
        [Route("[action]")]
        public void GetNickName(SendMessage sendMessage)
        {
            //Redis Connection
            string channelName = "GetNickName";
            string sendMessageJson = JsonConvert.SerializeObject(sendMessage);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = sendMessageJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            User user = new User();
            user.NickName = sendMessage.NickName;
            user.ConId = sendMessage.ConId;
            user.Created = DateTime.Now;

            user = _userRepo.Save(user);

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }

        [HttpPost]
        [Route("[action]")]
        public void ChatRoomList()
        {
            List<Room> rooms = new List<Room>();
            rooms = _roomRepo.Gets();

            string channelName = "ChatRoomList";
            string sendMessageJson = JsonConvert.SerializeObject(rooms);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = sendMessageJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }

    }
}
