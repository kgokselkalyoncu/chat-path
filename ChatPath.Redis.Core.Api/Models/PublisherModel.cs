using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Models
{
    public class PublisherModel
    {
        public string ChannelName { get; set; }
        public string Message { get; set; }
    }
}
