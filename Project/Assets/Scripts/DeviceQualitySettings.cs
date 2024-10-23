using System;
using UnityEngine;

public class DeviceQualitySettings : MonoBehaviour
{
	private void Start()
	{
		global::QualityLevel qualityLevel = (global::QualityLevel)PlayerPrefs.GetInt("QualityLevelKey", -1);
		if (qualityLevel != global::QualityLevel.NONE)
		{
			this.OnQualityLevelChange(qualityLevel);
		}
		else
		{
			qualityLevel = DeviceQualitySettings.GetAndroidQualityLevel();
			PlayerPrefs.SetInt("QualityLevelKey", (int)qualityLevel);
			PlayerPrefs.Save();
		}
		this.OnQualityLevelChange(qualityLevel);
	}

	public static void ReduceQuality()
	{
		int num = PlayerPrefs.GetInt("QualityLevelKey", -1);
		if (num == 0)
		{
			return;
		}
		num--;
		PlayerPrefs.SetInt("QualityLevelKey", num);
		PlayerPrefs.SetInt("QualityDenied", 0);
		PlayerPrefs.Save();
		QualitySettings.SetQualityLevel(num);
		UnityEngine.Debug.LogError("Reduce Quality To : " + (UnityEngine.QualityLevel)num);
	}

	private void OnQualityLevelChange(global::QualityLevel newLevel)
	{
		QualitySettings.SetQualityLevel((int)newLevel);
		UnityEngine.Debug.LogError("QualityLevel Changed : " + (UnityEngine.QualityLevel)newLevel);
	}

	public static global::QualityLevel GetiOSQualityLevel()
	{
		string deviceModel = SystemInfo.deviceModel;
		string[] array = deviceModel.Split(new char[]
		{
			','
		});
		if (deviceModel.Contains("iPhone"))
		{
			string value = array[0].Replace("iPhone", string.Empty);
			int num = Convert.ToInt32(value);
			if (num <= 6)
			{
				return global::QualityLevel.FAST;
			}
			if (num <= 8)
			{
				return global::QualityLevel.SIMPLE;
			}
			return global::QualityLevel.GOOD;
		}
		else if (deviceModel.Contains("iPad"))
		{
			string value2 = array[0].Replace("iPhone", string.Empty);
			int num2 = Convert.ToInt32(value2);
			if (num2 <= 4)
			{
				return global::QualityLevel.FAST;
			}
			if (num2 <= 5)
			{
				return global::QualityLevel.SIMPLE;
			}
			return global::QualityLevel.GOOD;
		}
		else
		{
			if (deviceModel.Contains("iPod"))
			{
				return global::QualityLevel.FAST;
			}
			return global::QualityLevel.GOOD;
		}
	}

	public static global::QualityLevel GetAndroidQualityLevel()
	{
		global::QualityLevel result = DeviceQualitySettings.checkGPU_Android(SystemInfo.graphicsDeviceName);
		DeviceQualitySettings.checkDevice_Android(ref result);
		return result;
	}

	private static global::QualityLevel checkGPU_Android(string gpuName)
	{
		global::QualityLevel result = global::QualityLevel.FAST;
		if (SystemInfo.systemMemorySize < DeviceQualitySettings.androidMemoryLowLimit)
		{
			return global::QualityLevel.FAST;
		}
		gpuName = gpuName.ToLower();
		char[] separator = new char[]
		{
			' ',
			'\t',
			'\r',
			'\n',
			'+',
			'-',
			':',
			'\0'
		};
		string[] array = gpuName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		if (array == null || array.Length == 0)
		{
			return global::QualityLevel.FAST;
		}
		if (array[0].Contains("vivante"))
		{
			return global::QualityLevel.FAST;
		}
		if (array[0] == "adreno")
		{
			return DeviceQualitySettings.checkGPU_Adreno(array);
		}
		if (array[0] == "powervr" || array[0] == "imagination" || array[0] == "sgx")
		{
			return DeviceQualitySettings.checkGPU_PowerVR(array);
		}
		if (array[0] == "arm" || array[0] == "mali" || (array.Length > 1 && array[1] == "mali"))
		{
			return DeviceQualitySettings.checkGPU_Mali(array);
		}
		if (!(array[0] == "tegra") && !(array[0] == "nvidia"))
		{
			return result;
		}
		return DeviceQualitySettings.checkGPU_Tegra(array);
	}

