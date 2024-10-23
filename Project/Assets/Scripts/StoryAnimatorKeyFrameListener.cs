using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StoryAnimatorKeyFrameListener : MonoBehaviour
{
	public void DoControlAnimator(int index)
	{
		this.aniInfo[index].animator.Play(this.aniInfo[index].aniName);
	}

	public List<AnimatorInfo> aniInfo = new List<AnimatorInfo>();
}
