using System;
using UnityEngine;

public interface EnemyTarget
{
	void OnHit(float damage, bool isCritical, AttackType type);

	Transform GetTransform();

	void DoRevive();

	void DoPause();

	void DoResume();

	bool IsVisible();

	EnemyTargetType GetTargetType();
}
