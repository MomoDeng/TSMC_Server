using System;
using System.Net.Sockets;
using System.Text;

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
        int max = 501;
        this.SendMsg(rnd.Next(min, max).ToString()); // 1 <= x < 501 

        //// 等待 Server 回覆
        string s = this.ReceiveMsg();

        //// 如果Server說結束則關掉
        if (s == "over")
        {
            //Console.WriteLine("Client Close:   ");
            this.CloseCLient();
        }
        else
        {
            //Console.WriteLine("There are still some msg");
        }
    }

    public void ClientStart()
    {
        _client = new TcpClient(serverIP, port);

        //Console.WriteLine("Client Start~\n");

        this.Act();
    }

    public void SendMsg(string msg)
    {
        //Console.WriteLine("SendMsg Start~");
        int byteCount = Encoding.ASCII.GetByteCount(msg);
        byte[] sendData = new byte[byteCount];
        sendData = Encoding.ASCII.GetBytes(msg);

        NetworkStream stream = _client.GetStream();
        if (stream.CanWrite)
        {
            stream.Write(sendData, 0, sendData.Length);
        }


    }

    public string ReceiveMsg()
    {
        //Console.WriteLine("trying to receive msg form server\n");

        byte[] receivedBuffer = new byte[_client.ReceiveBufferSize];
        NetworkStream stream = _client.GetStream();
        int numOfBytesRead ;
        string msg = string.Empty;

        if (stream.CanRead){
            do{
                numOfBytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                msg = Encoding.Default.GetString(receivedBuffer, 0, numOfBytesRead);

            } while (stream.DataAvailable);
        }

        //Console.WriteLine(msg + " | Len :" + msg.Length);

        return msg;

        //StringBuilder msg = new StringBuilder();

        //if (stream.CanRead)
        //{
        //    do
        //    {
        //        stream.Read(receivedBuffer, 0, receivedBuffer.Length);

        //        foreach (byte b in receivedBuffer)
        //        {
        //            if (b.Equals(00))
        //            {
        //                break;
        //            } // 00 == NULL
        //            else
        //            { msg.Append(Convert.ToChar(b).ToString()); }
        //        }
        //    } while (stream.DataAvailable);
        //}



        //Console.WriteLine("Receive finish");






    }

    public void CloseCLient()
    {
        _client.Close();
    }
}
