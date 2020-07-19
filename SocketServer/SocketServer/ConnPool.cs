using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class ConnPool
    {
        private IPAddress ipAddr;
        private IPEndPoint ipEnd;
        private TcpListener tcpListener;
        private const int MaxinumOfClient = 10000;
        public ConnPool()
        {
            this.ipAddr = IPAddress.Parse("127.0.0.1");
            this.ipEnd = new IPEndPoint(ipAddr, 8000);
            this.tcpListener = new TcpListener(ipEnd);
            this.tcpListener.Start();
        }
        public static Queue<TcpClient> clientPool = new Queue<TcpClient>();
        public void ListenStart()
        {
            while (true && clientPool.Count <= MaxinumOfClient)
            {
                try
                {
                    TcpClient tmpTcpClient;
                    //建立與客戶端的連線      
                    tmpTcpClient = this.tcpListener.AcceptTcpClient();
                    if (tmpTcpClient.Connected)
                    {
                        ConnPool.clientPool.Enqueue(tmpTcpClient);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
