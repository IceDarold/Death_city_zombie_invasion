using System;
using UnityEngine;

namespace SplineUtilities
{
	public static class SplineUtils
	{
		public static float WrapValue(float v, float start, float end, WrapMode wMode)
		{
			switch (wMode)
			{
			case WrapMode.Default:
			case WrapMode.Loop:
				return Mathf.Repeat(v, end - start) + start;
			case WrapMode.Once:
			case WrapMode.ClampForever:
				return Mathf.Clamp(v, start, end);
			case WrapMode.PingPong:
				return Mathf.PingPong(v, end - start) + start;
			}
			return v;
		}
	}
}
