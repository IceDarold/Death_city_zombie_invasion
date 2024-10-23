using System;
using UnityEngine;

namespace DataCenter
{
	public class GameTimeManager
	{
		public static string getCurrentTime(CurrentTime ct)
		{
			string text = DateTime.Now.ToString("yy,MM,dd,HH,mm,ss,ff");
			string[] array = text.Split(new char[]
			{
				','
			});
			switch (ct)
			{
			case CurrentTime.Year:
				return array[0];
			case CurrentTime.Month:
				return array[1];
			case CurrentTime.Day:
				return array[2];
			case CurrentTime.Hour:
				return array[3];
			case CurrentTime.Minute:
				return array[4];
			case CurrentTime.Second:
				return array[5];
			case CurrentTime.Millisecond:
				return array[6];
			case CurrentTime.Date:
				return string.Concat(new string[]
				{
					array[0],
					".",
					array[1],
					".",
					array[2]
				});
			case CurrentTime.Time:
				return string.Concat(new string[]
				{
					array[3],
					".",
					array[4],
					".",
					array[5]
				});
			case CurrentTime.All:
				return string.Concat(new string[]
				{
					array[0],
					".",
					array[1],
					".",
					array[2],
					".",
					array[3],
					".",
					array[4],
					".",
					array[5]
				});
			default:
				return null;
			}
		}

		public static int getTodayOfYear()
		{
			return DateTime.Now.DayOfYear;
		}

		public static int ConvertToSceond(string time)
		{
			string[] array = time.Split(new char[]
			{
				'.'
			});
			if (array.Length > 3)
			{
				return 0;
			}
			int result;
			if (array.Length == 3)
			{
				result = int.Parse(array[0]) * 3600 + int.Parse(array[1]) * 60 + int.Parse(array[2]);
			}
			else if (array.Length == 2)
			{
				result = int.Parse(array[0]) * 60 + int.Parse(array[1]);
			}
			else
			{
				result = int.Parse(array[0]);
			}
			return result;
		}

		public static string ConvertToString(int seconds)
		{
			int num = seconds / 3600;
			int num2 = seconds % 3600 / 60;
			int num3 = seconds % 3600 % 60;
			return string.Concat(new string[]
			{
				num.ToString("D2"),
				":",
				num2.ToString("D2"),
				":",
				num3.ToString("D2")
			});
		}

		public static int GetDifftime(string startTime, string endTime)
		{
			int num = GameTimeManager.ConvertToSceond(endTime) - GameTimeManager.ConvertToSceond(startTime);
			if (num > 0)
			{
				return num;
			}
			return -num;
		}

		public static void RecordTime(string name)
		{
			PlayerPrefs.SetString("Time_" + name, DateTime.Now.ToString());
		}

		public static double CalculateTimeToNow(string name)
		{
			if (PlayerPrefs.HasKey("Time_" + name))
			{
				DateTime d = DateTime.Parse(PlayerPrefs.GetString("Time_" + name));
				return (DateTime.Now - d).TotalSeconds;
			}
			return 0.0;
		}

		public static int GetLeftSecondsToday()
		{
			DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			return 86400 - (int)(DateTime.Now - d).TotalSeconds;
		}
	}
}
