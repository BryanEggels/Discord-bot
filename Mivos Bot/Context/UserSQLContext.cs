using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Mivos_Bot.Context.IContext;
using System.Data.SqlClient;

namespace Mivos_Bot.Context
{
    class UserSQLContext : IUserContext
    {
        public bool AddUser(DiscordUser user)
        {
            try
            {
                using (SqlConnection con = Database.Connection)
                {
                    string userinsert = "INSERT INTO discorduser (UID,Username,MuteCount) VALUES (@uid,@username,@mc)";
                    SqlCommand cmd = new SqlCommand(userinsert, con);
                    cmd.Parameters.AddWithValue("@uid", user.ID.ToString());
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@mc", 0);
                    
                    if (cmd.ExecuteNonQuery() == 1) //als er meer dan 1 row affected is wordt dit true
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }

        public List<ulong> GetMuted()
        {
            using (SqlConnection con = Database.Connection)
            {
                string checkmsg = "SELECT UID FROM discorduser WHERE Mute_Expired > current_timestamp";
                SqlCommand cmd = new SqlCommand(checkmsg, con);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ulong> muteduserids = new List<ulong>();
                    while (reader.Read())
                    {
                        muteduserids.Add(Convert.ToUInt64(reader[0]));
                    }
                    return muteduserids;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return new List<ulong>();
                }
            }
        }

        public bool MuteUser(DiscordUser User)
        {
            string mute = "UPDATE discorduser SET mutecount = mutecount + 1, Mute_Expired = current_timestamp + POWER(2, mutecount) WHERE UID = @uid";

            try
            {
                using (SqlConnection con = Database.Connection)
                {
                    SqlCommand cmd = new SqlCommand(mute, con);
                    cmd.Parameters.AddWithValue("@uid", User.ID.ToString());

                    if (cmd.ExecuteNonQuery() == 1) //als er meer dan 1 row affected is wordt dit true
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }
        public bool SelectUser(DiscordUser user)
        {
            using (SqlConnection con = Database.Connection)
            {
                string checkmsg = "SELECT * FROM discorduser WHERE UID = @uid";
                SqlCommand cmd = new SqlCommand(checkmsg, con);

                cmd.Parameters.AddWithValue("@uid", user.ID.ToString());
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }


    }
}
