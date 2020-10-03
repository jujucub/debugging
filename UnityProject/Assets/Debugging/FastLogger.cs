#if ENABLE_LOG
using UnityEngine;
using System.Collections;

namespace Debugging
{
	public static class FastLogger
	{
		public static FastLog.LogLevel Level { get { return _level; } set { _level = value; } }
		public static byte[] Buffer { get { if (_log == null) return null; return _log.Buffer; } }
		public static int Size { get { if (_log == null) return 0; return _log.Size; } }
		public static string FileName { get { return "DebugLog.txt"; } }

		private static string _FilePathCache = string.Empty;
		public static string FilePath
		{
			get
			{
				if (string.IsNullOrEmpty(_FilePathCache))
				{
					_FilePathCache = Application.temporaryCachePath + "/Log/";
				}

				return _FilePathCache;
			}
		}

		static FastLog.LogLevel _level = FastLog.LogLevel.Info1;
		static FastLog _log = new FastLog(FilePath, FileName);

		public static void Setting(FastLog.LogLevel level)
		{
			_log.Level = Level;
		}

		public static void Close()
		{
			_log.Write();
			_log.Dispose();
		}

		public static void LogLevel1(string msg)
		{
			_log.LogLevel1(msg);
		}
		public static void LogLevel2(string msg)
		{
			_log.LogLevel2(msg);
		}
		public static void LogLevel3(string msg)
		{
			_log.LogLevel3(msg);
		}
		public static void LogWarning(string msg)
		{
			_log.LogWarning(msg);
		}
		public static void LogError(string msg)
		{
			_log.LogError(msg);
		}

		public static void TimeLogLevel1(string msg)
		{
			_log.TimeLogLevel1(msg);
		}
		public static void TimeLogLevel2(string msg)
		{
			_log.TimeLogLevel2(msg);
		}
		public static void TimeLogLevel3(string msg)
		{
			_log.TimeLogLevel3(msg);
		}
		public static void TimeLogWarning(string msg)
		{
			_log.TimeLogWarning(msg);
		}
		public static void TimeLogError(string msg)
		{
			_log.TimeLogError(msg);
		}

		public static void Flush()
		{
			_log.Write();
		}

		public static void Reflection()
		{
			_log.Level = _level;
		}
	}
}
#endif //#if ENABLE_LOG
