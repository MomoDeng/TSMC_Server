using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    public class FileWriter
    {
        private static ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        public void WriteData(string dataWh, string filePath)
        {
            Stopwatch stopWatch = new Stopwatch();
            // stopWatch.Start();
            lock_.EnterWriteLock();
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    byte[] dataAsByteArray = new UTF8Encoding(true).GetBytes(dataWh);
                    fs.Write(dataAsByteArray, 0, dataWh.Length);
                }
            }
            finally
            {
                lock_.ExitWriteLock();
            }
            // stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            // Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}
