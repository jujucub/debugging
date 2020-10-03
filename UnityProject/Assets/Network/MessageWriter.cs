//#define USE_MESSAGE_PACk
using System;
using System.Collections.Generic;

namespace Network
{
	public sealed class MessageWriter : IDisposable
	{
		byte[] _tmpBuffer = new byte[sizeof(byte)];
		System.IO.MemoryStream _stream;
#if USE_MESSAGE_PACk
			Colopl.Serialization.MsgPack.Serializer _serializer;
#endif
		public int WriteSize { get { return (int)_stream.Position; } }

		public MessageWriter(byte[] buffer)
		{
			_stream = new System.IO.MemoryStream(buffer);
#if USE_MESSAGE_PACk
				_serializer = new Colopl.Serialization.MsgPack.Serializer (_stream);
#endif
		}

		public MessageWriter(int size)
		{
			_stream = new System.IO.MemoryStream(size);
#if USE_MESSAGE_PACk
				_serializer = new Colopl.Serialization.MsgPack.Serializer (_stream);
#endif
		}

		public void Write(byte value)
		{
			_tmpBuffer[0] = value;
			_stream.Write(_tmpBuffer, 0, sizeof(byte));
		}

		public void Write(byte[] value, int size)
		{
#if USE_MESSAGE_PACk
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value, size);
			_stream.Write(bytes, 0, size);
#endif
		}

		public void Write(short value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(int value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(long value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(ushort value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(uint value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(ulong value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(float value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(double value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(string value, int size)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			if (size > _tmpBuffer.Length)
			{
				_tmpBuffer = new byte[size];
			}
			bytes.CopyTo(_tmpBuffer, 0);
			_stream.Write(_tmpBuffer, 0, sizeof(byte) * size);
#endif
		}
		public void Write(string value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			Write((ushort)bytes.Length);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}
		public void Write(bool value)
		{
#if USE_MESSAGE_PACK
				_serializer.Serialize (value);
#else
			byte[] bytes = ByteOrderConverter.GetBytes(value);
			_stream.Write(bytes, 0, bytes.Length);
#endif
		}

		public byte[] GetBuffer()
		{
			return _stream.GetBuffer();
		}

		public byte[] ToArray()
		{
			return _stream.ToArray();
		}

		public void Dispose()
		{
			_stream.Dispose();
			_stream = null;
#if USE_MESSAGE_PACK
				_serializer.Dispose ();
				_serializer = null;
#endif
		}
	}
}