	private static void checkDevice_Android(ref global::QualityLevel q)
	{
		UnityEngine.Debug.LogError(SystemInfo.deviceModel);
		string text = SystemInfo.deviceModel.ToLower();
		switch (text)
		{
		case "samsung gt-s7568i":
			q = global::QualityLevel.FAST;
			break;
		case "xiaomi 1s":
			q = global::QualityLevel.SIMPLE;
			break;
		case "xiaomi 2013022":
			q = global::QualityLevel.SIMPLE;
			break;
		case "samsung sch-i959":
			q = global::QualityLevel.SIMPLE;
			break;
		case "xiaomi mi 3":
			q = global::QualityLevel.GOOD;
			break;
		case "xiaomi mi 2a":
			q = global::QualityLevel.SIMPLE;
			break;
		case "xiaomi hm 1sc":
			q = global::QualityLevel.FAST;
			break;
		}
	}

	private static global::QualityLevel checkGPU_Adreno(string[] tokens)
	{
		int num = 0;
		for (int i = 1; i < tokens.Length; i++)
		{
			if (DeviceQualitySettings.TryGetInt(ref num, tokens[i]))
			{
				if (num < 200)
				{
					return global::QualityLevel.FAST;
				}
				if (num < 300)
				{
					if (num > 220)
					{
						return global::QualityLevel.FAST;
					}
					return global::QualityLevel.FAST;
				}
				else if (num < 400)
				{
					if (num >= 330)
					{
						return global::QualityLevel.GOOD;
					}
					if (num >= 320)
					{
						return global::QualityLevel.SIMPLE;
					}
					return global::QualityLevel.FAST;
				}
				else if (num >= 400)
				{
					if (num < 420)
					{
						return global::QualityLevel.SIMPLE;
					}
					return global::QualityLevel.GOOD;
				}
			}
		}
		return global::QualityLevel.FAST;
	}

	private static global::QualityLevel checkGPU_PowerVR(string[] tokens)
	{
		bool flag = false;
		bool flag2 = false;
		global::QualityLevel result = global::QualityLevel.FAST;
		int num = 0;
		int i = 1;
		while (i < tokens.Length)
		{
			string text = tokens[i];
			if (text == null)
			{
				goto IL_52;
			}
			if (!(text == "sgx"))
			{
				if (!(text == "rogue"))
				{
					goto IL_52;
				}
				flag2 = true;
				break;
			}
			else
			{
				flag = true;
			}
			IL_1E8:
			i++;
			continue;
			IL_52:
			if (!flag)
			{
				if (text.Length <= 4)
				{
					goto IL_1E8;
				}
				char c = text[0];
				char c2 = text[1];
				if (c != 'g')
				{
					goto IL_1E8;
				}
				if (c2 >= '0' && c2 <= '9')
				{
					DeviceQualitySettings.TryGetInt(ref num, text.Substring(1));
				}
				else
				{
					DeviceQualitySettings.TryGetInt(ref num, text.Substring(2));
				}
				if (num > 0)
				{
					if (num >= 7000)
					{
						result = global::QualityLevel.GOOD;
					}
					else if (num >= 6000)
					{
						if (num < 6100)
						{
							result = global::QualityLevel.FAST;
						}
						else if (num < 6400)
						{
							result = global::QualityLevel.SIMPLE;
						}
						else
						{
							result = global::QualityLevel.GOOD;
						}
					}
					else
					{
						result = global::QualityLevel.FAST;
					}
					break;
				}
				goto IL_1E8;
			}
			else
			{
				bool flag3 = false;
				int num2 = text.IndexOf("mp");
				if (num2 > 0)
				{
					DeviceQualitySettings.TryGetInt(ref num, text.Substring(0, num2));
					flag3 = true;
				}
				else if (DeviceQualitySettings.TryGetInt(ref num, text))
				{
					for (int j = i + 1; j < tokens.Length; j++)
					{
						if (tokens[j].ToLower().IndexOf("mp") >= 0)
						{
							flag3 = true;
							break;
						}
					}
				}
				if (num <= 0)
				{
					goto IL_1E8;
				}
				if (num < 543)
				{
					result = global::QualityLevel.FAST;
				}
				else if (num == 543)
				{
					result = global::QualityLevel.FAST;
				}
				else if (num == 544)
				{
					result = global::QualityLevel.FAST;
					if (flag3)
					{
						result = global::QualityLevel.SIMPLE;
					}
				}
				else
				{
					result = global::QualityLevel.SIMPLE;
				}
				break;
			}
		}
		if (flag2)
		{
			result = global::QualityLevel.GOOD;
		}
		return result;
	}

