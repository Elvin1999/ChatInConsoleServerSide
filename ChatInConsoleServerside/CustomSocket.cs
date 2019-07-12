using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatInConsoleServerside
{
   public class CustomSocket
    {
        public int Id { get; set; } = -5;
        public Socket Client { get; set; }
    }
}
