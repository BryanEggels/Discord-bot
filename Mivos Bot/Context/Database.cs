using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Mivos_Bot.Context
{
    class Database
    {
        public static SqlConnection Connection
        {
            get
            {
                
                SqlConnection connection = new SqlConnection(connectionstring);
                try
                {
                    connection.Open();
                    return connection;
                }
                 catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return connection;
            }
         }
        private static string connectionstring = File.ReadAllText("connectionstring.txt");
        
    }
}
