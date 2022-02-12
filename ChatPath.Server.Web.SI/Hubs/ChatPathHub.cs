using ChatPath.Server.Web.SI.Data;
using ChatPath.Server.Web.SI.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Server.Web.SI.Hubs
{
    public class ChatPathHub : Hub
    {
        public async Task GetNickName(string nickName)
        {
            UserClient userClient = new UserClient
            {
                ConId = Context.ConnectionId,
                NickName = nickName
            };

            ClientDataSource.UserClients.Add(userClient);

            await Clients.Others.SendAsync("clientJoined", nickName);

        }

        public async Task AddChatRoom(string roomName)
        {
            Room room = new Room
            {
                RoomName = roomName
            };

            room.userClients.Add(ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == Context.ConnectionId));

            ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == Context.ConnectionId).ActiveRoomName = roomName;

            ClientDataSource.Rooms.Add(room);

            await Clients.All.SendAsync("newRoomAdded", ClientDataSource.Rooms);
        }

        public async Task JoinChatRoom(string roomName)
        {
            UserClient userClient = ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == Context.ConnectionId);
            string activeRoomName = userClient.ActiveRoomName;

            if (activeRoomName != null && activeRoomName != roomName)
            {
                ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == activeRoomName).userClients.Remove(userClient);
            }

            Room room = ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == roomName);

            bool isUserCheck = room.userClients.Any(c => c.ConId == Context.ConnectionId);
            if (!isUserCheck)
            {
                room.userClients.Add(userClient);
                ClientDataSource.UserClients.FirstOrDefault(c => c.ConId == Context.ConnectionId).ActiveRoomName = roomName;

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }

            await Clients.Others.SendAsync("roomUsersList", room.userClients);
        }

        public async Task ChatRoomList()
        {
            await Clients.All.SendAsync("newRoomAdded", ClientDataSource.Rooms);
        }

        public async Task GetUserToRoom(string roomName)
        {
            Room room = ClientDataSource.Rooms.FirstOrDefault(g => g.RoomName == roomName);

            await Clients.Caller.SendAsync("roomUsersList", room.userClients);
        }

        //public async Task SendMessage(string message, string roomName)
        //{
        //    await Clients.Group(roomName).SendAsync("reciveMessage", message);
        //}

        public async Task SendMessage(string message)
        {
            string nickName = ClientDataSource.UserClients.Where(c => c.ConId == Context.ConnectionId).FirstOrDefault().NickName;
            await Clients.Others.SendAsync("reciveMessage", message, nickName);
        }

        public async Task SendRoomMessage(string message, string roomName)
        {
            string nickName = ClientDataSource.UserClients.Where(c => c.ConId == Context.ConnectionId).FirstOrDefault().NickName;
            await Clients.OthersInGroup(roomName).SendAsync("reciveMessage", message, nickName);
        }

    }
}
