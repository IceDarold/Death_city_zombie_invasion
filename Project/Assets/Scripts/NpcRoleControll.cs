using System;
using UnityEngine;

public class NpcRoleControll : MonoBehaviour
{
	public void Awake()
	{
		this.animator.Play(0);
	}

	public void SetAni(int num)
	{
		this.animator.Play(this.AniName[num]);
	}

	public void Update()
	{
		if (this.canLook)
		{
		}
	}

	public const string ChangeAni = "Show";

	public bool canLook;

	private string[] AniName = new string[]
	{
		"Idle",
		"Show"
	};

	public Animator animator;

	public bool canBack;
}
