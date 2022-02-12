using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Server.Web.SI.Models
{
    public class UserClient
    {
        public string ConId { get; set; }
        public string NickName { get; set; }

        public string ActiveRoomName { get; set; }
    }
}
