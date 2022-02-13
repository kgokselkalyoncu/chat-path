using ChatPath.Redis.Core.Api.Entity;
using ChatPath.Redis.Core.Api.Models;
using ChatPath.Redis.Core.Api.Repo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatPath.Redis.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomRepo _roomRepo = null;
        public ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse("127.0.0.1:6379"));

        public RoomController(IRoomRepo roomRepo)
        {
            _roomRepo = roomRepo;
        }

        [HttpPost]
        public void Post(RoomViewModel roomViewModel)
        {
            string channelName = "AddRoom";
            string roomViewModelJson = JsonConvert.SerializeObject(roomViewModel);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = roomViewModelJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            Room room = new Room();
            room.RoomName = roomViewModel.RoomName;
            room.CreatedNickName = roomViewModel.NickName;
            room.Created = DateTime.Now;

            room = _roomRepo.Save(room);

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }

        [HttpPost]
        [Route("[action]")]
        public void JoinChatRoom(RoomViewModel roomViewModel)
        {
            string channelName = "JoinChatRoom";
            string roomViewModelJson = JsonConvert.SerializeObject(roomViewModel);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = roomViewModelJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }

        [HttpPost]
        [Route("[action]")]
        public void GetUserToRoom(RoomViewModel roomViewModel)
        {
            string channelName = "GetUserToRoom";
            string roomViewModelJson = JsonConvert.SerializeObject(roomViewModel);

            PublisherModel publisherModel = new PublisherModel();
            publisherModel.ChannelName = channelName;
            publisherModel.Message = roomViewModelJson;

            string publisherModelJson = JsonConvert.SerializeObject(publisherModel);

            var database = connection.GetDatabase();
            var publisher = connection.GetSubscriber();

            long pubReturn = publisher.Publish(channelName, $"{publisherModelJson}");
        }
    }
}
