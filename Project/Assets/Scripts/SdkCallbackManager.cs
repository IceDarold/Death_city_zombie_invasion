/*using System;
using System.Collections.Generic;
using DataCenter;

public class SdkCallbackManager : SDKCallback
{
	public int GetGameLevel(bool isMax)
	{
		if (CheckpointDataManager.SelectCheckpoint == null)
		{
			return 0;
		}
		if (isMax)
		{
			return CheckpointDataManager.GetCurrentCheckpoint().ID;
		}
		return CheckpointDataManager.SelectCheckpoint.ID;
	}

	public int GetMaxScore()
	{
		return 0;
	}

	public string GetUserData()
	{
		return string.Empty;
	}

	public void JumpPage(string data)
	{
	}

	public void OnGotServerData(string data, List<GRewardItem> items)
	{
	}

	public void OnMailCountChanged(int count)
	{
	}

	public void ShowTips(string tip)
	{
	}

	public void UseUIMode(int modeId)
	{
	}
}
*/