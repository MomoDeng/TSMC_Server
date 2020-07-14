using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    public class CommunicationBase
    {
        public void SendMsg(string msg, TcpClient tmpTcpClient)
        {
            NetworkStream ns = tmpTcpClient.GetStream();
            if (ns.CanWrite)
            {
                byte[] msgByte = Encoding.Default.GetBytes(msg);
                ns.Write(msgByte, 0, msgByte.Length);
            }
        }
        public string ReceiveMsg(TcpClient tmpTcpClient, int threadID)
        {
            string receiveMsg = string.Empty;
            byte[] receiveBytes = new byte[tmpTcpClient.ReceiveBufferSize];
            int numberOfBytesRead = 0;
            NetworkStream ns = tmpTcpClient.GetStream();
            if (ns.CanRead)
            {
                do
                {
                    numberOfBytesRead = ns.Read(receiveBytes, 0, tmpTcpClient.ReceiveBufferSize);
                    receiveMsg = Encoding.Default.GetString(receiveBytes, 0, numberOfBytesRead);
                    Thread.Sleep(int.Parse(receiveMsg));
                    FileWriter fw = new FileWriter();
                    fw.WriteData(String.Format("Client {0} sleep {1} milliseconds and ready to leave now.\n", threadID.ToString(), receiveMsg) , "XLOGS.txt");
                    fw.WriteData(String.Format("Client {0} leaves.\n", threadID.ToString()), "XLOGS.txt");
                }
                while (ns.DataAvailable);
            }
            return receiveMsg;
        }
    }
}
