using System;
using UnityEngine;

[Serializable]
public class NPCPathPoint
{
	public Transform trans;

	public NPCSTATE arriveState = NPCSTATE.NONE;

	public int arrivePlot = -1;

	public NPCSTATE moveAction = NPCSTATE.NONE;

	public int leavePlot = -1;

	public NPCTrigger activeCollider;

	public bool autoActive;

	public bool submitMission;
}
