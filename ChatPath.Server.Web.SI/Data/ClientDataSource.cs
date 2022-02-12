using ChatPath.Server.Web.SI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Server.Web.SI.Data
{
    public static class ClientDataSource
    {
        public static List<UserClient> UserClients { get; } = new List<UserClient>();
        public static List<Room> Rooms { get; } = new List<Room>();
    }
}
