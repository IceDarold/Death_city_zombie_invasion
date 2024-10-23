using System;
using UnityEngine;
using Zombie3D;

public class BaseListener : StateMachineBehaviour
{
	public void SetEnemy(Enemy target)
	{
		this.enemy = target;
	}

	public Enemy enemy;
}
