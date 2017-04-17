using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace Mivos_Bot.Context.IContext
{
    interface IMessageContext
    {
        bool CheckDuplicate(DiscordMessage msg,ulong GuildID);
        bool AddMessage(DiscordMessage msg, ulong GuildID);
        //deletes all messages from the database!
        bool ResetMessages();
    }
}
