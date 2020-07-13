using System;
namespace SocketServer
{
    class SocketServer
    {
        public static int Main(String[] args)
        {
            AsynchronousSocketListener socket = new AsynchronousSocketListener();
            socket.startListen();
            return 0;
        }
    }
}
