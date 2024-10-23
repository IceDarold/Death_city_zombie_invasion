using System;
using UnityEngine;
using Zombie3D;

public class TurretInfrared : MonoBehaviour
{
	private void Start()
	{
		this.lRenderer = (base.gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer);
	}

	private void Update()
	{
		this.lRenderer.startWidth = 0.04f;
		this.lRenderer.endWidth = 0.04f;
		this.lRenderer.SetPosition(0, base.transform.position);
		Physics.Raycast(base.transform.position, base.transform.forward, out this.raycast);
		if (this.raycast.collider != null)
		{
			this.lRenderer.SetPosition(1, this.raycast.point);
		}
		else
		{
			this.Range = 200f;
			this.lRenderer.SetPosition(1, base.transform.forward * this.Range);
		}
	}

	public Enemy Target;

	public float Range;

	private LineRenderer lRenderer;

	private RaycastHit raycast;
}
