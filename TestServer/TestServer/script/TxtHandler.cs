using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;


public static class TxtHandler
{
	static string _path;
	static FileStream _fileStream;
	static StreamWriter sw;

    static Queue<string> _queueStr = new Queue<string>();
	static bool _isWriting = false;

	

    public static void CreatTxt() {

		Console.WriteLine("CreatTxt");

		_path = System.IO.Directory.GetCurrentDirectory();
		_path = System.AppDomain.CurrentDomain.BaseDirectory;

		_path = _path + "..\\..\\..\\";
		_path = _path + "Data\\";
		_path = _path + "XLOGS" + ".txt";
		_path =  "XLOGS" + ".txt"; // 會在bin李

		Thread t = new Thread(
			new ThreadStart(TxtHandler.WritingData));
		t.Start();

	}

	public static void WriteSleepData(int Id, int r) {
		//// 寫 “Client ID sleep R milliseconds and ready to leave now.”
		string s = String.Empty;
		s = "Client " + Id + " sleep " + r + " milliseconds and ready to leave now. ";
		Console.WriteLine(s);

		//// 塞入queue
		_queueStr.Enqueue(s);

	}

	public static void WriteLeaveData(int Id)
	{
		//// 寫  “Client ID leaves!!”
		string s = String.Empty;
		s = "Client " +  Id + " leaves!!";
		Console.WriteLine(s);

		//// 塞入queue
		_queueStr.Enqueue(s);

	}

	public static void WritingData() {
		while (true) {
			if (_isWriting) {
				Thread.Sleep(5); //有人在寫資料，等一下
			}
			else {
				//// 若有東西則寫，無則等待
				if (_queueStr.Count != 0) {
					_isWriting = true;

					//開始寫入值
					_fileStream = new FileStream(_path, FileMode.Append, FileAccess.Write); 
					sw = new StreamWriter(_fileStream);
					sw.WriteLine(_queueStr.Dequeue());
					sw.Close();
					_fileStream.Close();

					_isWriting = false;
				}
				else {
					Thread.Sleep(5);
				}
			}
		}
	}

	public static void DisposeStream() {
		//sw.Close();
		//_fileStream.Close();
	}
}
