using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Context.IContext;
using Mivos_Bot.Models;

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

        public List<User> GetMuted()
        {
            return context.GetMuted();
        }

        public User GetUser(ulong uid)
        {
            return context.GetUser(uid);
        }

        public bool MuteUser(DiscordUser user)
        {
            return context.MuteUser(user);
        }

        public bool Unmute(ulong uid)
        {
            return context.Unmute(uid);
        }

        public bool User_exists(DiscordUser user)
        {
            return context.User_exists(user);
        }
    }
}
