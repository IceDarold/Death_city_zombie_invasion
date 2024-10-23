using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderSwitcher : MonoBehaviour
{
	public void Awake()
	{
		base.gameObject.layer = 31;
		base.GetComponent<Collider>().isTrigger = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}
		foreach (Collider collider in this.allColliders)
		{
			collider.enabled = true;
		}
		base.GetComponent<Collider>().enabled = false;
	}

	public Collider[] allColliders;
}
