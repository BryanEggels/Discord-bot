using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mivos_Bot.Models
{
    class User
    {
        public ulong uid { get; set; }
        public string username { get; set; }
        public DateTime Mute_Expired { get; set; }
        public int Mutecount { get; set; }

    }
}
