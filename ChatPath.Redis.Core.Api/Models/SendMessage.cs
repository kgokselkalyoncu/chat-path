using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Models
{
    public class SendMessage
    {
        public string NickName { get; set; }
        public string ConId { get; set; }
        public string RoomName { get; set; }
        public string Message { get; set; }
        public string Created { get; set; }

    }
}
