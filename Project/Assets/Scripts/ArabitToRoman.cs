using System;

public class ArabitToRoman
{
	public static string Exchange(int aNumber)
	{
		if (aNumber < 1 || aNumber > 3999)
		{
			return "-1";
		}
		int[] array = new int[]
		{
			1000,
			900,
			500,
			400,
			100,
			90,
			50,
			40,
			10,
			9,
			5,
			4,
			1
		};
		string[] array2 = new string[]
		{
			"M",
			"CM",
			"D",
			"CD",
			"C",
			"XC",
			"L",
			"XL",
			"X",
			"IX",
			"V",
			"IV",
			"I"
		};
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			while (aNumber >= array[i])
			{
				text += array2[i];
				aNumber -= array[i];
			}
		}
		return text;
	}
}
