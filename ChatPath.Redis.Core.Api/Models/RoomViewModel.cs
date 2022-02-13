using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Models
{
    public class RoomViewModel
    {
        public string RoomName { get; set; }
        public string ConId { get; set; }
        public string NickName { get; set; }
    }
}
