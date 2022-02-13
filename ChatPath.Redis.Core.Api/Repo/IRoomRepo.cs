using ChatPath.Redis.Core.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Repo
{
    public interface IRoomRepo
    {
        Room Save(Room room);
        Room Get(string roomId);
        List<Room> Gets();
        string Delete(string roomId);
    }
}
