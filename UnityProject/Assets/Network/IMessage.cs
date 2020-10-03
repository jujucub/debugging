using System;

namespace Network
{
	public interface IMessage
	{
		byte MessageId { get; }
		void Write(MessageWriter writer);
		void Read(MessageReader reader);
	}
}
