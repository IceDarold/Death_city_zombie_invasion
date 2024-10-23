using System;
using UnityEngine;

public class Animation2DScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime > this.frameRate)
		{
			this.deltaTime = 0f;
			int num = this.textures.Length;
			this.currentIndex++;
			if (this.currentIndex >= this.textures.Length)
			{
				this.currentIndex = 0;
			}
			base.GetComponent<Renderer>().material.SetTexture(this.texturePropertyName, this.textures[this.currentIndex]);
		}
	}

	public float frameRate = 0.02f;

	protected int currentIndex;

	public Texture2D[] textures;

	public string texturePropertyName = "_MainTex";

	protected float deltaTime;
}
