using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace RacingMode
{
	public class RacingCarInfo : MonoBehaviour
	{
		private void Awake()
		{
			this.m_car = base.GetComponent<Rigidbody>();
		}

		public void Init()
		{
			base.gameObject.SetActive(true);
			this.State = RacingCarState.RUNNING;
			this.m_car.useGravity = true;
			this.hp = this.MaxHp;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (this.State == RacingCarState.RUNNING)
			{
				if (other.tag == "RacingObstacle")
				{
					RacingObstacleInfo component = other.gameObject.GetComponent<RacingObstacleInfo>();
					if (component.Type == ObstacleEnum.Springboard)
					{
						this.State = RacingCarState.JUMPING;
						this.isLanding = false;
						this.jumpTime = 0f;
						this.jumpCurve = component.JumpCuver;
						this.riseCurve = component.RiseCuver;
						this.jumpAngle = component.JumpAngle;
						this.LandingIndex = component.LandingIndex;
					}
					else if (component.Type == ObstacleEnum.Block)
					{
						this.Speed.z = this.Speed.z - component.SlowDownValue;
						if (this.Speed.z <= 0f)
						{
							this.Speed.z = 0f;
						}
						this.SetHurt(component.HurtValue);
						if (component.HurtValue >= 10f && !RacingSceneManager.Instance.isDemo)
						{
							if (this.RacingPage == null)
							{
								this.RacingPage = Singleton<UiManager>.Instance.CurrentPage.GetComponent<InRacingPage>();
							}
							this.RacingPage.PlayHurtParticle();
						}
						if (this.hp > 0f)
						{
							this.m_car.AddForce(Vector3.back * component.HitPower, ForceMode.Impulse);
							if (component.HurtValue >= 10f)
							{
								RacingSceneManager.Instance.ShakeRotation(RacingSceneManager.Instance.ShakeRotationTime, RacingSceneManager.Instance.CameraShakeRotation);
							}
						}
						else
						{
							int num = UnityEngine.Random.Range(1000, 1500);
							if (base.transform.position.x < component.transform.position.x)
							{
								this.m_car.AddExplosionForce(this.m_car.mass * (float)num, this.m_car.transform.position + new Vector3(UnityEngine.Random.Range(0f, 2f), -2f, 4f), 10f);
							}
							else
							{
								this.m_car.AddExplosionForce(this.m_car.mass * (float)num, this.m_car.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 0f), -2f, 4f), 10f);
							}
						}
						component.CollideWithCar();
					}
					else if (component.Type == ObstacleEnum.Complete)
					{
						RacingSceneManager.Instance.SetComplete();
					}
				}
				else if (other.tag == "RacingZombie")
				{
					RacingZombieInfo component2 = other.gameObject.GetComponent<RacingZombieInfo>();
					component2.OnHit();
				}
			}
		}

		private void TurningControl()
		{
			if (!this.isRotate)
			{
				this.m_offset = Mathf.Lerp(this.m_offset, 0f, Time.deltaTime * 2f);
			}
			this.Speed.x = this.RotateCuver.Evaluate(this.m_offset) * this.HorizontalSpeed;
			this.CarRoot.localEulerAngles = new Vector3(0f, this.RotateCuver.Evaluate(this.m_offset) * this.MaxRotateAngle, 0f);
		}

		public void TurningLeft()
		{
			if (this.State == RacingCarState.RUNNING)
			{
				this.isRotate = true;
				this.m_offset = Mathf.Lerp(this.m_offset, -1f, Time.deltaTime * 3f);
			}
		}

		public void TurningRight()
		{
			if (this.State == RacingCarState.RUNNING)
			{
				this.isRotate = true;
				this.m_offset = Mathf.Lerp(this.m_offset, 1f, Time.deltaTime * 3f);
			}
		}

		public void StopTurning()
		{
			this.isRotate = false;
		}

		public void SetHurt(float hurt)
		{
			this.hp -= hurt;
			if (Singleton<GlobalData>.Instance.RacingTimes <= 0 && this.hp <= 0f)
			{
				this.hp = 1f;
			}
			if (!RacingSceneManager.Instance.isDemo)
			{
				if (this.RacingPage == null)
				{
					this.RacingPage = Singleton<UiManager>.Instance.CurrentPage.GetComponent<InRacingPage>();
				}
				this.RacingPage.SetBlood(this.hp / this.MaxHp);
			}
			this.HurtParticles[0].gameObject.SetActive(this.hp <= this.MaxHp * 0.6f);
			this.HurtParticles[1].gameObject.SetActive(this.hp <= this.MaxHp * 0.3f);
			this.HurtParticles[2].gameObject.SetActive(this.hp <= 0f);
			if (this.hp <= 0f)
			{
				this.State = RacingCarState.DEAD;
				this.OnDead();
			}
		}

		public void OnDead()
		{
			this.ShakeDownZombies();
			int num = UnityEngine.Random.Range(0, RacingSceneManager.Instance.CarExplosionSound.Length);
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(RacingSceneManager.Instance.CarExplosionSound[num], false);
			RacingSceneManager.Instance.GameState = RacingState.GAME_OVER;
			if (!RacingSceneManager.Instance.isDemo)
			{
				Singleton<UiManager>.Instance.ShowPage(PageName.FailedNoticePage, null);
			}
		}

		public void OnRevival()
		{
			this.hp = this.MaxHp;
			if (!RacingSceneManager.Instance.isDemo)
			{
				if (this.RacingPage == null)
				{
					this.RacingPage = Singleton<UiManager>.Instance.CurrentPage.GetComponent<InRacingPage>();
				}
				this.RacingPage.SetBlood(this.hp / this.MaxHp);
			}
			for (int i = 0; i < this.HurtParticles.Length; i++)
			{
				this.HurtParticles[i].gameObject.SetActive(false);
			}
			this.Speed = Vector3.zero;
			this.m_car.position = new Vector3(5f, 0f, this.m_car.position.z);
			this.CarRoot.localEulerAngles = Vector3.zero;
			this.State = RacingCarState.RUNNING;
		}

		public void SetLanding()
		{
			if (!this.isLanding)
			{
				this.isLanding = true;
				Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.LandingClip, false);
				if (this.LandingParticle != null)
				{
					this.LandingParticle.Play(true);
				}
				RacingSceneManager.Instance.ShakePosotion(RacingSceneManager.Instance.ShakePositionTime, RacingSceneManager.Instance.CameraShakePosition);
				this.ShakeDownZombies();
			}
		}

		private void ShakeDownZombies()
		{
			for (int i = 0; i < this.AttachZombies.Count; i++)
			{
				if (this.AttachZombies[i] != null)
				{
					RacingZombieInfo zombie = this.AttachZombies[i];
					zombie.State = RacingZombieState.DEAD;
					zombie.transform.SetParent(null);
					zombie.PlayDeadParticle();
					RacingSceneManager.Instance.KillZombies++;
					if (i < 3)
					{
						zombie.transform.DOMoveX(this.AttachZombies[i].transform.position.x - 10f, 1f, false).SetEase(Ease.OutQuart).OnComplete(delegate
						{
							zombie.gameObject.SetActive(false);
						});
					}
					else
					{
						zombie.transform.DOMoveX(this.AttachZombies[i].transform.position.x + 10f, 1f, false).SetEase(Ease.OutQuart).OnComplete(delegate
						{
							zombie.gameObject.SetActive(false);
						});
					}
					this.AttachZombies[i] = null;
				}
			}
		}

		public void ShockWave()
		{
			RacingSceneManager.Instance.ShakePosotion(this.ElectricShakeTime, this.ElectricShake);
			if (this.ElectricParticle != null)
			{
				this.ElectricParticle.Play();
			}
			if (this.ElectricClip != null)
			{
				Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.ElectricClip, false);
			}
			this.isElectric = true;
			this.electric_time = this.ElectricDuration;
			this.electric_rate = this.ElectricRate;
		}

		private void SetElectricShock()
		{
			for (int i = 0; i < this.AttachZombies.Count; i++)
			{
				if (this.AttachZombies[i] != null)
				{
					RacingZombieInfo zombie = this.AttachZombies[i];
					zombie.SetHurt(this.ElectricHurt);
					if (zombie.State == RacingZombieState.DEAD)
					{
						zombie.transform.SetParent(null);
						zombie.PlayDeadParticle();
						RacingSceneManager.Instance.KillZombies++;
						if (i < 3)
						{
							zombie.transform.DOMoveX(this.AttachZombies[i].transform.position.x - 10f, 1f, false).SetEase(Ease.OutQuart).OnComplete(delegate
							{
								zombie.gameObject.SetActive(false);
							});
						}
						else
						{
							zombie.transform.DOMoveX(this.AttachZombies[i].transform.position.x + 10f, 1f, false).SetEase(Ease.OutQuart).OnComplete(delegate
							{
								zombie.gameObject.SetActive(false);
							});
						}
						this.AttachZombies[i] = null;
					}
				}
			}
		}

		private void Update()
		{
			if (this.isElectric)
			{
				if (this.electric_time > 0f)
				{
					this.electric_time -= Time.deltaTime;
					if (this.electric_rate < this.ElectricRate)
					{
						this.electric_rate += Time.deltaTime;
					}
					else
					{
						this.electric_rate = 0f;
						this.SetElectricShock();
					}
				}
				else
				{
					this.ElectricParticle.Stop();
					this.isElectric = false;
				}
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				RacingSceneManager.Instance.SceneCamera.DOShakeRotation(0.7f, RacingSceneManager.Instance.CameraShakeRotation, 10, 90f, true);
			}
		}

		private void FixedUpdate()
		{
			if (RacingSceneManager.Instance.GameState == RacingState.RACING || RacingSceneManager.Instance.GameState == RacingState.COMPLETED)
			{
				RacingCarState state = this.State;
				if (state != RacingCarState.RUNNING)
				{
					if (state != RacingCarState.JUMPING)
					{
						if (state == RacingCarState.DEAD)
						{
							this.CarShadow.gameObject.SetActive(false);
						}
					}
					else
					{
						this.m_car.useGravity = false;
						this.m_car.isKinematic = true;
						this.CarShadow.gameObject.SetActive(false);
						if (this.jumpTime < this.jumpCurve.keys[this.jumpCurve.length - 1].time)
						{
							this.jumpTime += Time.deltaTime;
						}
						else
						{
							this.State = RacingCarState.RUNNING;
						}
						if (this.jumpTime > this.jumpCurve.keys[this.LandingIndex].time)
						{
							this.SetLanding();
						}
						this.isRotate = false;
						this.TurningControl();
						if (this.jumpTime < this.riseCurve.keys[this.riseCurve.length - 1].time)
						{
							base.transform.localEulerAngles = new Vector3(this.riseCurve.Evaluate(this.jumpTime) * this.jumpAngle, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
						}
						if (this.Speed.z < this.MaxSpeed)
						{
							this.Speed.z = this.Speed.z + this.Acceleration * Time.deltaTime;
						}
						this.m_car.position += this.Speed * Time.deltaTime;
						this.m_car.position = new Vector3(Mathf.Clamp(this.m_car.position.x, -12f, 12f), this.jumpCurve.Evaluate(this.jumpTime) * 5f, this.m_car.position.z);
					}
				}
				else
				{
					this.m_car.useGravity = true;
					this.m_car.isKinematic = false;
					this.CarShadow.gameObject.SetActive(true);
					this.TurningControl();
					if (this.Speed.z < this.MaxSpeed)
					{
						this.Speed.z = this.Speed.z + this.Acceleration * Time.deltaTime;
					}
					this.m_car.position += this.Speed * Time.deltaTime;
				}
				if (this.State != RacingCarState.DEAD)
				{
					for (int i = 0; i < this.Wheels.Length; i++)
					{
						this.Wheels[i].Rotate(Vector3.left, this.Speed.z * 25f * Time.deltaTime);
					}
				}
				if (this.CarShadow.gameObject.activeSelf)
				{
					this.CarShadow.position = new Vector3(this.CarShadow.position.x, 0.05f, this.CarShadow.position.z);
				}
			}
		}

		[Header("赛车状态(查看用)")]
		public RacingCarState State;

		[Header("最大血量")]
		public float MaxHp;

		[Header("最大速度")]
		public float MaxSpeed;

		[Header("横向速度")]
		public float HorizontalSpeed;

		[Header("加速度")]
		public float Acceleration = 30f;

		[Header("最大偏转角")]
		public float MaxRotateAngle;

		[Header("转向曲线")]
		public AnimationCurve RotateCuver;

		[Header("速度(查看用)")]
		public Vector3 Speed;

		[Header("赛车父节点")]
		public Transform CarRoot;

		[Header("赛车中心点")]
		public Transform CarCenter;

		[Header("赛车音效")]
		public AudioClip RacingClip;

		[Header("状态特效")]
		public ParticleSystem[] HurtParticles;

		[Header("着陆音效")]
		public AudioClip LandingClip;

		[Header("着陆特效")]
		public ParticleSystem LandingParticle;

		[Space(10f)]
		[Header("电击持续时间")]
		public float ElectricDuration;

		[Header("电击频率")]
		public float ElectricRate;

		[Header("电击伤害")]
		public int ElectricHurt;

		[Header("电击音效")]
		public AudioClip ElectricClip;

		[Header("电击特效")]
		public ParticleSystem ElectricParticle;

		[Header("电击震动时间")]
		public float ElectricShakeTime = 0.7f;

		[Header("电击震动")]
		public Vector3 ElectricShake;

		[Space(10f)]
		[Header("车影")]
		public Transform CarShadow;

		[Header("车轮")]
		public Transform[] Wheels = new Transform[4];

		[Header("僵尸附着点")]
		public Transform[] AttachPoints = new Transform[6];

		public List<RacingZombieInfo> AttachZombies = new List<RacingZombieInfo>
		{
			null,
			null,
			null,
			null,
			null,
			null
		};

		private Rigidbody m_car;

		private float hp;

		private float m_offset;

		private bool isRotate;

		private AnimationCurve jumpCurve;

		private AnimationCurve riseCurve;

		private float jumpAngle;

		private float jumpTime;

		private bool isLanding = true;

		private int LandingIndex;

		private float electric_time;

		private float electric_rate;

		private bool isElectric;

		private InRacingPage RacingPage;
	}
}
