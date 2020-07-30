using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{

	private IPAddress ip;
	private int port;
	private TcpListener _server;
	private IdHandler IdHandler;
	private CallingMachine _callingMachine;

	//private List<Thread> _calleeThreads;
	//private Queue<Callee> _callees;

	const int MaxNumOfThread = 1000;


	public Server()
	{
		ip = Dns.GetHostEntry("LocalHost").AddressList[0];
		port = 8787;
		IdHandler = new IdHandler();

		_server = new TcpListener(ip, port);
		//_calleeThreads = new List<Thread>();
		_callingMachine = new CallingMachine();

		ThreadPool.SetMinThreads(MaxNumOfThread, MaxNumOfThread);
		ThreadPool.SetMaxThreads(MaxNumOfThread, MaxNumOfThread);

	}

	public void ServerStart() {
		try
		{
			_server.Start();
			Console.WriteLine("Server Started...");
			this.ServerAct();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}



	public void ServerAct() {
		
        
		Thread ClientListener = new Thread(
			new ThreadStart(this.ClientListener));
		ClientListener.Start();

        Thread CalleeDistributor = new Thread(
            new ThreadStart(this.CalleeDistributor));
        CalleeDistributor.Start();

		Thread WriteSoul = new Thread(
			new ThreadStart(FileWriter.WriteSoul));
		WriteSoul.Start();

		#region 舊版

		//Thread CalleeAcceptor = new Thread(
		//    new ThreadStart(this.CalleeAcceptor));
		//CalleeAcceptor.Start();

		//Thread CalleeThreadDistributor = new Thread(
		//    new ThreadStart(this.CalleeThreadDistributor));
		//CalleeThreadDistributor.Start();

		#endregion


	}


    public void ClientListener() {
		while (true) {

			//// 接收新 client -> 先排隊，等叫人
			try {
                ListenNewClient();
            }
			catch (Exception ex) { 

			}
        }
    }

	public void ListenNewClient()
	{
		TcpClient client = default(TcpClient);
		client = _server.AcceptTcpClient();
		_callingMachine.AddNewClient(client);

		//Callee callee = new Callee(client, IdHandler.GetNewId());
		//ThreadPool.QueueUserWorkItem(new WaitCallback(callee.CalleeStart));
	}


	public void CalleeDistributor() {
		while (true) {
			//// 若 pool 還有可用 Thread AND 有人在排隊 ->  提供在排隊的 client 服務
			if (IsThreadAvailable() && _callingMachine.HaveClientWaiting()) {
				GiveService2client();
			}
		}
	}

	public void GiveService2client() {
		TcpClient client = _callingMachine.GetClientFromQueue();
		Callee callee = new Callee(client, IdHandler.GetNewId());
		ThreadPool.QueueUserWorkItem(new WaitCallback(callee.CalleeStart));

		//try {
		//	ThreadPool.QueueUserWorkItem(new WaitCallback(callee.CalleeStart));
		//}
		//catch (FormatException ex) {
		//	Console.WriteLine("GiveService2client catch");
		//	client.Close();
		//	return;
		//}
		//catch (Exception ex) {
		//	Console.WriteLine("go through GiveService2client");
		//	client.Close();
		//	Console.WriteLine("client close");
		//	return;
		//}
		


	}

	public bool IsThreadAvailable() {
		int currentAvailableThreads, asyncAvailableThreads;
		ThreadPool.GetAvailableThreads(out currentAvailableThreads, out asyncAvailableThreads);

		if (currentAvailableThreads > 0) return true;
		else return false;
	}




	//public void CalleeThreadDistributor() {
 //       //// 檢查無用的 thread ，派發新 callee 給 Thread  


 //       #region 舊版

 //       while (true)
 //       {
 //           if (_calleeThreads.Count < MaxNumOfThread) continue;

 //           //// 找出已經停止的 Thread 
 //           List<int> stopedCallees = new List<int>();
 //           int range = _calleeThreads.Count;
 //           for (int i = 0; i < range; i++)
 //           {
 //               //Console.WriteLine("Thread CountXXX: " + _calleeThreads.Count);
 //               //Thread.Sleep(5);
 //               if (_calleeThreads[i].ThreadState == ThreadState.Stopped)
 //               {
 //                   stopedCallees.Add(i);
 //                   //Console.WriteLine("Thread " + i + " stop");
 //               }
 //           }

 //           #region 派發 callee 給 Thread
 //           ///派發
 //           for (int i = 0; i < stopedCallees.Count; i++)
 //           {
 //               if (_callingMachine.HaveClientWaiting())
 //               {
 //                   Callee TmpcCallee = _callingMachine.GetCalleeFromQueue();
 //                   //Console.WriteLine("GetCalleeFromQueue: " + TmpcCallee.GetID());

 //                   /*
	//				ThreadStart Tstart = new ThreadStart(TmpcCallee.CalleeStart);
	//				_calleeThreads[stopedCallees[i]].Start(Tstart);
	//				*/

 //                   ThreadPool.QueueUserWorkItem(new WaitCallback(TmpcCallee.CalleeStart));

 //                   //_calleeThreads[stopedCallees[i]] = new Thread(
 //                   //	new ThreadStart(TmpcCallee.CalleeStart));
 //                   //_calleeThreads[stopedCallees[i]].Start();

 //               }

 //           }

 //           #endregion
 //       }
 //       #endregion

 //   }


	//public void CalleeAcceptor() {
 //       while (true) {
 //           #region 舊版
 //           try
 //           {
 //               //// 嘗試與新的client 做連結
 //               TcpClient client = default(TcpClient); // 等待新的 client
 //               client = _server.AcceptTcpClient();  //接受client
 //                                                    //Console.WriteLine("Thread Count: " + _calleeThreads.Count);

 //               //// 派發 ID 與 callee
 //               Callee newCallee = new Callee(client, IdHandler.GetNewId());


 //               //// 如果還有 Thread 可用，則新增client
 //               //// 若沒有 thread 可用，則叫他排隊
 //               if (_calleeThreads.Count < MaxNumOfThread)
 //               {
 //                   /*
 //                   //// 嘗試與新的client 做連結
 //                   TcpClient client = default(TcpClient); // 等待新的 client
 //                   client = _server.AcceptTcpClient();  //接受client

 //                   //// 派發 ID 與 callee
 //                   Callee newCallee = new Callee(client, IdHandler.GetNewId());
 //                   */


 //                   //// 取用 Thread forom ThreadPool
 //                   ThreadPool.QueueUserWorkItem(new WaitCallback(newCallee.CalleeStart));

 //                   //// 新增 Thread 
 //                   //Thread t = new Thread(
 //                   //	new ThreadStart(newCallee.CalleeStart));
 //                   //_calleeThreads.Add(t);
 //                   //t.Start();



 //                   //Console.WriteLine("New Thread: " + +newCallee.GetID());
 //               }
 //               else
 //               {
 //                   //// 排隊
 //                   _callingMachine.AddNewCallee(newCallee);

 //                   /*
 //                   if ( !_callingMachine.IsQueFull())
 //                   {
 //                       //// 嘗試與新的client 做連結
 //                       TcpClient client = default(TcpClient); // 等待新的 client
 //                       client = _server.AcceptTcpClient();  //接受client

 //                       //// 派發 ID 與 callee
 //                       Callee newCallee = new Callee(client, IdHandler.GetNewId());

 //                       //// 排隊
 //                       _callingMachine.AddNewClient(newCallee);
 //                   }
 //                   else {
 //                       //// 
 //                       Thread.Sleep(50);
 //                   }
 //                   */
 //               }

 //           }
 //           catch (Exception ex)
 //           {
 //               Console.WriteLine(ex.Message);
 //               Console.Read();
 //               break;
 //           }
 //           #endregion
 //       }

 //   }


}