	private static global::QualityLevel checkGPU_Mali(string[] tokens)
	{
		int num = 0;
		global::QualityLevel result = global::QualityLevel.FAST;
		for (int i = 1; i < tokens.Length; i++)
		{
			string text = tokens[i];
			if (text.Length >= 3)
			{
				int num2 = text.LastIndexOf("mp");
				bool flag = text[0] == 't';
				if (num2 > 0)
				{
					int num3 = flag ? 1 : 0;
					text = text.Substring(num3, num2 - num3);
					DeviceQualitySettings.TryGetInt(ref num, text);
				}
				else
				{
					if (flag)
					{
						text = text.Substring(1);
					}
					if (DeviceQualitySettings.TryGetInt(ref num, text))
					{
						for (int j = i + 1; j < tokens.Length; j++)
						{
							text = tokens[j];
							if (text.IndexOf("mp") >= 0)
							{
								break;
							}
						}
					}
				}
				if (num > 0)
				{
					if (num < 400)
					{
						return global::QualityLevel.FAST;
					}
					if (num < 500)
					{
						if (num != 400 && num == 450)
						{
							return global::QualityLevel.SIMPLE;
						}
						return global::QualityLevel.FAST;
					}
					else if (num < 700)
					{
						if (!flag)
						{
							return global::QualityLevel.FAST;
						}
						if (num < 620)
						{
							return global::QualityLevel.FAST;
						}
						if (num < 628)
						{
							return global::QualityLevel.SIMPLE;
						}
						return global::QualityLevel.GOOD;
					}
					else
					{
						if (!flag)
						{
							return global::QualityLevel.FAST;
						}
						return global::QualityLevel.GOOD;
					}
				}
			}
		}
		return result;
	}

	private static global::QualityLevel checkGPU_Tegra(string[] tokens)
	{
		bool flag = false;
		int num = 0;
		global::QualityLevel result = global::QualityLevel.FAST;
		for (int i = 1; i < tokens.Length; i++)
		{
			if (DeviceQualitySettings.TryGetInt(ref num, tokens[i]))
			{
				flag = true;
				if (num >= 4)
				{
					result = global::QualityLevel.GOOD;
				}
				else
				{
					if (num != 3)
					{
						goto IL_5E;
					}
					result = global::QualityLevel.SIMPLE;
				}
				break;
			}
			string a = tokens[i];
			if (a == "k1")
			{
				result = global::QualityLevel.GOOD;
				flag = true;
				break;
			}
			IL_5E:;
		}
		if (!flag)
		{
			result = global::QualityLevel.SIMPLE;
		}
		return result;
	}

	private static bool TryGetInt(ref int val, string str)
	{
		val = 0;
		bool result;
		try
		{
			val = Convert.ToInt32(str);
			result = true;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public global::QualityLevel level;

	public static int androidMemoryLowLimit = 1500;

	public const string QUALITY = "QualityLevelKey";

	public const string QUALITY_DENIED = "QualityDenied";
}
