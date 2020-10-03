using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace Debugging
{
    public class Log
    {
        public string tag; 
        public string text;
        public string stack;
        public System.DateTime date;
    }

    public static class Logger
    {
        static object _lock = new object();
        static FileStream _fs;
        static StringBuilder _builder = new StringBuilder(1024);
        static System.Threading.Thread _thread;

        public static void Initialize()
        {
            _thread = new System.Threading.Thread(Thread);
            _thread.Start();
        }

        public static void Log(string str)
        {
            Write(str);
        }

        static void Write(string str)
        {
            lock(_lock)
            {
                _builder.Append(str);
            }
        }

        public static void Finish()
        {
            if (_thread != null)
            {
                _thread.Abort();
            }

            WriteFile();

            if (_fs != null)
            {
                _fs.Close();
                _fs.Dispose();
            }
        }

        static void Thread()
        {
            while(true)
            {
                if(_builder.Length > _builder.MaxCapacity * 0.8f)
                {
                    lock (_lock)
                    {
                        WriteFile();
                    }
                }
                System.Threading.Thread.Sleep(16);
            }
        }

        static void WriteFile()
        {
            if (_fs == null)
            {
                _fs = new FileStream($"{System.DateTime.Now.ToString("yyyy-dd-MM_HH-mm-ss")}.log", FileMode.OpenOrCreate);
            }
            var bytes = System.Text.Encoding.Default.GetBytes(_builder.ToString());
            _fs.Write(bytes, 0, bytes.Length);
            _builder.Clear();
        }
    }
}
