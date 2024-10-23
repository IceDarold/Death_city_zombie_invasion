using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectInfo
{
	public ObjectInfo(string _prefabPath, string _nameInScene, Vector3 _position, Vector3 _rotation, Vector3 _scale, bool _visible)
	{
		this.prefabPath = _prefabPath;
		this.position = _position;
		this.rotation = _rotation;
		this.scale = _scale;
		this.visible = _visible;
		this.nameInScene = _nameInScene;
	}

	public void SeekRefInGamePlot(GameObject self)
	{
		GamePlotManager component = GameObject.Find("GamePlots").GetComponent<GamePlotManager>();
		foreach (GamePlot gamePlot in component.Plots)
		{
			for (int i = 0; i < gamePlot.ActionList.Count; i++)
			{
				PlotAction plotAction = gamePlot.ActionList[i];
				if (plotAction.TargetObject == self)
				{
					this.refPlotAction.Add(new PlotInfo(gamePlot.gameObject.name, i, ActionRefType.GAMEOBJECT));
				}
				else if (plotAction.TargetPoint != null && plotAction.TargetPoint.gameObject == self)
				{
					this.refPlotAction.Add(new PlotInfo(gamePlot.gameObject.name, i, ActionRefType.TRANSFORM));
				}
				else if (plotAction.PlotAnimator != null && plotAction.PlotAnimator.gameObject == self)
				{
					this.refPlotAction.Add(new PlotInfo(gamePlot.gameObject.name, i, ActionRefType.ANIMATOR));
				}
				else if (plotAction.Particle != null && plotAction.Particle.gameObject == self)
				{
					this.refPlotAction.Add(new PlotInfo(gamePlot.gameObject.name, i, ActionRefType.PARTICLESYSTEM));
				}
			}
		}
	}

	public void SetPlotRef(GamePlotManager plotManager, GameObject obj)
	{
		int i;
		for (i = 0; i < this.refPlotAction.Count; i++)
		{
			GamePlot gamePlot = plotManager.Plots.Find((GamePlot temp) => temp.gameObject.name.Equals(this.refPlotAction[i].plotName));
			switch (this.refPlotAction[i].refType)
			{
			case ActionRefType.ANIMATOR:
				gamePlot.ActionList[this.refPlotAction[i].actionIndex].PlotAnimator = obj.GetComponent<Animator>();
				break;
			case ActionRefType.TRANSFORM:
				gamePlot.ActionList[this.refPlotAction[i].actionIndex].TargetPoint = obj.transform;
				break;
			case ActionRefType.GAMEOBJECT:
				gamePlot.ActionList[this.refPlotAction[i].actionIndex].TargetObject = obj;
				break;
			case ActionRefType.PARTICLESYSTEM:
				gamePlot.ActionList[this.refPlotAction[i].actionIndex].Particle = obj.GetComponent<ParticleSystem>();
				break;
			}
		}
	}

	public string prefabPath;

	public string nameInScene;

	public Vector3 position;

	public Vector3 rotation;

	public Vector3 scale;

	public bool visible;

	public List<PlotInfo> refPlotAction = new List<PlotInfo>();
}
