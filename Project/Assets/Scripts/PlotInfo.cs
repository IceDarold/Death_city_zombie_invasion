using System;

[Serializable]
public class PlotInfo
{
	public PlotInfo(string _plotName, int _actionIndex, ActionRefType _refType)
	{
		this.plotName = _plotName;
		this.actionIndex = _actionIndex;
		this.refType = _refType;
	}

	public string plotName;

	public int actionIndex;

	public ActionRefType refType;
}
