using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Callee
{

	private TcpClient _client;
	private int _ID;
	private FileWriter fw;

	public Callee(TcpClient client, int ID)
	{
		_client = client;
		_ID = ID;
		fw = new FileWriter();
	}

	public void CalleeStart()
	{
		//Console.WriteLine("Caller Start:   " +  _ID);
		string _msg;
		string _repMsg;  //response Msg

		//// 從 Client 端得到訊息
		int _Rtime;
		_msg = ReceiveMsg();

		//// 將訊息轉成變數
		_Rtime = int.Parse(_msg);

		//// 根據變數 Sleep(R)
		Thread.Sleep(_Rtime);

		//// 醒來後，開 XLOGS ，寫 “Client ID sleep R milliseconds and ready to leave now.”
		//Console.WriteLine("Thread No." + _ID + " Wake up");
		fw.WriteData("Client " + _ID + " sleep " + _Rtime + " milliseconds and ready to leave now. ");
		//TxtHandler.WriteSleepData(_ID, _Rtime);

		//// 結束連結
		/// 先跟 client 結束了
		/// 在切斷連線
		_repMsg = "over";
		SendMsg(_repMsg);
		_client.Close();

		////  client.Dispose ???



		//// 紀錄結束置 XLOGS ，寫  “Client ID leaves!!”
		fw.WriteData("Client " + _ID + " leaves!!");
		//TxtHandler.WriteLeaveData(_ID);

		//Console.WriteLine(_ID + " over");

	}

	public string ReceiveMsg()
	{
		//Console.WriteLine("---GetMsg Start---");

		byte[] receivedBuffer = new byte[_client.ReceiveBufferSize];
		NetworkStream stream = _client.GetStream();
		string msg = string.Empty;
		int numOfBytesRead;

		///// stream 要準備好才可讀取
		if (stream.CanRead) {
			do {
				numOfBytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
				msg = Encoding.Default.GetString(receivedBuffer, 0, numOfBytesRead);


			} while (stream.DataAvailable);
		}

		//Console.WriteLine("Tread NO." + _ID + " sleep " + msg);
		//Console.WriteLine("---GetMsg Over---");
		return msg;
	}

	public void SendMsg(string _repMsg)
	{
		//Console.WriteLine("---SendMsg Start---");

		int byteCount = Encoding.ASCII.GetByteCount(_repMsg);
		byte[] sendData = new byte[byteCount];
		sendData = Encoding.ASCII.GetBytes(_repMsg);

		NetworkStream stream = _client.GetStream();
		stream = _client.GetStream();

		/// stream 要準備好才可寫
		if (stream.CanWrite)
		{
			stream.Write(sendData, 0, sendData.Length);
		}

		//Console.WriteLine("Send: " + _repMsg);
		//Console.WriteLine("---SendMsg over---");
	}

	public int GetID() {
		return _ID;
	}

}
