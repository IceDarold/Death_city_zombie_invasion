using System;
using UnityEngine;
using Zombie3D;

public class EnemyAnimatorEventListener : MonoBehaviour
{
	public void OnEnemyAttackFrameLeftHand()
	{
		if (this.enemy != null)
		{
			this.enemy.OnAttackEventFrame(true);
		}
	}

	public void OnEnemyAttackFrameRightHand()
	{
		if (this.enemy != null)
		{
			this.enemy.OnAttackEventFrame(false);
		}
	}

	public void OnEnemyAttackFrame()
	{
		if (this.enemy != null)
		{
			this.enemy.OnAttackEventFrame();
		}
	}

	public void OnEnemyPlayParticle(int index)
	{
		if (this.enemy != null)
		{
			this.enemy.OnPlayParticle(index);
		}
	}

	public void OneEnemyAnimationOver(int aniKey)
	{
		if (this.enemy != null)
		{
			this.enemy.SetAnimationEnd((E_ANIMATION)aniKey);
		}
	}

	public void OnDeadAnimationOver()
	{
	}

	public void ShakeCamera(int _id)
	{
		this.enemy.ShakeCamera(_id);
	}

	public void OnEnemySpecialEventFrame(int id)
	{
		if (this.enemy != null)
		{
			this.enemy.OnSpecialEventFrame(id);
		}
	}

	public void OnEnemyAttackBoxEnable(int boxID)
	{
		if (this.enemy != null)
		{
			this.enemy.OnEnemyAttackBoxEnable(boxID);
		}
	}

	public void OnEnemyAttackBoxDisable()
	{
		if (this.enemy != null)
		{
			this.enemy.OnEnemyAttackBoxDisable();
		}
	}

	public void StopFixOffset()
	{
		if (this.enemy != null)
		{
			this.enemy.StopFixOffset();
		}
	}

	public void StartJumpOffset()
	{
		if (this.enemy != null)
		{
			this.enemy.StartDoJumpOffset();
		}
	}

	public Enemy enemy;
}
