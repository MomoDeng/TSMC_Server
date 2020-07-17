using System;
using System.IO;
using System.Threading;

public class FileWriter
{
	private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
	//private static FileStream _fileStream;
	private static StreamWriter _sw;
	private string _path = "XLOGS" + ".txt";

	public void WriteData(string _data) {

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
			_lock.ExitWriteLock();
		}


	}



}
