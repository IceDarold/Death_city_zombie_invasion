using System;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	[ContextMenu("设置低端机")]
	private void TestLow()
	{
		QualityManager.isLow = !QualityManager.isLow;
	}

	private void Start()
	{
		if ((Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor && !this.OpenReleaseLog) || this.mustCloseLog)
		{
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Screen.sleepTimeout = int.MaxValue;
		Application.targetFrameRate = 60;
		int systemMemorySize = SystemInfo.systemMemorySize;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		float num = (float)Screen.width;
		if (systemMemorySize < 0)
		{
			if (num < 500f)
			{
				QualityManager.isLow = true;
				QualitySettings.SetQualityLevel(0);
			}
			else if (num > 1000f)
			{
				QualityManager.isLow = false;
				QualitySettings.SetQualityLevel(2);
			}
			else
			{
				QualityManager.isLow = true;
				QualitySettings.SetQualityLevel(1);
			}
		}
		else if (systemMemorySize < 1300)
		{
			QualityManager.isLow = true;
			QualitySettings.SetQualityLevel(0);
			Application.targetFrameRate = 40;
		}
		else if (systemMemorySize < 2100)
		{
			QualityManager.isLow = true;
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 50;
		}
		else
		{
			QualityManager.isLow = false;
			QualitySettings.SetQualityLevel(2);
			Application.targetFrameRate = 60;
		}
	}

	public Material crystalMat;

	public Material redCrystalMat;

	public Material riverMat;

	public static bool isLow;

	public bool OpenReleaseLog;

	public bool mustCloseLog;
}
