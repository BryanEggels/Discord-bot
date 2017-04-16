using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Context.IContext;

namespace Mivos_Bot.Repository
{
    class MessageRepository : IMessageContext
    {
        IMessageContext context;
        public MessageRepository(IMessageContext context)
        {
            this.context = context;
        }
        public bool AddMessage(DiscordMessage msg, ulong GuildID)
        {
            return context.AddMessage(msg,GuildID);
        }

        public bool CheckDuplicate(DiscordMessage msg, ulong GuildID)
        {
            return context.CheckDuplicate(msg, GuildID);
        }
    }
}
