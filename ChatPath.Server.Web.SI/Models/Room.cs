using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Server.Web.SI.Models
{
    public class Room
    {
        public string RoomName { get; set; }

        public List<UserClient> userClients { get; } = new List<UserClient>();
    }
}
