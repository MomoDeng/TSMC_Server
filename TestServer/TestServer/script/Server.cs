using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
	///// 增加 client queue
	///// 增加 server叫號 client等待 
	///// 增加thread pool

	private IPAddress ip;
	private int port;
	private TcpListener _server;
	private IdHandler IdHandler;

	private List<Thread> _calleeThreads;
	private CallingMachine _callingMachine;

	const int MaxNumOfThread = 500;



	public Server()
	{
		ip = Dns.GetHostEntry("LocalHost").AddressList[0];
		port = 8787;
		IdHandler = new IdHandler();

		_server = new TcpListener(ip, port);
		_calleeThreads = new List<Thread>();
		_callingMachine = new CallingMachine();
	}

	public void ServerStart() {
		
		try
		{
			_server.Start();
			Console.WriteLine("Server Started...");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		//TxtHandler.CreatTxt();
		this.ServerAct();
	}



	public void ServerAct() {
		
		Thread ClientAcceptor = new Thread(
			new ThreadStart(this.ClientAcceptor));
		ClientAcceptor.Start();

		Thread.Sleep(500);

		Thread CalleeThreadDistributor = new Thread(
			new ThreadStart(this.CalleeThreadDistributor));
		CalleeThreadDistributor.Start();	

	}


	public void ClientAcceptor() {
		while (true) {
			try
			{
				//// 嘗試與新的client 做連結
				TcpClient client = default(TcpClient); // 等待新的 client
				client = _server.AcceptTcpClient();  //接受client
				//Console.WriteLine("Thread Count: " + _calleeThreads.Count);

				//// 派發 ID 與 callee
				Callee newCallee = new Callee(client, IdHandler.GetNewId());


				//// 如果還有 Thread 可用，則新增client
				//// 若沒有 thread 可用，則叫他排隊
				if (_calleeThreads.Count < MaxNumOfThread)
				{
					//// 新增 Thread 
					Thread t = new Thread(
						new ThreadStart(newCallee.CalleeStart));
					_calleeThreads.Add(t);
					t.Start();
					//Console.WriteLine("New Thread: " + +newCallee.GetID());
				}
				else
				{
					//// Thread太多，則排隊
					_callingMachine.AddNewClient(newCallee);
					//Console.WriteLine("Queue Callee: " + newCallee.GetID());
					
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.Read();
				break;
			}
		}
	}

	public void CalleeThreadDistributor() {
		//// 檢查無用的 thread ，派發新 callee 給 Thread  

		while (true) {
			if (_calleeThreads.Count < MaxNumOfThread) continue;

			List<int> stopedCallees = new List<int>();
			int range = _calleeThreads.Count;
			for (int i = 0; i < range ; i++)
			{
				//Console.WriteLine("Thread CountXXX: " + _calleeThreads.Count);
				//Thread.Sleep(5);
				if (_calleeThreads[i].ThreadState == ThreadState.Stopped)
				{
					stopedCallees.Add(i);
					//Console.WriteLine("Thread " + i + " stop");
				}
			}

			#region 派發 callee 給 Thread
			///派發
			for (int i = 0; i < stopedCallees.Count; i++)
			{
				if (_callingMachine.HaveClientWaiting())
				{
					Callee TmpcCallee = _callingMachine.GetCalleeFromQueue();
					//Console.WriteLine("GetCalleeFromQueue: " + TmpcCallee.GetID());

					_calleeThreads[stopedCallees[i]] = new Thread(
						new ThreadStart(TmpcCallee.CalleeStart));
					_calleeThreads[stopedCallees[i]].Start();

				}

			}

			#endregion
		}
	}

	//private void Connect2NewClient() {
	//	Console.WriteLine("add");
	//	////// 嘗試與新的client 做連結
	//	//TcpClient client = default(TcpClient); // 等待新的 client
	//	//client = _server.AcceptTcpClient();  //接受client
	//	//Callee newCallee = new Callee(client, IdHandler.GetNewId());

	//	//// 開 Thread 給此 client
	//	//Thread t = new Thread(
	//	//	new ThreadStart(newCallee.CalleeStart));
	//	//_calleeThreads.Add(t);
	//	//t.Start();
	//}

	//private void EnqueNewClient() {
	//	Console.WriteLine("Que");
	//	//// 嘗試與新的client 做連結
	//	//TcpClient client = default(TcpClient); // 等待新的 client
	//	//client = _server.AcceptTcpClient();  //接受client

	//	////// 派發 ID 與 callee 但不給 Tread
	//	//Callee newCallee = new Callee(client, IdHandler.GetNewId());
	//	//_callingMachine.AddNewClient(newCallee);
	//}





}

