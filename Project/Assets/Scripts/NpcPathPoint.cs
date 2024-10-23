using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcPathPoint : MonoBehaviour
{
	public NPCSTATE arriveState;

	public int arriveDialogId = -1;

	public AutoEnemySpawnTrigger enemySpawnTrigger;

	public List<NPCSTATE> leaveState;

	public int leaveDialogId = -1;

	[CNName("关联的任务的ID")]
	public int relativeMissionID = -1;
}
