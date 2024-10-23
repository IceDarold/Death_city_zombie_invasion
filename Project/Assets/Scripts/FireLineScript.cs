using System;
using UnityEngine;

public class FireLineScript : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.03f)
		{
			return;
		}
		base.transform.Translate(this.speed * (this.endPos - this.beginPos).normalized * this.deltaTime, Space.World);
		if ((base.transform.position - this.endPos).magnitude < 1f)
		{
			base.gameObject.SetActive(false);
		}
		this.deltaTime = 0f;
	}

	public Vector3 beginPos;

	public Vector3 endPos;

	public float speed;

	protected float startTime;

	protected float deltaTime;
}
