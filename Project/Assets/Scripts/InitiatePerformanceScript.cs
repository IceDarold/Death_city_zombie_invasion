using System;
using UnityEngine;

public class InitiatePerformanceScript : MonoBehaviour
{
	private void Start()
	{
		this.rConfig = GameObject.Find("ResourceConfig").GetComponent<ResourceConfigScript>();
		this.timer.SetTimer(1f, false);
	}

	public ResourceConfigScript rConfig;

	protected Timer timer = new Timer();
}
