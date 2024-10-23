using System;
using UnityEngine;

public class NPCCreater : SceneBindAble
{
	public override void DoActive(int levelIndex)
	{
		this.canUse = base.CanUse(levelIndex);
		if (this.canUse && base.gameObject.activeSelf)
		{
			this.hasCreated = true;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("MissionItems/" + this.npcPrefabName), base.transform.position, base.transform.rotation) as GameObject;
			this.npc = gameObject.GetComponent<SceneNPC>();
			this.npc.Init(this);
		}
	}

	private void OnEnable()
	{
		if (!this.hasCreated && this.canUse)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("MissionItems/" + this.npcPrefabName), base.transform.position, base.transform.rotation) as GameObject;
			this.npc = gameObject.GetComponent<SceneNPC>();
			this.npc.Init(this);
		}
	}

	public SceneNPC GetNpc()
	{
		return this.npc;
	}

	[CNName("起始状态")]
	public NPCSTATE startState;

	[HideInInspector]
	public int startPlotID = -1;

	[CNName("触发碰撞")]
	public NPCTrigger startTrigger;

	[CNName("移动状态")]
	public NPCSTATE moveState;

	[HideInInspector]
	public string npcPrefabName;

	protected SceneNPC npc;

	private const string path = "MissionItems/";

	protected bool canUse;

	protected bool hasCreated;
}
