using System;
using UnityEngine;

namespace Zombie3D
{
	public interface IPlayerControl
	{
		void UseProp();

		void UseReBirth();

		bool ChangeWeapon();

		void DoReload();

		bool DoSkill(int skillID);

		bool DoQTE();

		Transform GetTransform();

		bool IsFullBodyActionOver { get; }
	}
}
