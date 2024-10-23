using System;
using UnityEngine;

public class RotateByTimeScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.03f)
		{
			return;
		}
		base.transform.Rotate(this.rotateSpeed * this.deltaTime, Space.Self);
		this.deltaTime = 0f;
	}

	public Vector3 rotateSpeed = new Vector3(0f, 45f, 0f);

	protected float deltaTime;
}
