using System;
using UnityEngine;
using Zombie3D;

public class EnemyFocusSensor : MonoBehaviour, EnemyTarget
{
	public void OnEnable()
	{
		this.updateEnemy = new TimeScheduler(0.5f, 0f, new Action(this.DoCheckEnemyDistances), null);
	}

	public void OnDisable()
	{
		if (GameApp.GetInstance().GetGameScene() != null)
		{
			GameApp.GetInstance().GetGameScene().ReleaseEnemyTargetOnSensor(base.gameObject);
		}
	}

	public void Update()
	{
		this.updateEnemy.DoUpdate(Time.deltaTime);
	}

	private void DoCheckEnemyDistances()
	{
		GameApp.GetInstance().GetGameScene().SetEnemyTargetOnSensor(this);
	}

	public bool IsVisible()
	{
		return base.gameObject.activeSelf;
	}

	public EnemyTargetType GetTargetType()
	{
		return EnemyTargetType.FOCUS_SENSOR;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	public void OnHit(float damage, bool isCritical, AttackType attackType)
	{
	}

	public void DoRevive()
	{
	}

	public void DoPause()
	{
	}

	public void DoResume()
	{
	}

	[CNName("吸引范围")]
	public float sensorRadius = 5f;

	protected TimeScheduler updateEnemy;
}
