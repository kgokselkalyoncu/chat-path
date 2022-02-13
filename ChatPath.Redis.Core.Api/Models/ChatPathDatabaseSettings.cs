using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Models
{
    public class ChatPathDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string MessagesCollectionName { get; set; } = null!;
        public string RoomsCollectionName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
    }
}
