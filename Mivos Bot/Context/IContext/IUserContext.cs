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
        //mutes 1 user
        bool MuteUser(DiscordUser user);
        //adds 1 user to the database
        bool AddUser(DiscordUser user);
        //checks if a user is already in our database
        bool User_exists(DiscordUser user);
        //gets all muted users
        List<User> GetMuted();
        //gets a self-made user object from our database
        User GetUser(ulong uid);
        //unmutes the specified user
        bool Unmute(ulong uid);
        //resets the mutecount of the users
        bool MuteCountreset();

        //unmutes every user
        bool MuteReset();
    }
}
