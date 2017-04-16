using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Context.IContext;
using System.Data.SqlClient;
using Mivos_Bot.Context;
using Mivos_Bot.Repository;

namespace Mivos_Bot.Context
{
    class MessageSQLContext : IMessageContext
    {
        public bool AddMessage(DiscordMessage msg, ulong GuildID)
        {
            try
            {
                using (SqlConnection con = Database.Connection)
                {
                    if (!new UserRepository(new UserSQLContext()).SelectUser(msg.Author)) //if msgAuthor isnt already in our database, then add it
                    {
                        new UserRepository(new UserSQLContext()).AddUser(msg.Author);
                    }
                    string addmsg = "INSERT INTO discordmessage (MessageID, UserID,MessageHash,GuildID,Timestamp) VALUES (@msgid,@uid,@msghash,@gid,@time)";
                    SqlCommand cmd = new SqlCommand(addmsg, con);
                    cmd.Parameters.AddWithValue("@msgid", msg.ID.ToString());
                    cmd.Parameters.AddWithValue("@uid", msg.Author.ID.ToString());
                    cmd.Parameters.AddWithValue("@msghash", msg.Content);
                    cmd.Parameters.AddWithValue("@gid", GuildID.ToString());
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    if (cmd.ExecuteNonQuery() == 1) //als er meer dan 1 row affected is wordt dit true
                    {
                        return true;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
            
        }

        public bool CheckDuplicate(DiscordMessage msg, ulong GuildID)
        {
            using (SqlConnection con = Database.Connection)
            {
                string checkmsg = "SELECT * FROM discordmessage WHERE MessageHash = @msghash AND GuildID = @gid";
                SqlCommand cmd = new SqlCommand(checkmsg, con);

                cmd.Parameters.AddWithValue("@msghash", msg.Content);
                cmd.Parameters.AddWithValue("@gid", GuildID.ToString());
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        AddMessage(msg,GuildID);
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                return false;
            }
        }
    }
}
