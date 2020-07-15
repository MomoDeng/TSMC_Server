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
        ////
        /// 先幫 server寫分配ID
        /// 再把 client 做 send random num
        /// 把 Server 接受到的 R ，做 sleep(R)
        /// 
        /// 
        /// 醒來後再將指令寫入檔案
        /// 結束後，將結束訊息寫入檔案
        /// 找可以看thread的程式觀察
        /// 再把 client 主程式改成多 client

        static void Main(string[] args)
        {

            List<client> clients = new List<client>();
                
            int maxNum = 10000;

            for (int i = 0; i < maxNum; i++) {

                //// 新增 client
                Console.WriteLine("New Thread " + (i+1));
                client client = new client();
                Thread t = new Thread(
                    new ThreadStart(client.ClientStart));
                t.Start();
            }




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
        }
    }



}
