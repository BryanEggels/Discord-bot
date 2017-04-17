using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Models;

namespace Mivos_Bot.Context.IContext
{
    interface IUserContext
    {
        bool MuteUser(DiscordUser user);
        bool AddUser(DiscordUser user);
        bool User_exists(DiscordUser user);
        List<User> GetMuted();
        User GetUser(ulong uid);
        bool Unmute(ulong uid);
    }
}
