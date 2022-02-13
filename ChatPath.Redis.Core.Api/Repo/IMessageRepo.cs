using ChatPath.Redis.Core.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatPath.Redis.Core.Api.Repo
{
    public interface IMessageRepo
    {
        Message Save(Message message);
        Message Get(string messageId);
        List<Message> Gets();
        string Delete(string messageId);
    }
}
