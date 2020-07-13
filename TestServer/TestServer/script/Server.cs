using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
	IPAddress ip = Dns.GetHostEntry("LocalHost").AddressList[0];
	int port = 8787;
	TcpListener _server;


    public Server()
	{

	}

	public void ServerStart() {
		_server = new TcpListener(ip, port);
		try
		{
			_server.Start();
			Console.WriteLine("Server Started...");

		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());

		}
		this.NewClient();
	}

	public void NewClient() {

		List<TcpClient> LTcpC = new List<TcpClient>();

		while (true) {
			Console.WriteLine("trying to connect new client...");

			//// 嘗試與新的client 做連結
			TcpClient client = default(TcpClient); // 等待新的 client
			client = _server.AcceptTcpClient();  //接受client

            Console.WriteLine("add: " + $"({client})");

			LTcpC.Add(client);

			//// 開 Thread 給此 client
			Console.WriteLine("");
			Console.WriteLine("trying to creat new Thread for Caller...");
			Caller newCaller = new Caller(client);
			Thread t = new Thread(
				new ThreadStart(newCaller.CallerStart));
			t.Start();

			if (LTcpC.Count == 2) {
				if (LTcpC[0].Equals(LTcpC[1]))
				{
					Console.WriteLine("SSSSSSSSSSSSSSSSSSame  ");
				}
				else {
					Console.WriteLine("NNNNNNNNNNNNNN Same  ");

				}
			}

			Console.WriteLine("finish creat new Thread for Caller...");
			Console.WriteLine("");
		}

	}


}


public class Caller {

	TcpClient _client;
	

	public Caller(TcpClient client) {
		_client = client;
	}

	public void CallerStart() {
		Console.WriteLine("Caller Start~~~");
		string _msg = string.Empty;
		string _repMsg = string.Empty; //response Msg

		while (true) {
			_msg = GetMsg();
			_repMsg = Table.GetMsgRespose(_msg);
			Thread.Sleep(1000);
			SendMsg(_repMsg);
			Console.WriteLine("\n\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ \n");
		}

	}

	public string GetMsg() {
		Console.WriteLine("---GetMsg Start---");

		byte[] receivedBuffer = new byte[100];

        NetworkStream stream = _client.GetStream();

        stream.Read(receivedBuffer, 0, receivedBuffer.Length);

		Console.WriteLine("rLen: " + receivedBuffer.Length);

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
		Console.WriteLine(msg.ToString());
		Console.WriteLine("---GetMsg Over---");
		return msg.ToString();

	}

    public void SendMsg(string _repMsg) {
		Console.WriteLine("---SendMsg Start---");

        int byteCount = Encoding.ASCII.GetByteCount(_repMsg);
        byte[] sendData = new byte[byteCount];
        sendData = Encoding.ASCII.GetBytes(_repMsg);

		NetworkStream stream = _client.GetStream();
		stream = _client.GetStream();
        stream.Write(sendData, 0, sendData.Length);
		Console.WriteLine("Send: " + _repMsg);
		Console.WriteLine("---SendMsg over---");

	}

}
