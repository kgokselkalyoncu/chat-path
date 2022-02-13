using ChatPath.Redis.Sub.App.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace ChatPath.Redis.Sub.App
{
    class Program
    {
        public static string url = "https://localhost:5001/chat-path";
        public static HubConnection connectionSignalr = new HubConnectionBuilder().WithUrl(url).Build();
        static void Main(string[] args)
        {
            //Redis Connection
            var configuration = ConfigurationOptions.Parse("127.0.0.1:6379");
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(configuration);

            string channelPattern = "*";

            // get a database client
            var database = connection.GetDatabase();

            // get a subscriber client, which is a different client from database.
            var subPatternChannel = new RedisChannel(channelPattern, RedisChannel.PatternMode.Pattern);
            var subscriber = connection.GetSubscriber().Subscribe(subPatternChannel);

            Console.WriteLine("Starting message consumer");
            Console.WriteLine("Checking message queue");

            while (database.ListLength("scb.messages") > 0)
            {
                string messages = database.ListRightPop("scb.messages");

                PublisherModel publisherModel = JsonConvert.DeserializeObject<PublisherModel>(messages);
                string message = publisherModel.Message;

                var channelName = database.ListRightPop("scb.channel");
                ProcessMessage(channelName,message);
            }

            Console.WriteLine("Subscribing to pubsub channel");
           
            subscriber.OnMessage(msg =>{ Handler(msg); });

            Console.ReadKey();
        }
        private static void Handler(ChannelMessage channelMessage)
        {
            string channelName = channelMessage.Channel.ToString();
            string messages = channelMessage.Message;

            PublisherModel publisherModel = JsonConvert.DeserializeObject<PublisherModel>(messages);
            string message = publisherModel.Message;

            ProcessMessage(channelName, message);
        }
        private static void ProcessMessage(string channelName, string message)
        {
            Console.WriteLine(channelName + ":" + message);

            switch (channelName)
            {
                case "AddRoom":
                    AddChatRoom(message);
                    break;
                case "JoinChatRoom":
                    JoinChatRoom(message);
                    break;
                case "GetUserToRoom":
                    GetUserToRoom(message);
                    break;
                case "GetNickName":
                    GetNickName(message);
                    break;
                case "ChatRoomList":
                    ChatRoomList(message);
                    break;
                case "ChatRoom":
                    SendRoomMessage(message);
                    break;
            }
        }

        private static void SendRoomMessage(string messages)
        {
            
            ChatRoomModel chatRoomModel = JsonConvert.DeserializeObject<ChatRoomModel>(messages);

            string roomName = chatRoomModel.RoomName;
            string nickName = chatRoomModel.NickName;
            string conId = chatRoomModel.ConId;
            string message = chatRoomModel.Message;
            string created = chatRoomModel.Created;

            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("SendRoomMessage", message, roomName, created, conId);
        }
        private static void AddChatRoom(string message)
        {
            RoomModel roomModel = JsonConvert.DeserializeObject<RoomModel>(message);

            string roomName = roomModel.RoomName;
            string nickName = roomModel.NickName;
            string conId = roomModel.ConId;

            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("AddChatRoom", roomName);
        }
        private static void JoinChatRoom(string message)
        {
            RoomModel roomModel = JsonConvert.DeserializeObject<RoomModel>(message);

            string roomName = roomModel.RoomName;
            string nickName = roomModel.NickName;
            string conId = roomModel.ConId;

            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("JoinChatRoom", roomName, conId);
        }
        private static void GetUserToRoom(string message)
        {
            RoomModel roomModel = JsonConvert.DeserializeObject<RoomModel>(message);

            string roomName = roomModel.RoomName;
            string nickName = roomModel.NickName;
            string conId = roomModel.ConId;

            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("GetUserToRoom", roomName, conId);
        }
        private static void GetNickName(string messages)
        {

            ChatRoomModel chatRoomModel = JsonConvert.DeserializeObject<ChatRoomModel>(messages);

            string roomName = chatRoomModel.RoomName;
            string nickName = chatRoomModel.NickName;
            string conId = chatRoomModel.ConId;
            string message = chatRoomModel.Message;
            string created = chatRoomModel.Created;

            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("GetNickName", nickName, conId);
        }
        private static void ChatRoomList(string message)
        {
            connectionSignalr.StartAsync();
            connectionSignalr.InvokeAsync("ChatRoomList", message);
        }
    }
}
