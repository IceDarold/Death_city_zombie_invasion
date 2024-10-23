using System;
using UnityEngine;
using UnityEngine.UI;

public class FrostedGlassEffect : MonoBehaviour
{
	private void Start()
	{
		this.mScreenx = Screen.width;
		this.mScreeny = Screen.height;
		this.RefreshBackground();
	}

	private void OnEnable()
	{
		this.RefreshBackground();
	}

	private void RefreshBackground()
	{
		if (this.mScreenx <= 0)
		{
			return;
		}
		RenderTexture renderTexture = new RenderTexture(this.mScreenx, this.mScreeny, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		Camera.main.targetTexture = renderTexture;
		Camera.main.Render();
		RenderTexture.active = renderTexture;
		Texture2D texture2D = new Texture2D(this.mScreenx, this.mScreeny, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)this.mScreenx, (float)this.mScreeny), 0, 0, false);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.Apply();
		Camera.main.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)this.mScreenx, (float)this.mScreeny), Vector2.zero);
		base.GetComponent<Image>().sprite = sprite;
	}

	public Camera camera;

	private int mScreenx;

	private int mScreeny;
}
