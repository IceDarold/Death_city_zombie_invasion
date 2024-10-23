using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class CannonCreater : SceneBindAble
{
	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		this.gameScene = GameApp.GetInstance().GetGameScene();
		bool canUse = false;
		canUse = base.CanUse(this.gameScene.SelectedLevel);
		if (canUse)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Player/Cannon"), base.transform.position, base.transform.rotation) as GameObject;
			this.cannon = gameObject.GetComponent<Cannon>();
			this.cannon.Init(this);
			this.cannon.SetRotateLimit(this.minXAxisValue, this.maxXAxisValue, this.minYAxisValue, this.maxYAxisValue);
			this.cannon.SetStartRotate(this.startX, this.startY);
		}
		yield break;
	}

	public Cannon GetCannon()
	{
		return this.cannon;
	}

	public const string path = "Prefabs/Player/";

	[CNName("立即切换")]
	public bool controllCannon;

	[Space]
	[CNName("怪物目标")]
	public NPCCreater npcCreater;

	[Header("角度偏转限制")]
	public float minXAxisValue;

	public float maxXAxisValue;

	public float minYAxisValue;

	public float maxYAxisValue;

	[Header("初始偏转角")]
	public float startX;

	public float startY;

	[HideInInspector]
	public Cannon cannon;

	protected GameScene gameScene;
}
