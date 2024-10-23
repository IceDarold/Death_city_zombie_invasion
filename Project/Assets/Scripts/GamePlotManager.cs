using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePlotManager : MonoBehaviour
{
	public GamePlot GetGamePlot(int id)
	{
		for (int i = 0; i < this.Plots.Count; i++)
		{
			if (this.Plots[i].ID == id)
			{
				return this.Plots[i];
			}
		}
		return null;
	}

	public void SetPlotEnableByLevel(int levelIndex)
	{
		int i = 0;
		int count = this.Plots.Count;
		while (i < count)
		{
			this.Plots[i].gameObject.SetActive(this.Plots[i].CanUse(levelIndex));
			i++;
		}
	}

	public void ShowPlot(int id, UnityAction callback = null)
	{
		if (id == -1)
		{
			return;
		}
		for (int i = 0; i < this.Plots.Count; i++)
		{
			if (this.Plots[i].ID == id)
			{
				this.Plots[i].DoPlot(callback);
				return;
			}
		}
		UnityEngine.Debug.Log("剧情ID不匹配");
	}

	public bool isPlot()
	{
		for (int i = 0; i < this.Plots.Count; i++)
		{
			if (this.Plots[i].isPlot)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanPause()
	{
		for (int i = 0; i < this.Plots.Count; i++)
		{
			if (this.Plots[i].isPlot)
			{
				for (int j = 0; j < this.Plots[i].ActionList.Count; j++)
				{
					if (this.Plots[i].ActionList[j].Type == PlotActionType.对话)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	[ContextMenu("查找对话")]
	private void Test()
	{
		foreach (GamePlot gamePlot in this.Plots)
		{
			foreach (PlotAction plotAction in gamePlot.ActionList)
			{
				if (plotAction.Type == PlotActionType.对话 && plotAction.DialogueTag == 5007)
				{
					UnityEngine.Debug.LogError(gamePlot.gameObject.name);
				}
			}
		}
	}

	public List<GamePlot> Plots = new List<GamePlot>();
}
