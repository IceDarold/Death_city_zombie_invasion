using System;
using System.Collections.Generic;
using Zombie3D;

public class GlobalMessage : Singleton<GlobalMessage>
{
	public void SendGlobalMessage(MessageType type)
	{
		for (int i = 0; i < this.allHandlers.Count; i++)
		{
			this.allHandlers[i].HandleMessage(type);
		}
	}

	public void RegisterMessageHandler(IMessageHandler handler)
	{
		this.allHandlers.Add(handler);
	}

	public void RemoveMessageHandler(IMessageHandler handler)
	{
		if (this.allHandlers.Contains(handler))
		{
			this.allHandlers.Remove(handler);
		}
	}

	public List<IMessageHandler> allHandlers = new List<IMessageHandler>();
}
