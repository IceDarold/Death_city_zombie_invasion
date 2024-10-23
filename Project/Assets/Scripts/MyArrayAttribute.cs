using System;
using UnityEngine;

public class MyArrayAttribute : PropertyAttribute
{
	public MyArrayAttribute(params string[] title)
	{
		this.header = new string[title.Length];
		for (int i = 0; i < this.header.Length; i++)
		{
			this.header[i] = title[i];
		}
	}

	public string[] header;
}
