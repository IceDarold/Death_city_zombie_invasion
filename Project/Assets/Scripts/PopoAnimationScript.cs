using System;
using UnityEngine;

public class PopoAnimationScript : MonoBehaviour
{
	private void Start()
	{
		this.alpha = base.GetComponent<Renderer>().material.GetFloat(this.alphaPropertyName);
		this.startTime = Time.time;
		this.rndUV = UnityEngine.Random.Range(0f, 1f);
		this.rndScale = UnityEngine.Random.Range(2, 5);
	}

	private void Update()
	{
		float num = (Time.time * this.scrollSpeed + this.rndUV) % 1f;
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
		this.currentScale += Time.deltaTime * this.scaleSpeed;
		this.currentScale = Mathf.Clamp(this.currentScale, 0.01f, 0.1f + 0.1f * (float)this.rndScale);
		base.transform.localScale = this.currentScale * new Vector3(1f, 1f, 1f);
	}

	public bool u = true;

	public bool v = true;

	public float scrollSpeed = 1f;

	public float scaleSpeed = 0.1f;

	public string alphaPropertyName = "_Alpha";

	public string texturePropertyName = "_MainTex";

	protected float rndUV;

	protected int rndScale;

	protected float alpha;

	protected float startTime;

	protected float currentScale;
}
