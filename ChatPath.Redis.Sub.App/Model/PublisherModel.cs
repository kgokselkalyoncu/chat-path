using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPath.Redis.Sub.App.Model
{
    public class PublisherModel
    {
        public string ChannelName { get; set; }
        public string Message { get; set; }
    }
}
