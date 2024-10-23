using System;
using UnityEngine;

public class CameraFadeScript : MonoBehaviour
{
	private void Awake()
	{
		this.m_FadeTexture = new Texture2D(1, 1);
		this.m_BackgroundStyle.normal.background = this.m_FadeTexture;
		this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
	}

	private void OnGUI()
	{
		if (this.m_CurrentScreenOverlayColor != this.m_TargetScreenOverlayColor)
		{
			if (Mathf.Abs(this.m_CurrentScreenOverlayColor.a - this.m_TargetScreenOverlayColor.a) < Mathf.Abs(this.m_DeltaColor.a) * Time.deltaTime)
			{
				this.m_CurrentScreenOverlayColor = this.m_TargetScreenOverlayColor;
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
				this.m_DeltaColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor + this.m_DeltaColor * Time.deltaTime);
			}
		}
		if (this.m_CurrentScreenOverlayColor.a > 0f)
		{
			GUI.depth = this.m_FadeGUIDepth;
			GUI.Label(new Rect(-10f, -10f, (float)(Screen.width + 10), (float)(Screen.height + 10)), this.m_FadeTexture, this.m_BackgroundStyle);
		}
	}

	public float GetAlpha()
	{
		return this.m_CurrentScreenOverlayColor.a;
	}

	public void StartFade(Color start, Color end, float duration)
	{
		this.SetScreenOverlayColor(start);
		this.StartFade(end, duration);
	}

	public void FadeOutBlack()
	{
		this.StartFade(Color.black, new Color(0f, 0f, 0f, 0f), 1f);
	}

	public void FadeInBlack()
	{
		this.StartFade(new Color(0f, 0f, 0f, 0f), Color.black, 1f);
	}

	private void Start()
	{
		this.SetScreenOverlayColor(this.startColor);
		this.StartFade(this.endColor, this.duration);
	}

	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		this.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		this.m_FadeTexture.SetPixel(0, 0, this.m_CurrentScreenOverlayColor);
		this.m_FadeTexture.Apply();
	}

	public static CameraFadeScript GetInstance()
	{
		return GameObject.Find("CameraFade").GetComponent<CameraFadeScript>();
	}

	public void StartFade(Color newScreenOverlayColor, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			this.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			this.m_TargetScreenOverlayColor = newScreenOverlayColor;
			this.m_DeltaColor = (this.m_TargetScreenOverlayColor - this.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	private GUIStyle m_BackgroundStyle = new GUIStyle();

	private Texture2D m_FadeTexture;

	private Color m_CurrentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	private Color m_TargetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	private Color m_DeltaColor = new Color(0f, 0f, 0f, 0f);

	private int m_FadeGUIDepth = -1000;

	public Color startColor = Color.black;

	public Color endColor = new Color(0f, 0f, 0f, 0f);

	public float duration = 3f;
}
