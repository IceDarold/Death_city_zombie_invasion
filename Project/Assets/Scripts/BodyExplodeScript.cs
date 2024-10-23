using System;
using UnityEngine;

public class BodyExplodeScript : MonoBehaviour
{
	private void Start()
	{
		this.trans = base.GetComponentsInChildren<Transform>();
		foreach (Transform transform in this.trans)
		{
			UnityEngine.Debug.Log(transform.name);
		}
		for (int j = 0; j < 7; j++)
		{
			base.transform.rotation = Quaternion.AngleAxis(51.42857f, Vector3.up) * base.transform.rotation;
			this.dir[j] = base.transform.forward;
		}
	}

	private void Update()
	{
		for (int i = 0; i < 7; i++)
		{
			this.trans[i].Translate(this.dir[i] * Time.deltaTime, Space.World);
		}
	}

	protected Vector3[] dir = new Vector3[7];

	protected Transform[] trans;
}
