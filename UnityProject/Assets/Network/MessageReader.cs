//#define USE_MESSAGE_PACk
using System;
using System.Collections.Generic;

namespace Network
{
	public sealed class MessageReader : IDisposable
	{
		byte[] _tmpBuffer = new byte[64];
		System.IO.MemoryStream _stream;

#if USE_MESSAGE_PACK
		Colopl.Serialization.MsgPack.Deserializer _deserializer;
#endif
		public long ReadSize { get { return _stream.Position; } }
		public long RemainingSize { get { return _stream.Length - _stream.Position; } }

		public MessageReader(byte[] buffer)
		{
			_stream = new System.IO.MemoryStream(buffer);
#if USE_MESSAGE_PACK
			_deserializer = new Colopl.Serialization.MsgPack.Deserializer (_stream);
#endif
		}

		public MessageReader(byte[] buffer, int index)
		{
			_stream = new System.IO.MemoryStream(buffer, index, buffer.Length);
#if USE_MESSAGE_PACK
			_deserializer = new Colopl.Serialization.MsgPack.Deserializer (_stream);
#endif
		}

		public byte ReadByte()
		{
			_stream.Read(_tmpBuffer, 0, sizeof(byte));
			return _tmpBuffer[0];
		}

		public short ReadInt16()
		{
#if USE_MESSAGE_PACK
			return (short)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(short));
			return ByteOrderConverter.ToInt16(_tmpBuffer);
#endif
		}
		public int ReadInt32()
		{
#if USE_MESSAGE_PACK
			return (int)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(int));
			return ByteOrderConverter.ToInt32(_tmpBuffer);
#endif
		}
		public long ReadInt64()
		{
#if USE_MESSAGE_PACK
			return (long)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(long));
			return ByteOrderConverter.ToInt64(_tmpBuffer);
#endif
		}
		public ushort ReadUInt16()
		{
#if USE_MESSAGE_PACK
			return (ushort)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(ushort));
			return ByteOrderConverter.ToUInt16(_tmpBuffer);
#endif
		}
		public uint ReadUInt32()
		{
#if USE_MESSAGE_PACK
			return (uint)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(uint));
			return ByteOrderConverter.ToUInt32(_tmpBuffer);
#endif
		}
		public ulong ReadUInt64()
		{
#if USE_MESSAGE_PACK
			return (ulong)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(ulong));
			return ByteOrderConverter.ToUInt64(_tmpBuffer);
#endif
		}
		public float ReadSingle()
		{
#if USE_MESSAGE_PACK
			return (float)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(float));
			return ByteOrderConverter.ToSingle(_tmpBuffer);
#endif
		}
		public double ReadDouble()
		{
#if USE_MESSAGE_PACK
			return (double)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(double));
			return ByteOrderConverter.ToDouble(_tmpBuffer);
#endif
		}

		public string ReadString()
		{
#if USE_MESSAGE_PACK
			return (string)_deserializer.Deserialize ();
#else
			ushort length = ReadUInt16();
			if (length > _tmpBuffer.Length)
			{
				_tmpBuffer = new byte[length];
			}
			_stream.Read(_tmpBuffer, 0, sizeof(byte) * length);
			return ByteOrderConverter.ToString(_tmpBuffer, length);
#endif
		}

		public string ReadString(int size)
		{
#if USE_MESSAGE_PACK
			return (string)_deserializer.Deserialize ();
#else
			if (size > _tmpBuffer.Length)
			{
				_tmpBuffer = new byte[size];
			}
			_stream.Read(_tmpBuffer, 0, sizeof(byte) * size);
			return ByteOrderConverter.ToString(_tmpBuffer, size);
#endif
		}

		public bool ReadBoolean()
		{
#if USE_MESSAGE_PACK
			return (bool)_deserializer.Deserialize ();
#else
			_stream.Read(_tmpBuffer, 0, sizeof(bool));
			return ByteOrderConverter.ToBoolean(_tmpBuffer);
#endif
		}

		public byte[] ReadBytes(int size)
		{
#if USE_MESSAGE_PACK
			return (byte[])_deserializer.Deserialize ();
#else
			if (size > _tmpBuffer.Length)
			{
				_tmpBuffer = new byte[size];
			}
			_stream.Read(_tmpBuffer, 0, sizeof(byte) * size);
			byte[] bytes = new byte[size];
			Array.Copy(ByteOrderConverter.ToBytes(_tmpBuffer, size), bytes, size);
			return bytes;
#endif
		}

		public object Read()
		{
#if USE_MESSAGE_PACK
			return _deserializer.Deserialize ();
#else
			throw new NotImplementedException();
#endif
		}

		public List<object> ReadArray()
		{
#if USE_MESSAGE_PACK
			return (List<object>)_deserializer.Deserialize ();
#else
			throw new NotImplementedException();
#endif
		}

		public void Dispose()
		{
#if USE_MESSAGE_PACk
			_deserializer.Dispose ();
			_deserializer = null;
#endif
			_stream.Dispose();
			_stream = null;
		}
	}
}

