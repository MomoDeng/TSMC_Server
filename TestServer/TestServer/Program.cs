using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Server _server = new Server();
            _server.ServerStart();
            
        }
    }
}
