using System;

namespace Zombie3D
{
	public interface IBindAble
	{
		BindLevel GetBindInfo();

		bool CanUse(int index);
	}
}
