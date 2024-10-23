using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUIPop : MonoBehaviour
{
	private void Awake()
	{
		AchievementUIPop.instance = this;
	}

	private void Update()
	{
		if (this.showAchieve)
		{
			this.showDuration -= Time.deltaTime;
			if (this.showDuration <= 0f)
			{
				this.showDuration = 5f;
				this.showAchieve = false;
				this.PopStack();
			}
		}
	}

	public void AddItemToStack(AchievementGroup _item)
	{
		if (this.achieveUIPool != null)
		{
			this.achieveUIPool.Push(_item);
		}
		if (!this.showAchieve)
		{
			this.PopStack();
		}
	}

	public bool IsStackEmpty()
	{
		return this.achieveUIPool != null && this.achieveUIPool.Count <= 0;
	}

	public void PopStack()
	{
	}

	public void JudgeStackIsEmpty()
	{
		if (!this.IsStackEmpty())
		{
			this.showAchieve = true;
			this.achieveItem = this.achieveUIPool.Pop();
		}
	}

	public static AchievementUIPop instance;

	private Stack<AchievementGroup> achieveUIPool = new Stack<AchievementGroup>();

	private bool showAchieve;

	private float showDuration = 5f;

	private AchievementGroup achieveItem = new AchievementGroup();
}
