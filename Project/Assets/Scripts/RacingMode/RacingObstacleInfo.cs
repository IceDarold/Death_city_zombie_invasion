using System;
using UnityEngine;

namespace RacingMode
{
	public class RacingObstacleInfo : MonoBehaviour
	{
		private void Awake()
		{
			this._obstacle = base.GetComponent<Rigidbody>();
			this.ObstacleTrigger = base.GetComponent<BoxCollider>();
		}

		public void Activate()
		{
			this.recycling = false;
			this.ExplosionPartical.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void Recycle()
		{
			if (!this.recycling)
			{
				this.recycling = true;
				base.gameObject.SetActive(false);
			}
		}

		public void CollideWithCar()
		{
			int num = UnityEngine.Random.Range(0, RacingSceneManager.Instance.HitCarSound.Length);
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(RacingSceneManager.Instance.HitCarSound[num], false);
			if (this.ExplosionPartical != null)
			{
				this.ExplosionPartical.gameObject.SetActive(true);
				this.ExplosionPartical.Play(true);
			}
			if (RacingSceneManager.Instance.Car.transform.position.x < base.transform.position.x)
			{
				this._obstacle.AddExplosionForce(this._obstacle.mass * this.MassForceProportion, new Vector3(base.transform.position.x - this.ExplosionDirection.x, base.transform.position.y + this.ExplosionDirection.y, base.transform.position.z + this.ExplosionDirection.z), 10f);
			}
			else
			{
				this._obstacle.AddExplosionForce(this._obstacle.mass * this.MassForceProportion, new Vector3(base.transform.position.x + this.ExplosionDirection.x, base.transform.position.y + this.ExplosionDirection.y, base.transform.position.z + this.ExplosionDirection.z), 10f);
			}
		}

		private void FixedUpdate()
		{
			if ((RacingSceneManager.Instance.GameState == RacingState.RACING || RacingSceneManager.Instance.GameState == RacingState.GAME_OVER) && (base.transform.position.z < RacingSceneManager.Instance.Car.transform.position.z - 100f || base.transform.position.y < -5f))
			{
				this.Recycle();
			}
		}

		public ObstacleEnum Type;

		[Header("撞击特效")]
		public ParticleSystem ExplosionPartical;

		[Header("伤害值")]
		public float HurtValue;

		[Header("减速值")]
		public float SlowDownValue;

		[Header("施加给汽车的撞击力")]
		public float HitPower;

		[Header("质量与爆炸力的比值")]
		public float MassForceProportion;

		[Header("炸点")]
		public Vector3 ExplosionDirection;

		[Header("起跳曲线")]
		public AnimationCurve JumpCuver;

		[Header("起跳角")]
		public float JumpAngle;

		[Header("仰角曲线")]
		public AnimationCurve RiseCuver;

		[Header("着陆点索引值")]
		public int LandingIndex = 2;

		[HideInInspector]
		public BoxCollider ObstacleTrigger;

		private Rigidbody _obstacle;

		private bool recycling;
	}
}
