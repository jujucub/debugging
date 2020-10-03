using System;

namespace Network
{
	public static class ByteOrderConverter
	{
		private static bool _isLittleEndian = BitConverter.IsLittleEndian;
		public static bool IsLittleEndian { get { return _isLittleEndian; } set { _isLittleEndian = value; } }

		public static short ToInt16(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert16(bytes);
			}
			return BitConverter.ToInt16(bytes, 0);
		}
		public static int ToInt32(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return BitConverter.ToInt32(bytes, 0);
		}
		public static long ToInt64(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return BitConverter.ToInt64(bytes, 0);
		}
		public static ushort ToUInt16(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert16(bytes);
			}
			return BitConverter.ToUInt16(bytes, 0);
		}
		public static uint ToUInt32(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return BitConverter.ToUInt32(bytes, 0);
		}
		public static ulong ToUInt64(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return BitConverter.ToUInt64(bytes, 0);
		}
		public static float ToSingle(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return BitConverter.ToSingle(bytes, 0);
		}
		public static double ToDouble(byte[] bytes)
		{
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return BitConverter.ToDouble(bytes, 0);
		}
		public static string ToString(byte[] bytes, int count)
		{
			if (IsNeedConvert())
			{
				Convert(bytes, count);
			}
			return System.Text.Encoding.UTF8.GetString(bytes, 0, count);
		}
		public static bool ToBoolean(byte[] bytes)
		{
			return bytes[0] != 0;
		}
		public static byte[] ToBytes(byte[] bytes, int count)
		{
			if (IsNeedConvert())
			{
				Convert(bytes, count);
			}
			return bytes;
		}

		public static byte[] GetBytes(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert16(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert16(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert32(bytes);
			}
			return bytes;
		}
		public static byte[] GetBytes(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert64(bytes);
			}
			return bytes;

		}
		public static byte[] GetBytes(string value)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
			if (IsNeedConvert())
			{
				Convert(bytes, bytes.Length);
			}
			return bytes;
		}
		public static byte[] GetBytes(bool value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			return bytes;
		}
		public static byte[] GetBytes(byte[] value, int count)
		{
			if (IsNeedConvert())
			{
				Convert(value, count);
			}
			return value;
		}
		public static bool IsNeedConvert()
		{
			if (_isLittleEndian != BitConverter.IsLittleEndian)
			{
				return true;
			}
			return false;
		}

		private static void Convert16(byte[] bytes)
		{
			byte tmpByte;
			tmpByte = bytes[0];
			bytes[0] = bytes[1];
			bytes[1] = tmpByte;
		}

		private static void Convert32(byte[] bytes)
		{
			byte tmpByte;
			tmpByte = bytes[0];
			bytes[0] = bytes[3];
			bytes[3] = tmpByte;
			tmpByte = bytes[1];
			bytes[1] = bytes[2];
			bytes[2] = tmpByte;
		}

		private static void Convert64(byte[] bytes)
		{
			byte tmpByte;
			tmpByte = bytes[0];
			bytes[0] = bytes[7];
			bytes[7] = tmpByte;
			tmpByte = bytes[1];
			bytes[1] = bytes[6];
			bytes[6] = tmpByte;
			tmpByte = bytes[2];
			bytes[2] = bytes[5];
			bytes[5] = tmpByte;
			tmpByte = bytes[3];
			bytes[3] = bytes[4];
			bytes[4] = tmpByte;
		}

		private static void Convert(byte[] bytes, int count)
		{
			Array.Reverse(bytes, 0, count);
		}
	}
}
