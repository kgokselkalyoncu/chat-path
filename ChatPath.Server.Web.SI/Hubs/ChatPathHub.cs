using ChatPath.Server.Web.SI.Data;
using ChatPath.Server.Web.SI.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Server.Web.SI.Hubs
{
    public class ChatPathHub : Hub
    {
        public async Task GetNickName(string nickName, string conId)
        {
            string redisSubConnectionId = Context.ConnectionId;
            UserClient userClient = new UserClient
            {
                ConId = conId,
                NickName = nickName
            };

            ClientDataSource.UserClients.Add(userClient);

            await Clients.Others.SendAsync("clientJoined", nickName);
        }

        public async Task AddChatRoom(string roomName)
        {
            Room room = new Room{ RoomName = roomName };
            ClientDataSource.Rooms.Add(room);

            await Clients.All.SendAsync("newRoomAdded", ClientDataSource.Rooms);
        }

        public async Task JoinChatRoom(string roomName, string conId)
        {
            string redisSubConnectionId = Context.ConnectionId;
            UserClient userClient = ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == conId);
            string activeRoomName = userClient.ActiveRoomName;

            if (activeRoomName != null && activeRoomName != roomName)
            {
                ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == activeRoomName).userClients.Remove(userClient);
                await Groups.RemoveFromGroupAsync(userClient.ConId, activeRoomName);
                Room roomPasive = ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == activeRoomName);
                await Clients.Group(activeRoomName).SendAsync("roomUsersList", roomPasive.userClients);
            }

            Room room = ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == roomName);

            bool isUserCheck = room.userClients.Any(c => c.ConId == conId);
            if (!isUserCheck)
            {
                room.userClients.Add(userClient);
                ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == conId).ActiveRoomName = roomName;

                await Groups.AddToGroupAsync(conId, roomName);
            }

            await Clients.Group(roomName).SendAsync("roomUsersList", room.userClients);
        }

        public async Task ChatRoomList(string message)
        {
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(message);
            ClientDataSource.Rooms.Clear();
            ClientDataSource.Rooms.AddRange(rooms);

            await Clients.All.SendAsync("newRoomAdded", ClientDataSource.Rooms);
        }

        public async Task GetUserToRoom(string roomName, string conId)
        {
            Room room = ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == roomName);

            await Clients.Client(conId).SendAsync("roomUsersList", room.userClients);
        }

        public async Task SendMessageAllClient(string message)
        {
            //await Clients.Group(roomName).SendAsync("reciveMessage", message);
            await Clients.All.SendAsync("reciveMessage", message, "RedisTest");
        }

        public async Task SendRoomMessage(string message, string roomName, string created, string conId)
        {
            string redisSubConnectionId = Context.ConnectionId;
            string nickName = ClientDataSource.UserClients.Where(c => c.ConId == conId).FirstOrDefault().NickName;

            await Clients.GroupExcept(roomName, conId).SendAsync("reciveMessage", message, nickName, created);
        }

        public async Task UserDisconnect(string conId)
        {
            UserClient userClient = ClientDataSource.UserClients.Where(c => c.ConId == conId).FirstOrDefault();
            string activeRoomName = ClientDataSource.UserClients.Where(c => c.ConId == conId).FirstOrDefault().ActiveRoomName;

            if (ClientDataSource.Rooms.Count > 0)
            {
                Room room = ClientDataSource.Rooms.Where(r => r.RoomName == activeRoomName).FirstOrDefault();
                ClientDataSource.Rooms.Remove(room);
                if(room != null)
                {
                    room.userClients.Remove(userClient);
                    ClientDataSource.Rooms.Add(room);
                }
            }

            ClientDataSource.UserClients.Remove(userClient);

            await Groups.RemoveFromGroupAsync(conId, activeRoomName);
        }
    }
}
