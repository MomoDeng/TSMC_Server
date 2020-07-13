
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{

    public class AsynchronousSocketListener
    {
        private IPAddress ipAddr;
        private IPEndPoint ipEnd;
        private TcpListener tcpListener;
        public AsynchronousSocketListener()
        {
            this.ipAddr = IPAddress.Parse("127.0.0.1");
            this.ipEnd = new IPEndPoint(ipAddr, 8000);
            this.tcpListener = new TcpListener(ipEnd);
        }

        public void startListen()
        {
            this.tcpListener.Start();
            Console.WriteLine("等待客戶端連線中... \n");
            TcpClient tmpTcpClient;
            int numberOfClients = 0;
            while (true)
            {
                try
                {
                    //建立與客戶端的連線
                    tmpTcpClient = this.tcpListener.AcceptTcpClient();
                    if (tmpTcpClient.Connected)
                    {
                        Console.WriteLine("連線成功!");
                        HandleClient handleClient = new HandleClient(tmpTcpClient, numberOfClients);
                        Thread myThread = new Thread(new ThreadStart(handleClient.Communicate));
                        numberOfClients += 1;
                        myThread.IsBackground = true;
                        myThread.Start();
                        myThread.Name = tmpTcpClient.Client.RemoteEndPoint.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();
                }
            }
        }
    }
}
