using System;

[Serializable]
public class BindLevel
{
	public bool GetEnable(int levelIndex)
	{
		if (levelIndex == -1)
		{
			return true;
		}
		int num = levelIndex / 32;
		if (num >= this.bindLevel.Length)
		{
			return false;
		}
		int num2 = levelIndex % 32;
		return ((ulong)this.bindLevel[num] & (ulong)(1L << (num2 & 31))) > 0UL;
	}

	public const int BIND_ARRAY_LENGTH = 8;

	public uint[] bindLevel = new uint[8];
}
