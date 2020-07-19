using System;
using System.Collections.Generic;
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
        public HandleClient(TcpClient _tmpTcpClient, int _id)
        {
            this.mTcpClient = _tmpTcpClient;
            this.id = _id;
        }
        public void Communicate(object state)
        {
            try
            {
                CommunicationBase cb = new CommunicationBase();
                string msg = cb.ReceiveMsg(this.mTcpClient, this.id);
                // Console.WriteLine("Client ID: {0} R: {1}", id.ToString(), msg + "\n");;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("客戶端強制關閉連線!");
                this.mTcpClient.Close();
            }
        } 
    } 
}
