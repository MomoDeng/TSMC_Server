
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

        public void startListen()
        {
            Console.WriteLine("等待客戶端連線中... \n");
            TcpClient tmpTcpClient;
            int numberOfClients = 0;
            // const int MaxinumOfThread = 500;
            ThreadPool.SetMaxThreads(500, 500);
            Thread connThread = new Thread(new ThreadStart(new ConnPool().ListenStart));
            connThread.Start();
            while (true)
            {
                try
                {
                    if (ConnPool.clientPool.Count > 0)
                    {
                        //建立與客戶端的連線      
                        tmpTcpClient = ConnPool.clientPool.Dequeue();
                        if (tmpTcpClient.Connected)
                        {
                            // Console.WriteLine("連線成功!");
                            HandleClient handleClient = new HandleClient(tmpTcpClient, numberOfClients);
                            // Thread myThread = new Thread(new ThreadStart(handleClient.Communicate));
                            numberOfClients += 1;
                            // myThread.Start();
                            // myThread.Name = tmpTcpClient.Client.RemoteEndPoint.ToString();
                            ThreadPool.QueueUserWorkItem(new WaitCallback(handleClient.Communicate));
                        }
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
