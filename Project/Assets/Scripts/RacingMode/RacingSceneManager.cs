using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using UnityEngine;

namespace RacingMode
{
	public class RacingSceneManager : MonoBehaviour
	{
		private void Awake()
		{
			RacingSceneManager.Instance = this;
			Physics.gravity = new Vector3(0f, -30f, 0f);
			this.GameState = RacingState.BEGIN;
			this.isBegin = false;
			this.ReviveTimes = 0;
			this.KillZombies = 0;
			this.isShakePosition = false;
			this.isShakeRotation = false;
			for (int i = 1; i <= 4; i++)
			{
				string path = "Zombie_" + i.ToString("D2");
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(path)) as GameObject;
				gameObject.GetComponent<RacingZombieInfo>().Attr.CombineMesh(new Bone[0]);
				this.ZombiePrefab.Add(gameObject);
			}
		}

		private void Start()
		{
			this.LoadScene();
			Singleton<FadeAnimationScript>.Instance.SetColor(Color.black, null);
			this.CameraRoot.gameObject.SetActive(false);
			if (!this.isDemo)
			{
				Singleton<UiManager>.Instance.CurrentPage.Hide();
				Singleton<UiManager>.Instance.CanBack = false;
				Singleton<FadeAnimationScript>.Instance.FadeOutBlack(0.2f, delegate
				{
				});
				if (this.Plot != null)
				{
					this.Plot.ShowPlot(delegate
					{
						this.CameraRoot.gameObject.SetActive(true);
						this.Car.Init();
						this.GameState = RacingState.RACING;
						this.CameraAnimator.Play("Camera_Animation");
					});
				}
			}
			else
			{
				Singleton<FadeAnimationScript>.Instance.FadeOutBlack(0.2f, delegate
				{
				});
				this.CameraRoot.gameObject.SetActive(true);
				this.Car.Init();
				this.GameState = RacingState.RACING;
			}
		}

		private void OnDisable()
		{
			Physics.gravity = new Vector3(0f, -10f, 0f);
		}

		private void SetBeginAnimation()
		{
			if (!this.isBegin)
			{
				this.isBegin = true;
				this.SceneCamera.transform.SetParent(this.CameraRunPosition);
				this.SceneCamera.transform.DOLocalMove(Vector3.zero, 0.5f, false);
				this.SceneCamera.transform.DOLocalRotate(Vector3.zero, 0.5f, RotateMode.Fast).OnComplete(delegate
				{
					if (!this.isDemo)
					{
						Singleton<UiManager>.Instance.CurrentPage.Show();
						Singleton<UiManager>.Instance.CanBack = true;
					}
				});
			}
		}

