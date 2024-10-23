using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class ClipNearScript : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return 0;
		this.selfTrans = base.transform;
		this.cameraTrans = GameApp.GetInstance().GetGameScene().GetCamera().transform;
		this.init = true;
		yield break;
	}

	private void Update()
	{
		if (!this.init)
		{
			return;
		}
		if ((this.selfTrans.position - this.cameraTrans.position).sqrMagnitude < 25f)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = true;
		}
	}

	private Transform selfTrans;

	private Transform cameraTrans;

	private bool init;
}
