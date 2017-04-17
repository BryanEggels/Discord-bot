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

        //made an interface 'context' so i can switch to any other context with those specified methods.
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

        public bool MuteCountreset()
        {
            return context.MuteCountreset();
        }

        public bool MuteReset()
        {
            return context.MuteReset();
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
