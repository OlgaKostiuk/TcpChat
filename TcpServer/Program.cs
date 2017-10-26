using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("192.168.0.101", 9050);
            server.Start();
        }
    }
}
