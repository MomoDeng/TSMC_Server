
using System;
using System.Collections;
using System.Collections.Generic;
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
            const int MaxinumOfThread = 500;
            List<Thread> threads = new List<Thread>();
            while (true)
            {
                if (threads.Count >= MaxinumOfThread)
                {
                    List<Thread> rmTmpList = new List<Thread>();
                    for (int i = 0; i < threads.Count; i++)
                    {
                        if (threads[i].ThreadState == ThreadState.Stopped)
                        {
                            rmTmpList.Add(threads[i]);
                        }
                    }
                    for (int i = 0; i < rmTmpList.Count; i++)
                    {
                        threads.Remove(rmTmpList[i]);
                    }
                    Thread.Sleep(500);
                }
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
                        myThread.Start();
                        myThread.Name = tmpTcpClient.Client.RemoteEndPoint.ToString();
                        threads.Add(myThread);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();
                    break;
                }
            }
            Console.WriteLine();
        }
    }
}
