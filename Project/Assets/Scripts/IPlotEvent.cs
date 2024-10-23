using System;

public interface IPlotEvent
{
	void AddListener(BaseEventHandler call);

	void RemoveListener(BaseEventHandler call);

	void TriggerEvent(float t);
}
