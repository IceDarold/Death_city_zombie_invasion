using System;
using UnityEngine;

public class UVAnimationScript : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	private void Update()
	{
		float num = Time.time * this.scrollSpeed % 1f;
		if (this.u && this.v)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(this.texturePropertyName, new Vector2(num, num));
		}
		else if (this.u)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(this.texturePropertyName, new Vector2(num, 0f));
		}
		else if (this.v)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(this.texturePropertyName, new Vector2(0f, num));
		}
	}

	public bool u = true;

	public bool v = true;

	public float scrollSpeed = 1f;

	protected float alpha;

	protected float startTime;

	public string alphaPropertyName = "_Alpha";

	public string texturePropertyName = "_MainTex";
}
