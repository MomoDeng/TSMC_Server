
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Caller
{

	TcpClient _client;
	int _ID;
	//TxtHandler _txtHandler;

	public Caller(TcpClient client, int ID)
	{
		_client = client;
		_ID = ID;


	}

	public void CallerStart()
	{
		Console.WriteLine("Caller Start:   " +  _ID);
		string _msg = string.Empty;
		string _repMsg = string.Empty; //response Msg

		//// 從 Client 端得到變數
		int _Rtime;
		_msg = ReceiveMsg();
		_Rtime = int.Parse(_msg);

		//// 根據變數 Sleep(R)
		Thread.Sleep(_Rtime);

		//// 醒來後，開 XLOGS ，寫 “Client ID sleep R milliseconds and ready to leave now.”
		Console.WriteLine("Thread No." + _ID + " Wake up");
		TxtHandler.WriteSleepData(_ID, _Rtime);

		//// 結束連結
		/// 先跟 client 結束了
		/// 在切斷連線
		SendMsg("over");

		//if (_client.Connected) {
		//	Console.WriteLine("Trying to disconnect");
		//}


		_client.Close();
		if (!_client.Connected)
		{
			Console.WriteLine("Disconnected:    " + _ID);
		}

		//_client.Dispose();




		//// 紀錄結束置 XLOGS ，寫  “Client ID leaves!!”
		TxtHandler.WriteLeaveData(_ID);



		//while (true)
		//{
		//	_msg = ReceiveMsg();
		//	_repMsg = Table.GetMsgRespose(_msg);
		//	Thread.Sleep(1000);
		//	SendMsg(_repMsg);
		//	Console.WriteLine("\n\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ \n");
		//}

	}

	public string ReceiveMsg()
	{
		//Console.WriteLine("---GetMsg Start---");

		byte[] receivedBuffer = new byte[_client.ReceiveBufferSize];

		NetworkStream stream = _client.GetStream();
		StringBuilder msg = new StringBuilder();

		/// stream 要準備好才可讀取
		if (stream.CanRead)
		{
			do
			{
				stream.Read(receivedBuffer, 0, receivedBuffer.Length);

				foreach (byte b in receivedBuffer)
				{
					if (b.Equals(00))
					{
						break;
					} // 00 == NULL
					else
					{ msg.Append(Convert.ToChar(b).ToString()); }
				}
			} while (stream.DataAvailable);
		}



		Console.WriteLine("Tread NO." + _ID + " sleep " + msg.ToString());
		//Console.WriteLine("---GetMsg Over---");
		return msg.ToString();

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

}
