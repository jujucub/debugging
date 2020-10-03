#if ENABLE_LOG

using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Debugging
{
	public class FastLog
	{
		public enum LogLevel
		{
			Info1,
			Info2,
			Info3,
			Warning,
			Error,
		}

		int _index = 0;
		LogLevel _level = LogLevel.Info1;
		byte[] _buffer = null;

		string _path = string.Empty;
		string _filename = string.Empty;
		int _bufferSize = 0;
		char[] _newLine = null;
		System.IO.FileStream _fs = null;

		public byte[] Buffer { get { return _buffer; } }
		public int Size { get { return _index; } }
		public LogLevel Level { get { return _level; } set { _level = value; } }

		public FastLog(string path, string filename, int bufferSize = 102400)
		{
			_path = path;
			_filename = filename;
			_index = 0;
			_bufferSize = bufferSize;
			_buffer = new byte[_bufferSize];
			_newLine = System.Environment.NewLine.ToCharArray();
		}

		~FastLog()
		{
			Dispose();
		}

		public void LogLevel1(string msg)
		{
			Log(LogLevel.Info1, msg);
		}

		public void LogLevel2(string msg)
		{
			Log(LogLevel.Info2, msg);
		}


		public void LogLevel3(string msg)
		{
			Log(LogLevel.Info3, msg);
		}


		public void LogWarning(string msg)
		{
			Log(LogLevel.Warning, msg);
		}


		public void LogError(string msg)
		{
			Log(LogLevel.Error, msg);
		}


		public void TimeLogLevel1(string msg)
		{
			TimeLog(LogLevel.Info1, msg);
		}


		public void TimeLogLevel2(string msg)
		{
			TimeLog(LogLevel.Info2, msg);
		}


		public void TimeLogLevel3(string msg)
		{
			TimeLog(LogLevel.Info3, msg);
		}


		public void TimeLogWarning(string msg)
		{
			TimeLog(LogLevel.Warning, msg);
		}


		public void TimeLogError(string msg)
		{
			TimeLog(LogLevel.Error, msg);
		}


		public void TimeLog(LogLevel level, string msg, bool needStackTrace = false)
		{
			System.DateTime now = System.DateTime.Now;
			WriteBufferLevel(level);
			WriteBufferDate(now);
			Log(level, msg, needStackTrace);
		}


		public void Write()
		{
			if (_fs == null)
			{
				string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");
				if (!System.IO.Directory.Exists(_path))
				{
					System.IO.Directory.CreateDirectory(_path);
				}
				_fs = new System.IO.FileStream(_path + date + "_" + _filename, System.IO.FileMode.OpenOrCreate);
			}
			_fs.Write(_buffer, 0, _index);
			_index = 0;

		}

		public void Dispose()
		{
			if (_fs != null)
			{
				_fs.Close();
				_fs = null;
			}

		}

		void Log(LogLevel level, string msg, bool needStackTrace = false)
		{
			int size = 0;
			if (_level > level)
			{
				return;
			}
			if (needStackTrace)
			{
				string stackTrace = System.Environment.StackTrace;
				WriteBufferLevel(level);
				if ((_index + stackTrace.Length) >= _bufferSize)
				{
					Write();
				}
				size = System.Text.Encoding.UTF8.GetBytes(stackTrace, 0, stackTrace.Length, _buffer, _index);
				_index += size;
			}
			WriteBufferLevel(level);
			if ((_index + msg.Length + _newLine.Length) >= _bufferSize)
			{
				Write();
			}
			size = System.Text.Encoding.UTF8.GetBytes(msg, 0, msg.Length, _buffer, _index);
			_index += size;
			for (int i = 0; i < _newLine.Length; ++i)
			{
				WriteBufferByte((byte)_newLine[i]);
			}

		}

		void WriteBufferLevel(LogLevel level)
		{

			if ((_index + sizeof(byte) * 3) >= _bufferSize)
			{
				Write();
			}
			WriteBufferByte((byte)(((byte)level) + (byte)('0')));
			WriteBufferByte(((byte)':'));
			WriteBufferByte(((byte)' '));

		}

		void WriteBufferDate(System.DateTime time)
		{

			if (_index + (sizeof(int) * 7) + (sizeof(byte) * 8) >= _bufferSize)
			{
				Write();
			}
			WriteBufferByte((byte)'[');
			WriteBufferInt(time.Year);
			WriteBufferByte((byte)'/');
			WriteBufferInt(time.Month);
			WriteBufferByte((byte)'/');
			WriteBufferInt(time.Day);
			WriteBufferByte((byte)' ');
			WriteBufferInt(time.Hour);
			WriteBufferByte((byte)':');
			WriteBufferInt(time.Minute);
			WriteBufferByte((byte)':');
			WriteBufferInt(time.Second);
			WriteBufferByte((byte)':');
			WriteBufferInt(time.Millisecond);
			WriteBufferByte((byte)']');

		}

		// 高速化のためバッファサイズのチェックをしないので注意
		void WriteBufferByte(byte value)
		{
			_buffer[_index] = value;
			++_index;
		}

		// 高速化のためバッファサイズのチェックをしないので注意
		void WriteBufferInt(int value)
		{
			int digit = (int)Mathf.Log10(value);
			do
			{
				int digitBase = (int)Mathf.Pow(10, digit);
				int digitNum = (value / (digitBase > 0 ? digitBase : 1));
				WriteBufferByte((byte)(digitNum + '0'));
				value -= digitNum * digitBase;
				digit--;
			} while (value != 0);
		}
	}
}
#endif //#if ENABLE_LOG
