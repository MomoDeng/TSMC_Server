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

	List<Caller> _Callers = new List<Caller>();

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
		TxtHandler.CreatTxt();
		this.NewClient();
	}

	public void NewClient() {

		List<TcpClient> LTcpC = new List<TcpClient>();

		while (true) {
			//Console.WriteLine("trying to connect new client...");

			//// 嘗試與新的client 做連結
			TcpClient client = default(TcpClient); // 等待新的 client
			client = _server.AcceptTcpClient();  //接受client

            //Console.WriteLine("add: " + $"({client})");

			LTcpC.Add(client);

			//// 開 Thread 給此 client
			//Console.WriteLine("");
			//Console.WriteLine("trying to creat new Thread for Caller...");
			Caller newCaller = new Caller(client, IdHandler.GetNewId());
			_Callers.Add(newCaller);	
			Thread t = new Thread(
				new ThreadStart(newCaller.CallerStart));
			t.Start();


			//Console.WriteLine("finish creat new Thread for Caller...");
			//Console.WriteLine("");
		}

	}


}

