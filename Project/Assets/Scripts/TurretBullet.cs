using System;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
	public void Activate(Vector3 pos)
	{
		float num = Vector3.Distance(base.transform.position, pos);
		this.dt = num / this.Speed;
		this.isActivate = true;
	}

	private void Update()
	{
		if (this.isActivate)
		{
			base.transform.Translate(Vector3.forward * this.Speed * Time.deltaTime, Space.Self);
			if (this.dt > 0f)
			{
				this.dt -= Time.deltaTime;
			}
			else
			{
				this.isActivate = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	public float Speed;

	private bool isActivate;

	private float dt;
}
