using System;
using UnityEngine;

public class ArrayHeader : HeaderAttribute
{
	public ArrayHeader(string _name) : base(_name)
	{
		this.name = _name;
	}

	public string name;
}
