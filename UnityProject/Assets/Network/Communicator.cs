
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Network
{
	enum MessageType
	{
		Log = 1,
	}
	
	public class LogMessage : IMessage
	{
		public byte MessageId { get { return (int)MessageType.Log; } }
		public byte[] Data { get; set; }
		
		public void Write(MessageWriter writer)
		{
			writer.Write(Data.Length);
			writer.Write(Data, Data.Length);
		}

		public void Read(MessageReader reader)
		{
			var size = reader.ReadInt32();
			Data = reader.ReadBytes(size);
		}
	}

	public class Communicator : System.IDisposable
	{
		string _host;
		int _sendPort;
		int _recvPort;

		object _sendLock = new object();
		object _recvLock = new object();
		
		System.Threading.Thread _sendThread;
		System.Threading.Thread _recvThread;

		byte[] _sendBuffer = new byte[1024 * 1024];
		byte[] _recvBuffer = new byte[1024 * 1024];

		Queue<IMessage> _sendQueue = new Queue<IMessage>(1024);
		Queue<IMessage> _recvQueue = new Queue<IMessage>(1024);
		Dictionary<int, System.Type> _messages = new Dictionary<int, System.Type>();

		public void Send(IMessage message)
		{
			lock(_sendLock)
			{
				_sendQueue.Enqueue(message);
			}
		}

		public IMessage[] Recv()
		{
			if (!_recvQueue.Any())
			{
				return null;
			}

			lock (_recvLock)
			{
				var messages = _recvQueue.ToArray();

				_recvQueue.Clear();

				return messages;
			}
		}

		public void Setup(string hostname = "localhost", int sendPort = 5601, int recvPort = 5602)
		{
			_host = hostname;
			_sendPort = sendPort;
			_recvPort = recvPort;
			
			_sendThread = new System.Threading.Thread(SendThread);
			_sendThread.Start();

			_recvThread = new System.Threading.Thread(RecvThread);
			_recvThread.Start();
		}

		public void RegiserMessage<T>() where T : IMessage, new()
		{
			_messages.Add((new T()).MessageId, typeof(T));
		}

		public void UnregisterMessage<T>() where T : IMessage, new()
		{
			_messages.Remove((new T()).MessageId);
		}

		public void Dispose()
		{
			if(_sendThread != null)
			{
				_sendThread.Abort();
			}
			if (_recvThread != null)
			{
				_recvThread.Abort();
			}
		}

		void SendThread()
		{
			while (true)
			{
				try
				{
					using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
					{
						socket.Connect(_host, _sendPort);
						if (!socket.Connected)
						{
							System.Threading.Thread.Sleep(16);
							continue;
						}

						while(socket.Connected)
						{
							if(_sendQueue.Any())
							{
								using (var writer = new MessageWriter(_sendBuffer))
								{
									lock (_sendLock)
									{
										writer.Write(_sendQueue.Count());
										while (_sendQueue.Any())
										{
											var message = _sendQueue.Dequeue();
											writer.Write(message.MessageId);
											message.Write(writer);
										}
									}
									socket.Send(_sendBuffer, writer.WriteSize, SocketFlags.None);
								}
							}

							System.Threading.Thread.Sleep(16);
						}
					}
				}

				catch(System.Exception e)
				{
					UnityEngine.Debug.Log("Exception " + e.Message);
				}

				System.Threading.Thread.Sleep(16);
			}
		}

		void RecvThread()
		{
			while (true)
			{
				try
				{
					using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
					{
						socket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, _recvPort));

						socket.Listen(10);

						if (!socket.Connected)
						{
							System.Threading.Thread.Sleep(16);
							continue;
						}

						while (socket.Connected)
						{
							int size = socket.Receive(_recvBuffer);

							using (var reader = new MessageReader(_recvBuffer))
							{
								lock (_recvLock)
								{
									var count = reader.ReadInt32();
									for(int i = 0; i < count; ++i)
									{
										var messageId = reader.ReadByte();
										if(_messages.ContainsKey(messageId))
										{
											var type = _messages[messageId];
											var message = System.Activator.CreateInstance(type) as IMessage;
											message.Read(reader);
											_recvQueue.Enqueue(message);
										}
									}
								}
							}

							System.Threading.Thread.Sleep(16);
						}
					}
				}

				catch (System.Exception e)
				{
					UnityEngine.Debug.Log("Exception " + e.Message);
				}

				System.Threading.Thread.Sleep(16);
			}
		}
	}
}

