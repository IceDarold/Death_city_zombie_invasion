using System;
using UnityEngine;

namespace Zombie3D
{
	public interface IBindAbleCollection
	{
		void SeekGameObjectInCollection(GameObject obj, Action<string, BindLevel> Success);
	}
}
