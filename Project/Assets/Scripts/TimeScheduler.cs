using System;

public class TimeScheduler
{
	public TimeScheduler(float _interval, float _counter, Action callback, Func<bool> _condition) : this(_interval, callback, _condition)
	{
		this.counter = _counter;
	}

	public TimeScheduler(float _interval, Action callback, Func<bool> _condition)
	{
		this.interval = _interval;
		this.counter = this.interval;
		this.cb = callback;
		this.condition = _condition;
	}

	private TimeScheduler()
	{
	}

	public void DoUpdate(float dt)
	{
		if (this.condition != null && !this.condition())
		{
			return;
		}
		this.counter -= dt;
		if (this.counter > 0f)
		{
			return;
		}
		this.counter = this.interval;
		if (this.cb != null)
		{
			this.cb();
		}
	}

	public void ResetCounter()
	{
		this.counter = this.interval;
	}

	protected float interval;

	protected float counter;

	private Action cb;

	private Func<bool> condition;
}
