using System;
using UnityEngine;

public class ScreenBloodScript : MonoBehaviour
{
	private void Start()
	{
		this.alpha = base.GetComponent<Renderer>().material.GetFloat(this.alphaPropertyName);
		this.startTime = Time.time;
	}

	public void NewBlood(float damage)
	{
		base.GetComponent<Renderer>().enabled = true;
		this.alpha = damage;
		this.alpha = Mathf.Clamp(this.alpha, 0f, 1f);
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.03f)
		{
			return;
		}
		this.alpha -= 0.5f * this.deltaTime;
		if (this.alpha <= 0f)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		base.GetComponent<Renderer>().material.SetFloat(this.alphaPropertyName, this.alpha);
		this.deltaTime = 0f;
	}

	public float scrollSpeed = 1f;

	protected float alpha;

	protected float startTime;

	protected float deltaTime;

	public string alphaPropertyName = "_Alpha";
}
