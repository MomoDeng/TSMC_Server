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
	IdHandler IdHandler = new IdHandler();

	List<Callee> _Callees = new List<Callee>();

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
		//TxtHandler.CreatTxt();
		this.NewClient();
	}

	public void NewClient() {

		while (true) {

			//// 嘗試與新的client 做連結
			TcpClient client = default(TcpClient); // 等待新的 client
			client = _server.AcceptTcpClient();  //接受client

			//// 開 Thread 給此 client
			Callee newCaller = new Callee(client, IdHandler.GetNewId());
			_Callees.Add(newCaller);	
			Thread t = new Thread(
				new ThreadStart(newCaller.CalleeStart));
			t.Start();

		}

	}


}

