using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class HandleClient
    {
        private TcpClient mTcpClient;
        private int id;
        private Stopwatch StopWatch;
        public HandleClient(TcpClient _tmpTcpClient, int _id, Stopwatch _stopWatch)
        {
            this.mTcpClient = _tmpTcpClient;
            this.id = _id;
            this.StopWatch = _stopWatch;
        }
        public void Communicate(object state)
        {
            try
            {
                CommunicationBase cb = new CommunicationBase();
                string msg = cb.ReceiveMsg(this.mTcpClient, this.id, this.StopWatch);
                // Console.WriteLine("Client ID: {0} R: {1}", id.ToString(), msg + "\n");;
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                Console.WriteLine("客戶端主動關閉連線無法接收資料!");
                this.mTcpClient.Close();
            }
        } 
    } 
}
