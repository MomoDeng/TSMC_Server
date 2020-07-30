using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class client
{


    string serverIP = "LocalHost";
    int port = 8787;
    TcpClient _client;


    public client()
    {

    }

    public void Act() {

        //// 隨機產生亂數並作為訊息，傳送給 Server
        Random rnd = new Random();
        int min = 1;
        int max = 502;
        int randomNum = rnd.Next(min, max); // min <= x < max


        this.SendMsg(randomNum.ToString());

        #region Exception Ver
        //if (randomNum < 0)
        //{
        //    this.SendMsg(randomNum.ToString());
        //} else if (randomNum < 300) {
        //    this.SendMsg("A type over Num " + randomNum.ToString());
        //}
        //else {
        //    this.SendMsg("B type over Num " + randomNum.ToString());
        //}
        #endregion

        //// 等待 Server 回覆
        string s = this.ReceiveMsg();

        //// 如果Server說結束則關掉
        if (s == "over")
        {
            //Console.WriteLine("Client Close  " );
            //Console.Read();
            this.CloseCLient();
        }
        else
        {
            //Console.WriteLine("There are still some msg");
        }
    }

    public void ClientStart()
    {
        try {
            _client = new TcpClient(serverIP, port);
        } catch (Exception ex ){
            Console.WriteLine("建立連結失敗...");
            Thread.Sleep(1000);
            this.ClientStart();
        }
        

        //Console.WriteLine("Client Start~\n");

        this.Act();
    }

    public void SendMsg(string msg)
    {
        //Console.WriteLine("SendMsg Start~");
        try {
            int byteCount = Encoding.ASCII.GetByteCount(msg);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(msg);

            NetworkStream stream = _client.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(sendData, 0, sendData.Length);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("無法傳送資料...");
            Thread.Sleep(500);
        }



    }

    public string ReceiveMsg()
    {
        //Console.WriteLine("trying to receive msg form server\n");
        try
        {
            byte[] receivedBuffer = new byte[_client.ReceiveBufferSize];
            NetworkStream stream = _client.GetStream();
            int numOfBytesRead;
            string msg = string.Empty;

            if (stream.CanRead)
            {
                do
                {
                    numOfBytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                    msg = Encoding.Default.GetString(receivedBuffer, 0, numOfBytesRead);

                } while (stream.DataAvailable);
            }
            return msg;
        }
        catch (Exception ex) {
            Console.WriteLine("無法獲得資料");
            Thread.Sleep(500);
            return "";
        }


        //Console.WriteLine(msg + " | Len :" + msg.Length);

    }

    public void CloseCLient()
    {
        _client.Close();
    }
}
