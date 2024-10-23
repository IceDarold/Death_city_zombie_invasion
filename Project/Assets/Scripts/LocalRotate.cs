using System;
using UnityEngine;

public class LocalRotate : MonoBehaviour
{
	private void Update()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.transform.localEulerAngles += new Vector3(0f, this.Speed * Time.deltaTime, 0f);
		}
	}

	public float Speed;
}
