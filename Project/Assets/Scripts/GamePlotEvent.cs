using System;

public struct GamePlotEvent : IPlotEvent
{
	public void AddListener(BaseEventHandler call)
	{
		this.PlotEventHandler = (BaseEventHandler)Delegate.Combine(this.PlotEventHandler, call);
	}

	public void RemoveListener(BaseEventHandler call)
	{
		this.PlotEventHandler = (BaseEventHandler)Delegate.Remove(this.PlotEventHandler, call);
	}

	public void TriggerEvent(float t)
	{
		if (this.PlotEventHandler != null)
		{
			this.PlotEventHandler(t);
		}
	}

	private BaseEventHandler PlotEventHandler;
}
