using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPath.Redis.Sub.App.Model
{
    public class ChatRoomModel
    {
        public string NickName { get; set; }
        public string ConId { get; set; }
        public string RoomName { get; set; }
        public string Message { get; set; }
        public string Created { get; set; }
    }
}
