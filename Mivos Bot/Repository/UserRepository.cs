using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Context.IContext;

namespace Mivos_Bot.Repository
{
    class UserRepository : IUserContext
    {
        IUserContext context;
        public UserRepository(IUserContext context)
        {
            this.context = context;
        }
        public bool AddUser(DiscordUser user)
        {
            return context.AddUser(user);
        }

        public List<ulong> GetMuted()
        {
            return context.GetMuted();
        }

        public bool MuteUser(DiscordUser user)
        {
            return context.MuteUser(user);
        }
        public bool SelectUser(DiscordUser user)
        {
            return context.SelectUser(user);
        }
    }
}
