using System;
using System.Diagnostics;

public class DynamicData : Singleton<DynamicData>
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<int> OnHeadShotNumChanged;

	public int KillNum
	{
		get
		{
			return this.killNum;
		}
	}

	public int HeadShotNum
	{
		get
		{
			return this.headShotNum;
		}
	}

	public int KillBoss
	{
		get
		{
			return this.killBoss;
		}
	}

	public void DoKillBoss()
	{
		this.killBoss++;
		if (this.bossKillListener != null)
		{
			this.bossKillListener(this.killBoss);
		}
	}

	public void DoHeadShot()
	{
		this.headShotNum++;
		this.totalHeadShotNum++;
		if (this.OnHeadShotNumChanged != null && this.headShotNum != 0)
		{
			this.OnHeadShotNumChanged(this.headShotNum);
		}
		if (this.headShotNumListener != null)
		{
			this.headShotNumListener(this.headShotNum);
		}
		Singleton<DailyTaskMgr>.Instance.AddDailyTaskNum(DailyTask.HEADSHOT, 1, false);
	}

	public void DoKillEnemy()
	{
		this.killNum++;
		this.totalKillNum++;
		if (this.killNumListener != null)
		{
			this.killNumListener(this.killNum);
		}
		Singleton<DailyTaskMgr>.Instance.AddDailyTaskNum(DailyTask.KILL_ZOMBIE, 1, false);
	}

	public void ClearLevelData(bool clearReviveData = true, bool isReset = false)
	{
		this.killNum = 0;
		this.headShotNum = 0;
		if (clearReviveData)
		{
			this.reviveNum = 0;
		}
		if (!isReset)
		{
			this.OnHeadShotNumChanged = null;
		}
	}

	public bool isInit;

	public int SelectLevel = 1;

	public int playerEquiGunIndex;

	public int totalKillNum;

	public int totalHeadShotNum;

	public bool teachFinished = true;

	public int reviveNum;

	public float levelStartTime;

	protected int headShotNum;

	protected int killNum;

	protected int killBoss;

	public Action<int> killNumListener;

	public Action<int> headShotNumListener;

	public Action<int> bossKillListener;

	public string targetScene;
}