		private void LoadScene()
		{
			if (!this.isDemo)
			{
				string scene = CheckpointDataManager.GetScene(CheckpointDataManager.SelectCheckpoint.SceneID);
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(scene)) as GameObject;
				this.SceneInfo = gameObject.GetComponent<RacingSceneInfo>();
			}
			if (this.SceneInfo != null)
			{
				for (int i = 0; i < this.SceneInfo.Tracks.Count; i++)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(this.SceneInfo.Tracks[i].Name)) as GameObject;
					gameObject2.transform.position = this.SceneInfo.Tracks[i].Position;
					gameObject2.transform.rotation = this.SceneInfo.Tracks[i].Rotation;
					gameObject2.transform.localScale = this.SceneInfo.Tracks[i].Scale;
					RacingTrackInfo component = gameObject2.GetComponent<RacingTrackInfo>();
					component.StartPoint.position = this.SceneInfo.Tracks[i].StartPoint;
					component.EndPoint.position = this.SceneInfo.Tracks[i].EndPoint;
					gameObject2.SetActive(component.StartPoint.position.z < this.Car.transform.position.z + 350f);
					this.Tracks.Add(component);
				}
				for (int j = 0; j < this.SceneInfo.Zombies.Count; j++)
				{
					GameObject gameObject3 = null;
					string name = this.SceneInfo.Zombies[j].Name;
					if (name != null)
					{
						if (!(name == "Zombie_01"))
						{
							if (!(name == "Zombie_02"))
							{
								if (!(name == "Zombie_03"))
								{
									if (name == "Zombie_04")
									{
										gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.ZombiePrefab[3]);
									}
								}
								else
								{
									gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.ZombiePrefab[2]);
								}
							}
							else
							{
								gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.ZombiePrefab[1]);
							}
						}
						else
						{
							gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.ZombiePrefab[0]);
						}
					}
					gameObject3.transform.position = this.SceneInfo.Zombies[j].Position;
					gameObject3.transform.rotation = this.SceneInfo.Zombies[j].Rotation;
					gameObject3.transform.localScale = this.SceneInfo.Zombies[j].Scale;
					RacingZombieInfo component2 = gameObject3.GetComponent<RacingZombieInfo>();
					component2.Type = this.SceneInfo.Zombies[j].Type;
					component2.MaxHp = this.SceneInfo.Zombies[j].MaxHp;
					component2.WalkSpeed = this.SceneInfo.Zombies[j].WalkSpeed;
					component2.RunSpeed = this.SceneInfo.Zombies[j].RunSpeed;
					component2.ChaseProbability = this.SceneInfo.Zombies[j].ChaseProbability;
					component2.ChaseSpeed = this.SceneInfo.Zombies[j].ChaseSpeed;
					component2.JumpDistance = this.SceneInfo.Zombies[j].JumpDistance;
					component2.JumpTime = this.SceneInfo.Zombies[j].JumpTime;
					component2.HurtRate = this.SceneInfo.Zombies[j].HurtRate;
					component2.HurtValue = this.SceneInfo.Zombies[j].HurtValue;
					if (component2.transform.position.z < this.Car.transform.position.z + 350f)
					{
						component2.SetActivate();
					}
					else
					{
						gameObject3.SetActive(false);
					}
					this.Zombies.Add(component2);
				}
				for (int k = 0; k < this.SceneInfo.Obstacles.Count; k++)
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load(this.SceneInfo.Obstacles[k].Name)) as GameObject;
					gameObject4.transform.position = this.SceneInfo.Obstacles[k].Position;
					gameObject4.transform.rotation = this.SceneInfo.Obstacles[k].Rotation;
					gameObject4.transform.localScale = this.SceneInfo.Obstacles[k].Scale;
					RacingObstacleInfo component3 = gameObject4.GetComponent<RacingObstacleInfo>();
					component3.Type = this.SceneInfo.Obstacles[k].Type;
					component3.HurtValue = this.SceneInfo.Obstacles[k].HurtValue;
					component3.SlowDownValue = this.SceneInfo.Obstacles[k].SlowDownValue;
					component3.HitPower = this.SceneInfo.Obstacles[k].HitPower;
					component3.MassForceProportion = this.SceneInfo.Obstacles[k].MassForceProportion;
					component3.ExplosionDirection = this.SceneInfo.Obstacles[k].ExplosionDirection;
					component3.JumpCuver = this.SceneInfo.Obstacles[k].JumpCuver;
					component3.RiseCuver = this.SceneInfo.Obstacles[k].RiseCuver;
					component3.JumpAngle = this.SceneInfo.Obstacles[k].JumpAngle;
					component3.LandingIndex = this.SceneInfo.Obstacles[k].LandingIndex;
					gameObject4.SetActive(component3.transform.position.z < this.Car.transform.position.z + 350f);
					this.Obstacles.Add(component3);
				}
			}
		}

		private void RefreshScene()
		{
			for (int i = 0; i < this.Tracks.Count; i++)
			{
				if (this.Tracks[i].StartPoint.position.z > this.Car.transform.position.z + 300f && this.Tracks[i].StartPoint.position.z < this.Car.transform.position.z + 400f)
				{
					this.Tracks[i].gameObject.SetActive(true);
				}
			}
			for (int j = 0; j < this.Zombies.Count; j++)
			{
				if (this.Zombies[j].transform.position.z > this.Car.transform.position.z + 300f && this.Zombies[j].transform.position.z < this.Car.transform.position.z + 400f)
				{
					this.Zombies[j].SetActivate();
				}
			}
			for (int k = 0; k < this.Obstacles.Count; k++)
			{
				if (this.Obstacles[k].transform.position.z > this.Car.transform.position.z + 300f && this.Obstacles[k].transform.position.z < this.Car.transform.position.z + 400f)
				{
					this.Obstacles[k].gameObject.SetActive(true);
				}
			}
		}

		public void ShakePosotion(float t, Vector3 vec)
		{
			if (!this.isShakePosition)
			{
				this.isShakePosition = true;
				RacingSceneManager.Instance.SceneCamera.DOShakePosition(t, vec, 10, 90f, true).OnComplete(delegate
				{
					this.isShakePosition = false;
				});
			}
		}

		public void ShakeRotation(float t, Vector3 vec)
		{
			if (!this.isShakeRotation)
			{
				this.isShakeRotation = true;
				RacingSceneManager.Instance.SceneCamera.DOShakeRotation(t, vec, 10, 90f, true).OnComplete(delegate
				{
					this.isShakeRotation = false;
				});
			}
		}

		public ParticleSystem SetDeadParticle(Transform root)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.ZombieDeadParticle);
			particleSystem.transform.SetParent(root);
			particleSystem.transform.localPosition = Vector3.zero;
			return particleSystem;
		}

		public void SetPause(bool pause)
		{
			if (pause)
			{
				this.LastState = this.GameState;
				this.GameState = RacingState.PAUSE;
			}
			else
			{
				this.GameState = this.LastState;
			}
		}

		public void SetRevive()
		{
			this.ReviveTimes++;
			this.Car.OnRevival();
			this.ClearObstacles();
			this.GameState = RacingState.RACING;
		}

		public void SetComplete()
		{
			this.GameState = RacingState.COMPLETED;
			if (!this.isDemo)
			{
				GamePage currentPage = Singleton<UiManager>.Instance.CurrentPage;
				if (currentPage.Name == PageName.InRacingPage)
				{
					currentPage.Close();
				}
				base.StartCoroutine(this.ShowCompletePage());
			}
		}

		private IEnumerator ShowCompletePage()
		{
			yield return new WaitForSeconds(2f);
			this.Car.State = RacingCarState.WAITING;
			this.Car.Speed = Vector3.zero;
			Singleton<UiManager>.Instance.GameSuccess = 1;
			Singleton<UiManager>.Instance.ShowPage(PageName.CommonFinishPage, null);
			yield break;
		}

		public void ClearObstacles()
		{
			for (int i = 0; i < this.Obstacles.Count; i++)
			{
				float num = Vector3.Distance(this.Car.transform.position, this.Obstacles[i].transform.position);
				if (num < 30f && this.Obstacles[i].Type == ObstacleEnum.Block)
				{
					Rigidbody component = this.Obstacles[i].GetComponent<Rigidbody>();
					component.AddExplosionForce(component.mass * 3000f, this.Car.transform.position, 30f);
				}
			}
		}

		public void Update()
		{
			if (this.GameState == RacingState.RACING && this.Car.transform.position.z > 0f)
			{
				this.SetBeginAnimation();
			}
		}

		public void FixedUpdate()
		{
			switch (this.GameState)
			{
			case RacingState.BEGIN:
				this.RefreshScene();
				break;
			case RacingState.RACING:
				if (this.Car.transform.position.z > 0f)
				{
					this.CameraRoot.position = new Vector3(Mathf.Lerp(this.CameraRoot.position.x, this.Car.transform.position.x, this.SmoothTime * Time.deltaTime), Mathf.Lerp(this.CameraRoot.position.y, this.Car.transform.position.y, this.SmoothTime * Time.deltaTime), this.Car.transform.position.z);
				}
				this.RefreshScene();
				break;
			case RacingState.COMPLETED:
				this.SceneCamera.transform.LookAt(this.Car.transform);
				break;
			case RacingState.GAME_OVER:
				this.CameraRoot.position = new Vector3(Mathf.Lerp(this.CameraRoot.position.x, this.Car.transform.position.x, this.SmoothTime * Time.deltaTime), Mathf.Lerp(this.CameraRoot.position.y, this.Car.transform.position.y, this.SmoothTime * Time.deltaTime), this.Car.transform.position.z);
				break;
			}
		}

		public static RacingSceneManager Instance;

		public RacingState GameState;

		public RacingCarInfo Car;

		public RacingSceneInfo SceneInfo;

		public RacingPlot Plot;

		public Transform CameraRoot;

		public Camera SceneCamera;

		public Animator CameraAnimator;

		public Transform CameraRunPosition;

		[Tooltip("相机跟随平滑时间")]
		public float SmoothTime;

		[Tooltip("相机位置震动幅度")]
		public Vector3 CameraShakePosition;

		[Tooltip("相机位置震动时间")]
		public float ShakePositionTime = 0.7f;

		[Tooltip("相机角度震动幅度")]
		public Vector3 CameraShakeRotation;

		[Tooltip("相机角度震动时间")]
		public float ShakeRotationTime = 0.7f;

		[Tooltip("僵尸死亡特效")]
		public ParticleSystem ZombieDeadParticle;

		[Tooltip("僵尸死亡音效")]
		public AudioClip[] ZombieDieSound;

		[Tooltip("撞车音效")]
		public AudioClip[] HitCarSound;

		[Tooltip("赛车爆炸音效")]
		public AudioClip[] CarExplosionSound;

		public bool isDemo;

		[HideInInspector]
		public int ReviveTimes;

		[HideInInspector]
		public int KillZombies;

		private bool isBegin;

		private RacingState LastState;

		private bool isShakePosition;

		private bool isShakeRotation;

		private List<RacingTrackInfo> Tracks = new List<RacingTrackInfo>();

		private List<RacingZombieInfo> Zombies = new List<RacingZombieInfo>();

		private List<RacingObstacleInfo> Obstacles = new List<RacingObstacleInfo>();

		private List<GameObject> ZombiePrefab = new List<GameObject>();
	}
}
