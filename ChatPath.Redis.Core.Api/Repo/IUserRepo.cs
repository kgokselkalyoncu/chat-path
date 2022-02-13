using ChatPath.Redis.Core.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Repo
{
    public interface IUserRepo
    {
        User Save(User user);
        User Get(string userId);
        List<User> Gets();
        string Delete(string userId);
    }
}
