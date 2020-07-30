using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

public class FileWriter
{
	
	private static string _path = "XLOGS" + ".txt";
	private static object _fileLock = new object();
	private static int _lockCount = 0;
	private static long _lockTotalTime = 0;

	private static Queue<string> _DataQue = new Queue<string>();
	private static Queue<string> _DataWritingQue = new Queue<string>();

	//private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
	//private static FileStream _fileStream;
	//private static StreamWriter _sw;

	public FileWriter() {
		
	}

    public void WriteData(string _data)
    {
        Stopwatch _stopWatch = new Stopwatch();

		_stopWatch.Start();

		lock (_fileLock) {
			_stopWatch.Stop();
			_lockCount++;
			_lockTotalTime = _lockTotalTime + _stopWatch.ElapsedMilliseconds;
			printLockTime(_lockTotalTime);

			//// Queue 版
			/// 如果資料量太多 可能會要catch 滿的狀況
			try {
				_DataQue.Enqueue(_data);
				//Thread.Sleep(5);
			}
			catch (Exception ex) {
				throw;
			}
			

			#region 直接 write 版
			//try
			//{
				
			//	//using (var _fileStream = new FileStream(_path, FileMode.Append, FileAccess.Write))
			//	//{
			//	//	byte[] dataAsByteArray = new UTF8Encoding(true).GetBytes(_data);
			//	//	_fileStream.Write(dataAsByteArray, 0, _data.Length);

				
			//}
			//catch (Exception ex) { 

			//}
			#endregion

		}	

	}

	public void printLockTime(long sec) {
		if (_lockCount % 1000 == 0) {
			Console.WriteLine("lock count: " + _lockCount );
			Console.WriteLine("lock aver time: " + (_lockTotalTime / Convert.ToInt64(_lockCount)));
			Console.WriteLine("lock Totle time: " + _lockTotalTime  );
			Console.WriteLine();
			
		}
		//Console.WriteLine($"LockTime {sec} ms");
	}

	public static void WriteSoul() {
		bool haveToWait = false;
		while (true) {
			if (_DataWritingQue.Count == 0)
			{
				lock (_fileLock) {
					_DataWritingQue = _DataQue;
					_DataQue = new Queue<string>();
					//Console.WriteLine("getWriteData");
				}
				Thread.Sleep(50);
			}
			else {
                using (var _fileStream = new FileStream(_path, FileMode.Append, FileAccess.Write))
                {
					string _data = _DataWritingQue.Dequeue();

					byte[] dataAsByteArray = new UTF8Encoding(true).GetBytes(_data);
                    _fileStream.Write(dataAsByteArray, 0, _data.Length);

                    #region 舊版
                    /*
					_sw = new StreamWriter(_fileStream);
					_sw.WriteLine(_data);
					_sw.Close();
					_fileStream.Close();
					*/
                    #endregion
                }
            }
		}
	}


}
