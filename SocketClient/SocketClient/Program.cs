using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClient
{

    public class Client
    {
        public static void ConnectToServer()
        {

            //先建立IPAddress物件,IP為欲連線主機之IP

            //開始連線

            const int clientNum = 10000;
            IPAddress[] ipAddr = Dns.GetHostAddresses("localhost");
            IPEndPoint ipEnd = new IPEndPoint(ipAddr[0], 8000);
            TcpListener tcpListener = new TcpListener(ipEnd);
            // List<TcpClient> clientList = new List<TcpClient>();
            Random rng = new Random();
            for (int i = 0; i < clientNum; i++)
            {
                TcpClient tcpClient = new TcpClient();
                try
                {
                    tcpClient.ConnectAsync("127.0.0.1", 8000).Wait(2147483647);
                    if (tcpClient.Connected)
                    {
                        Console.WriteLine("連線成功");
                        CommunicationBase cb = new CommunicationBase();
                        string s = rng.Next(1, 500).ToString();
                        cb.SendMsg(s, tcpClient);
                        Console.WriteLine("send done.");
                    }
                    else
                    {
                        Console.WriteLine("連線失敗!");
                    }
                }
                catch (Exception ex)
                {
                    i--;
                    Console.WriteLine("Server拒絕連線，重新連線中...");
                    Thread.Sleep(1000);
                }
            }
            Console.ReadLine();
        }
        public static int Main(String[] args)
        {
            ConnectToServer();
            return 0;
        }
    }


}