using System;
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

            Console.WriteLine("Client Main Start~");

            client client = new client();
            client.ClientStart();

            while (true)
            {
                //// 讀取 Input 
                // 從 command line 得到 input
                string msg;
                Console.WriteLine("Please Enter your input...");
                msg = Console.ReadLine();
                Console.WriteLine("");

            
                // 傳送 input 給 server
                client.SendMsg(msg);

                // 接收 server 訊息
                client.ReceiveMsg();

            }

        }
    }

    class client
    {
        string serverIP = "LocalHost";
        int port = 8787;
        TcpClient _client;

        public void ClientStart()
        {
            _client = new TcpClient(serverIP, port);

            Console.WriteLine("Client Start~\n");

        }

        public void SendMsg(string msg) {

            int byteCount = Encoding.ASCII.GetByteCount(msg);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(msg );

            NetworkStream stream = _client.GetStream();
            stream.Write(sendData, 0, sendData.Length);

        }

        public void ReceiveMsg() {

            while (true) {
                Console.WriteLine("trying to receive msg form server\n");

                byte[] receivedBuffer = new byte[100];

                NetworkStream stream = _client.GetStream();

                stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                Console.WriteLine(receivedBuffer.ToString());

                StringBuilder msg = new StringBuilder();

                foreach (byte b in receivedBuffer)
                {
                    if (b.Equals(00)) // 00 == NULL
                    {
                        break;
                    }
                    else
                    {
                        msg.Append(Convert.ToChar(b).ToString());
                    }

                }

                Console.WriteLine(msg.ToString() + " | Len :" + msg.Length);

                Console.WriteLine("Receive finish");
                break;

            }


        }



    }

}
