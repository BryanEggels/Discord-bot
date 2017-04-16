using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace Mivos_Bot.Context.IContext
{
    interface IUserContext
    {
        bool MuteUser(DiscordUser user);
        bool AddUser(DiscordUser user);
        bool SelectUser(DiscordUser user);
        List<ulong> GetMuted();
    }
}
