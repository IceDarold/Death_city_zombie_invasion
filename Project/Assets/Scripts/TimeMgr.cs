using System;
using UnityEngine;

public class TimeMgr : Singleton<TimeMgr>
{
	public float RewardModeColdDown { get; set; }

	public float LotteryColdDown { get; set; }

	public int LotteryTimes
	{
		get
		{
			return this.lotteryTimes;
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnRefreshLotteryTimes;

	public new void Init()
	{
		this.createTime = Time.realtimeSinceStartup;
		this.InitColdDownData();
		this.preFrameRealtime = Time.realtimeSinceStartup;
	}

	private void InitColdDownData()
	{
		this.LoadLotteryState();
		this.lotteryTimes = PlayerPrefs.GetInt("lotterytimes", 3);
		this.RewardModeColdDown = Mathf.Clamp(180f - this.GetTotalSecondsUtcNow("rewardtimespan"), 0f, 180f);
		float totalSecondsUtcNow = this.GetTotalSecondsUtcNow("lotterytimespan");
		this.LotteryColdDown = PlayerPrefs.GetFloat("lotteryColdDown", 0f);
		this.LotteryColdDown = Mathf.Clamp(this.LotteryColdDown - totalSecondsUtcNow, 0f, this.LotteryColdDown);
		this.AddLotteryTime((int)totalSecondsUtcNow / 120);
	}

	public void SaveTimeRecorder(string key)
	{
		PlayerPrefs.SetFloat(key, (float)(DateTime.UtcNow - new DateTime(2017, 12, 1, 0, 0, 0, 0)).TotalSeconds);
		PlayerPrefs.Save();
		this.InitColdDownData();
		if (this.OnTimeTick != null)
		{
			this.OnTimeTick();
		}
	}

	private float GetTotalSecondsUtcNow(string key)
	{
		TimeSpan timeSpan = DateTime.UtcNow - new DateTime(2017, 12, 1, 0, 0, 0, 0);
		float @float = PlayerPrefs.GetFloat(key, 0f);
		return (float)timeSpan.TotalSeconds - @float;
	}

	private void SaveLotteryState()
	{
		string value = string.Concat(new string[]
		{
			this.lotteryState[0].ToString(),
			"_",
			this.lotteryState[1].ToString(),
			"_",
			this.lotteryState[2].ToString()
		});
		PlayerPrefs.SetString("lotterystate", value);
	}

	private void LoadLotteryState()
	{
		string[] array = PlayerPrefs.GetString("lotterystate", "0_0_0").Split(new char[]
		{
			'_'
		});
		for (int i = 0; i < this.lotteryState.Length; i++)
		{
			this.lotteryState[i] = int.Parse(array[i]);
		}
	}

	public int GetLotteryState(int index)
	{
		return this.lotteryState[index];
	}

	private void AddLotteryTime(int times = 1)
	{
		for (int i = 0; i < times; i++)
		{
			this.lotteryState[this.GetMaxLotteryIndex()] = 0;
		}
	}

	private int GetMaxLotteryIndex()
	{
		int num = 0;
		for (int i = 1; i < this.lotteryState.Length; i++)
		{
			if (this.lotteryState[num] < this.lotteryState[i])
			{
				num = i;
			}
		}
		return num;
	}

	public void DoLottery(int index)
	{
		for (int i = 0; i < this.lotteryState.Length; i++)
		{
			if (i == index)
			{
				this.lotteryState[i]++;
			}
			else if (this.lotteryState[i] != 0)
			{
				this.lotteryState[i]++;
			}
		}
		this.SaveLotteryState();
		this.lotteryTimes--;
		this.LotteryColdDown += 120f;
		PlayerPrefs.SetFloat("lotteryColdDown", this.LotteryColdDown);
		PlayerPrefs.SetInt("lotterytimes", this.lotteryTimes);
		PlayerPrefs.Save();
		this.SaveTimeRecorder("lotterytimespan");
		if (this.OnTimeTick != null)
		{
			this.OnTimeTick();
		}
		if (this.OnRefreshLotteryTimes != null)
		{
			this.OnRefreshLotteryTimes();
		}
	}

	public void Update()
	{
		this.timePassed = Time.realtimeSinceStartup - this.createTime;
		float num = Time.realtimeSinceStartup - this.preFrameRealtime;
		this.RewardModeColdDown = Mathf.Clamp(this.RewardModeColdDown - num, 0f, this.RewardModeColdDown);
		if (this.lotteryTimes < 3)
		{
			this.LotteryColdDown = Mathf.Clamp(this.LotteryColdDown - num, 0f, this.LotteryColdDown);
			if (this.LotteryColdDown - (float)(3 - this.lotteryTimes - 1) * 120f <= 0f)
			{
				this.lotteryTimes++;
				this.AddLotteryTime(1);
			}
		}
		if (this.timePassed >= 1f)
		{
			this.timePassed -= 1f;
			this.createTime = Time.realtimeSinceStartup;
			if (this.OnTimeTick != null)
			{
				this.OnTimeTick();
			}
		}
		this.preFrameRealtime = Time.realtimeSinceStartup;
	}

	public void RegisterTimeTick(Action callback)
	{
		this.OnTimeTick = (Action)Delegate.Combine(this.OnTimeTick, callback);
		callback();
	}

	public void RemoveTimeTick(Action callback)
	{
		this.OnTimeTick = (Action)Delegate.Remove(this.OnTimeTick, callback);
	}

	public const float RewardModeInterval = 180f;

	public const float LotteryInterval = 120f;

	public const int maxLotteryTimes = 3;

	protected float createTime;

	protected float timePassed;

	protected float preFrameRealtime;

	protected int lotteryTimes;

	protected Action OnTimeTick;

	protected int[] lotteryState = new int[3];
}
