using System;
using System.Collections.Generic;

[Serializable]
public class NPCPathInfo
{
	public NPCSTATE startState;

	public int startPlotID = -1;

	public NPCTrigger startTrigger;

	public NPCSTATE moveState;

	public List<NPCPathPoint> pathPoint;

	[NonSerialized]
	public bool showInEditor;
}
