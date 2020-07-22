using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Test_Client
{
    class Program
    {

        static void Main(string[] args)
        {

            List<client> clients = new List<client>();
                
            const int maxNum = 10000;

            for (int i = 0; i < maxNum; i++) {

                //// 新增 client
                //Console.WriteLine("New Thread " + (i+1));
                client client = new client();
                Thread t = new Thread(
                    new ThreadStart(client.ClientStart));
                t.Start();
            }

            #region 小黑窗版本
            /*** 小黑窗傳送訊息
            while (true)
            {
                //// 讀取 Input 
                // 從 command line 得到 input
                //string msg;
                //Console.WriteLine("Please Enter your input...");
                //msg = Console.ReadLine();
                //Console.WriteLine("");

                //// 傳送 input 給 server
                //client.SendMsg(msg);

                //// 接收 server 訊息
                //client.ReceiveMsg();

            }
            ***/
            #endregion
        }
    }
}
