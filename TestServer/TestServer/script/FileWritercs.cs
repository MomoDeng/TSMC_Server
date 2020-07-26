using System;
using System.IO;
using System.Threading;

public class FileWriter
{
	private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
	//private static FileStream _fileStream;
	private static StreamWriter _sw;
	private string _path = "XLOGS" + ".txt";

	private float _timeAll = 0f;
	



	public FileWriter() {
		
	}

	public void WriteData(string _data) {
		DateTime TimeStart;
		DateTime TimeEnd;
		TimeStart = DateTime.Now;
		_lock.EnterWriteLock();
		try
		{
			using (var _fileStream = new FileStream(_path, FileMode.Append, FileAccess.Write)) {
				_sw = new StreamWriter(_fileStream);
				_sw.WriteLine(_data);
				_sw.Close();
				_fileStream.Close();
			}
		}
		finally {
			TimeEnd = DateTime.Now;
			_timeAll = _timeAll + float.Parse( ((TimeSpan)(TimeEnd - TimeStart)).TotalMilliseconds.ToString() );
			printLockTime();
			_lock.ExitWriteLock();
		}


	}

	public void printLockTime() {
		Console.WriteLine("Lock Time: " + _timeAll);
	}



}
