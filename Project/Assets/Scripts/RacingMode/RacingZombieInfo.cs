using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RacingMode
{
	public class RacingZombieInfo : MonoBehaviour
	{
		private void Awake()
		{
			this.ZombieTrigger = base.gameObject.GetComponent<CapsuleCollider>();
		}

		private void Start()
		{
			this.car = RacingSceneManager.Instance.Car;
			this.RandomDistance = UnityEngine.Random.Range(1f, 2f);
		}

		public void SetActivate()
		{
			this.point = -1;
			this.hp = this.MaxHp;
			this.State = RacingZombieState.WANDERING;
			this.m_Animator.gameObject.SetActive(true);
			base.gameObject.SetActive(true);
		}

		public void SetHurt(int value)
		{
			this.hp -= value;
			if (this.hp <= 0)
			{
				this.State = RacingZombieState.DEAD;
			}
		}

		public void Recycle()
		{
			base.gameObject.SetActive(false);
			base.gameObject.transform.SetParent(null);
		}

		public void SetWandering()
		{
			switch (this.Type)
			{
			case RacingZombieType.Idle:
				this.m_Animator.Play("Idle");
				if (UnityEngine.Random.Range(0, 100) < this.ChaseProbability)
				{
					if (base.transform.position.z < this.car.transform.position.z - 2f)
					{
						float num = Vector3.Distance(base.transform.position, this.car.transform.position);
						if (num > this.JumpDistance * 3f)
						{
							this.State = RacingZombieState.WATCHING;
						}
						else
						{
							this.State = RacingZombieState.CHASING;
						}
					}
				}
				else
				{
					this.State = RacingZombieState.WATCHING;
				}
				break;
			case RacingZombieType.Walk:
				this.speed = this.WalkSpeed;
				this.m_Animator.Play("Walk");
				if (base.transform.position.x < -10f)
				{
					base.transform.rotation = Quaternion.Euler(new Vector3(0f, (float)UnityEngine.Random.Range(45, 135), 0f));
				}
				else if (base.transform.position.x > 10f)
				{
					base.transform.rotation = Quaternion.Euler(new Vector3(0f, (float)UnityEngine.Random.Range(225, 315), 0f));
				}
				base.transform.Translate(new Vector3(0f, 0f, this.speed * Time.deltaTime), Space.Self);
				if (UnityEngine.Random.Range(0, 100) < this.ChaseProbability)
				{
					if (base.transform.position.z < this.car.transform.position.z - 2f)
					{
						float num2 = Vector3.Distance(base.transform.position, this.car.transform.position);
						if (num2 > this.JumpDistance * 3f)
						{
							this.State = RacingZombieState.WATCHING;
						}
						else
						{
							this.State = RacingZombieState.CHASING;
						}
					}
				}
				else
				{
					this.State = RacingZombieState.WATCHING;
				}
				break;
			case RacingZombieType.Run:
				this.speed = this.RunSpeed;
				if (base.transform.position.x < -10f || base.transform.position.x > 10f)
				{
					this.RunSpeed = 0f;
					this.m_Animator.Play("Idle");
				}
				else
				{
					this.m_Animator.Play("Run");
				}
				base.transform.Translate(Vector3.forward * this.speed * Time.deltaTime, Space.Self);
				break;
			case RacingZombieType.Recline:
				this.m_Animator.Play("Recline");
				break;
			case RacingZombieType.Biting:
				this.m_Animator.Play("Biting");
				break;
			}
		}

		public void SetWatching()
		{
			if (base.transform.position.x < this.car.transform.position.x)
			{
				base.transform.LookAt(this.car.transform.position + new Vector3(-this.RandomDistance, 0f, -2f));
			}
			else
			{
				base.transform.LookAt(this.car.transform.position + new Vector3(this.RandomDistance, 0f, -2f));
			}
			if (this.speed > this.WalkSpeed)
			{
				this.speed -= Time.deltaTime * 20f;
				this.m_Animator.Play("Run");
			}
			else
			{
				this.speed = this.WalkSpeed;
				this.m_Animator.Play("Walk");
			}
			base.transform.Translate(new Vector3(0f, 0f, this.speed * Time.deltaTime), Space.Self);
		}

		public void SetChasing()
		{
			this.m_Animator.Play("Run");
			this.speed = this.ChaseSpeed;
			if (base.transform.position.x < this.car.transform.position.x)
			{
				base.transform.LookAt(new Vector3(this.car.transform.position.x - this.RandomDistance, 0f, this.car.transform.position.z));
			}
			else
			{
				base.transform.LookAt(new Vector3(this.car.transform.position.x + this.RandomDistance, 0f, this.car.transform.position.z));
			}
			base.transform.Translate(new Vector3(0f, 0f, this.speed * Time.deltaTime), Space.Self);
			float num = Vector3.Distance(base.transform.position, this.car.CarCenter.position);
			if (num <= this.JumpDistance)
			{
				this.point = this.getAttachPosition();
				if (this.point >= 0 && this.point < this.car.AttachPoints.Length)
				{
					this.car.AttachZombies[this.point] = this;
					this.State = RacingZombieState.JUMPING;
					this.SetJumping();
				}
				else
				{
					this.State = RacingZombieState.WATCHING;
				}
			}
			else if (num >= this.JumpDistance * 2f)
			{
				this.State = RacingZombieState.WATCHING;
			}
		}

		public void SetJumping()
		{
			this.m_Animator.Play("Jump");
			this.ZombieTrigger.enabled = false;
			base.transform.SetParent(this.car.AttachPoints[this.point].transform);
			base.transform.DOLocalMove(Vector3.zero, 0.5f, false).OnComplete(delegate
			{
				this.State = RacingZombieState.SHAKING;
			});
		}

		public void SetShaking()
		{
			base.transform.position = this.car.AttachPoints[this.point].position;
			base.transform.eulerAngles = this.car.AttachPoints[this.point].eulerAngles;
			this.m_Animator.Play("Shake");
			if (this.rate < this.HurtRate)
			{
				this.rate += Time.deltaTime;
			}
			else
			{
				this.rate = 0f;
				this.car.SetHurt(this.HurtValue);
			}
		}

		private int getAttachPosition()
		{
			if (base.transform.position.x < this.car.transform.position.x)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.car.AttachZombies[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = 3; j < 6; j++)
				{
					if (this.car.AttachZombies[j] == null)
					{
						return j;
					}
				}
			}
			return -1;
		}

		public void OnHit()
		{
			int num = UnityEngine.Random.Range(0, RacingSceneManager.Instance.ZombieDieSound.Length);
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(RacingSceneManager.Instance.ZombieDieSound[num], false);
			this.PlayDeadParticle();
			this.hp = 0;
			this.State = RacingZombieState.DEAD;
			this.ZombieTrigger.enabled = false;
			RacingSceneManager.Instance.KillZombies++;
			switch (this.Type)
			{
			case RacingZombieType.Idle:
			case RacingZombieType.Walk:
			case RacingZombieType.Run:
			case RacingZombieType.Biting:
				if (this.car.State == RacingCarState.JUMPING)
				{
					this.m_Animator.gameObject.SetActive(false);
				}
				else
				{
					base.transform.DORotate(new Vector3((float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180)), 0.6f, RotateMode.Fast).SetEase(Ease.OutQuint);
					Vector3 b;
					if (base.transform.position.x < this.car.transform.position.x)
					{
						b = new Vector3(UnityEngine.Random.Range(-2.5f, -1f), UnityEngine.Random.RandomRange(3.2f, 4f), UnityEngine.Random.Range(4f, 8f));
					}
					else
					{
						b = new Vector3(UnityEngine.Random.Range(1f, 2.5f), UnityEngine.Random.RandomRange(3.2f, 4f), UnityEngine.Random.Range(4f, 8f));
					}
					base.transform.DOMove(base.transform.position + b, 0.4f, false).SetEase(Ease.OutQuint).OnComplete(delegate
					{
						base.gameObject.SetActive(false);
					});
				}
				break;
			}
		}

		public void PlayDeadParticle()
		{
			if (this.DeathParticle == null)
			{
				this.DeathParticle = RacingSceneManager.Instance.SetDeadParticle(base.transform);
			}
			this.DeathParticle.Play(true);
		}

		public void FixedUpdate()
		{
			if (RacingSceneManager.Instance.GameState == RacingState.BEGIN || RacingSceneManager.Instance.GameState == RacingState.RACING)
			{
				switch (this.State)
				{
				case RacingZombieState.WANDERING:
					this.SetWandering();
					break;
				case RacingZombieState.CHASING:
					this.SetChasing();
					break;
				case RacingZombieState.SHAKING:
					this.SetShaking();
					break;
				case RacingZombieState.WATCHING:
					this.SetWatching();
					break;
				}
				if (base.transform.position.z < this.car.transform.position.z - 100f || base.transform.position.y < -5f)
				{
					this.Recycle();
				}
			}
		}

		[Header("僵尸状态（不可修改）")]
		public RacingZombieState State;

		[Tooltip("僵尸类型")]
		public RacingZombieType Type;

		[Tooltip("动画控制器")]
		public Animator m_Animator;

		[Tooltip("僵尸血量")]
		public int MaxHp;

		[Tooltip("行走速度")]
		public float WalkSpeed = 0.7f;

		[Tooltip("奔跑速度")]
		public float RunSpeed = 0.7f;

		[Tooltip("追车概率")]
		[Range(0f, 100f)]
		public int ChaseProbability;

		[Tooltip("追车速度")]
		public float ChaseSpeed = 50f;

		[Tooltip("起跳距离")]
		public float JumpDistance = 10f;

		[Tooltip("跳跃时间")]
		public float JumpTime = 1f;

		[Tooltip("伤害频率")]
		public float HurtRate = 4f;

		[Tooltip("伤害值")]
		public float HurtValue;

		[Tooltip("血条UI")]
		public Slider HpSlider;

		[HideInInspector]
		public CapsuleCollider ZombieTrigger;

		public EnemyAttr Attr;

		private RacingCarInfo car;

		private float speed;

		private int point = -1;

		private int hp;

		private float rate;

		private float RandomDistance;

		private ParticleSystem DeathParticle;
	}
}
