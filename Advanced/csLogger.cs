using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RT.Advanced
{
    class csLogger
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();      
        public string logName = string.Empty;
        private string logpath = string.Empty;
        private string DateTimeFormat = "yyyyMMddHHmmss";
        private int MaxSize = 10;

        public csLogger(string _logname)
        {
            logName = _logname;
            logpath = string.Format("{0}log",AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);
            logpath = logpath + "\\" + logName + ".log";
            if (!File.Exists(logpath))
            {
                File.Create(logpath);                
            }
        }

        public void WriteLine(string text)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                CheckFileSize();
                // Append text to the file
                using (StreamWriter sw = File.AppendText(logpath))
                {
                    sw.WriteLineAsync(string.Format("{0}\t{1}", DateTime.Now.ToString(DateTimeFormat), text));
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// if file size higher than specific MB, and recreate new file.
        /// </summary>
        private void CheckFileSize()
        {
            FileInfo file = new FileInfo(logpath);
            if (file.Length > (MaxSize * 1024 * 1024))
            {
                File.Move(logpath, logpath.Replace(logName, logName + "_" + DateTime.Now.ToString(DateTimeFormat)));
                File.Create(logpath);
            }else if (DateTime.Now.Day - file.CreationTime.Day > 0)
            {
                File.Move(logpath, logpath.Replace(logName, logName + "_" + file.CreationTime.ToString("yyyyMMdd235959")));
                File.Create(logpath);
            }
        }
    }
}
