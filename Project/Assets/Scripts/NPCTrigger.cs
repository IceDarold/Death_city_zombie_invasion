using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCTrigger : MonoBehaviour
{
	public void SetOnTriggerEnter(Action action)
	{
		this.onTriggerEnter = action;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (this.onTriggerEnter != null)
		{
			this.onTriggerEnter();
		}
		base.GetComponent<Collider>().enabled = false;
	}

	protected Action onTriggerEnter;
}
