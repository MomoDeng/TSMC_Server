using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{

    public class Client
    {
        public static void ConnectToServer()
        {

            //先建立IPAddress物件,IP為欲連線主機之IP
            IPAddress[] ipAddr = Dns.GetHostAddresses("localhost");
            IPEndPoint ipEnd = new IPEndPoint(ipAddr[0], 8000);
            TcpListener tcpListener = new TcpListener(ipEnd);
            TcpClient tcpClient = new TcpClient();
            //開始連線
            try
            {
                Console.WriteLine("主機IP=" + ipAddr.ToString());
                Console.WriteLine("連線至主機中...\n");
                tcpClient.Connect("127.0.0.1", 8000);
                if (tcpClient.Connected)
                {
                    Console.WriteLine("連線成功!");
                    CommunicationBase cb = new CommunicationBase();
                    int cnt = 0;
                    while (true)
                    {
                        string s = Console.ReadLine();
                        cb.SendMsg(s, tcpClient);
                        Console.WriteLine(cb.ReceiveMsg(tcpClient));
                    }
                }
                else
                {
                    Console.WriteLine("連線失敗!");
                    Console.Read();
                }
                
            }
            catch (Exception ex)
            {
                tcpClient.Close();
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
        public static int Main(String[] args)
        {
            ConnectToServer();
            return 0;
        }
    }

    
}