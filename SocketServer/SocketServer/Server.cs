
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            const bool isConnPool = false;
            // const int MaxinumOfThread = 500;
            ThreadPool.SetMinThreads(500, 500);
            ThreadPool.SetMaxThreads(500, 500);
            // Conn Pool
            // Thread connThread = new Thread(new ThreadStart(new ConnPool().ListenStart));
            // connThread.Start();

            // one to one
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddr, 8000);
            TcpListener tcpListener = new TcpListener(ipEnd);
            tcpListener.Start();
            while (true)
            {
                try
                {
                    int currentAvailableThreads, asyncAvailableThreads;
                    ThreadPool.GetAvailableThreads(out currentAvailableThreads, out asyncAvailableThreads);
                    if (currentAvailableThreads  > 0 && (ConnPool.clientPool.Count > 0 || !isConnPool))
                    {
                        //建立與客戶端的連線      
                        // tmpTcpClient = ConnPool.clientPool.Dequeue();
                        tmpTcpClient = tcpListener.AcceptTcpClient();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        if (tmpTcpClient.Connected)
                        {
                            // Console.WriteLine("連線成功!");
                            HandleClient handleClient = new HandleClient(tmpTcpClient, numberOfClients, stopWatch);
                            // multi-thread (non-pool)
                            // Thread myThread = new Thread(new ThreadStart(handleClient.Communicate));
                            // myThread.Start();
                            // myThread.Name = tmpTcpClient.Client.RemoteEndPoint.ToString();
                            numberOfClients += 1; 
                            ThreadPool.QueueUserWorkItem(new WaitCallback(handleClient.Communicate));
                        }
                    }
                    if (numberOfClients == 10000)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();
                    break;
                }
            }
            Console.WriteLine("10000個client都已排隊完成");
            Console.ReadLine();
        }
    }
}
