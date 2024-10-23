using System;
using UnityEngine;
using Zombie3D;

public class Cannon : MonoBehaviour
{
	public SceneNPC BindNPC
	{
		get
		{
			return this.bindNPC;
		}
	}

	public Cannon Init(CannonCreater creater)
	{
		this.gameScene = GameApp.GetInstance().GetGameScene();
		this.Caculate_RandomRange_And_ZorderDistances();
		this.shootShakeRootPos = this.xAxis.localPosition;
		this.shootScheduler = new TimeScheduler(this.shootInterval, 0f, new Action(this.DoFire), () => this.aniSpeed >= 0.95f);
		this.detectPosition = new TimeScheduler(0.1f, 0f, new Action(this.CheckReachTargetPoint), () => this.targetDestination != null && this.reachTargetPointCallback != null);
		this.sparksPool.Init("CannonSparks", Singleton<ResourceConfigScript>.Instance.hitparticles, 10, 0.22f);
		this.startQuaternion = this.gunRoot.transform.rotation;
		if (creater.controllCannon)
		{
			this.gameScene.SetPlayer2Cannon(this, null);
		}
		this.bindNPC = creater.npcCreater.GetNpc();
		if (this.bindNPC == null)
		{
			UnityEngine.Debug.LogError(creater.gameObject.name + "绑定的" + creater.npcCreater.gameObject.name + "没有生成NPC");
		}
		return this;
	}

	private void Caculate_RandomRange_And_ZorderDistances()
	{
		this.legOfRightTriangle_sin = this.hypotenuse * Mathf.Sin(this.randomAngel * 0.0174532924f);
		this.legOfRightTriangle_cos = this.hypotenuse * Mathf.Cos(this.randomAngel * 0.0174532924f);
	}

	public void SetShootState(bool _isShooting)
	{
		if (this.isShooting == _isShooting)
		{
			return;
		}
		this.isShooting = _isShooting;
		this.aniTargetSpeed = (float)((!this.isShooting) ? 0 : 1);
		if (this.isShooting)
		{
			this.startAudio.Play();
		}
	}

	public void DoLogic(float dt)
	{
		this.aniSpeed = Mathf.Lerp(this.aniSpeed, this.aniTargetSpeed, dt * 3f);
		this.animator.SetFloat("AniSpeed", this.aniSpeed);
		this.shootScheduler.DoUpdate(dt);
		this.detectPosition.DoUpdate(dt);
		this.sparksPool.AutoDestruct();
		if (this.aniSpeed > 0.95f && !this.gunFire.activeSelf)
		{
			this.gunFire.SetActive(true);
		}
		if (this.aniSpeed < 0.95f && this.gunFire.activeSelf)
		{
			this.gunFire.SetActive(false);
			this.ResetShake();
		}
		if (this.aniSpeed > 0.95f)
		{
			this.DoShootShake(dt);
		}
		if (this.gameScene.PlayingState != PlayingState.GamePlaying && this.gameScene.PlayingState != PlayingState.WaitForEnd)
		{
			this.aniTargetSpeed = 0f;
		}
	}

	public void LateUpdate()
	{
		this.gunRoot.transform.rotation = this.startQuaternion;
	}

	private void DoShootShake(float deltaTime)
	{
		this.shootShakeRange.y = UnityEngine.Random.Range(-this.shotRange, this.shotRange);
		this.shootShakeRange.z = UnityEngine.Random.Range(-this.shotRange, this.shotRange);
		this.xAxis.localPosition = this.shootShakeRootPos + this.shootShakeRange;
	}

	private void ResetShake()
	{
		this.xAxis.localPosition = this.shootShakeRootPos;
	}

	public Vector3 GetCameraAnchorPos_BeforeShake()
	{
		Vector3 vector = this.yAxis.TransformDirection(this.shootShakeRange);
		return this.cameraAnchor.position - new Vector3(vector.x * this.scaleRoot.transform.localScale.x, vector.y * this.scaleRoot.transform.localScale.y, vector.z * this.scaleRoot.transform.localScale.z);
	}

	private void CheckReachTargetPoint()
	{
		Vector3 vector = this.targetDestination.position - base.transform.position;
		vector.y = 0f;
		if (vector.magnitude > 0.1f)
		{
			return;
		}
		this.reachTargetPointCallback();
		this.targetDestination = null;
	}

	public void SetTargetDest(Transform target, Action callback = null)
	{
	}

	public void SetSpeed(float _speed)
	{
	}

	[ContextMenu("SetDest")]
	private void Test()
	{
		this.SetTargetDest(this.targetDestination, delegate
		{
			UnityEngine.Debug.LogError("reach point!");
		});
	}

	public void SetRotateLimit(float minX, float maxX, float minY, float maxY)
	{
		this.minXAxisValue = minX;
		this.maxXAxisValue = maxX;
		this.minYAxisValue = minY;
		this.maxYAxisValue = maxY;
	}

	public void SetStartRotate(float _x, float _y)
	{
		this.xAxisValue = _x;
		this.yAxisValue = _y;
		this.yAxisValue = Mathf.Clamp(this.yAxisValue, this.minYAxisValue, this.maxYAxisValue);
		this.xAxisValue = Mathf.Clamp(this.xAxisValue, this.minXAxisValue, this.maxXAxisValue);
		this.yAxis.localRotation = Quaternion.Euler(0f, -this.yAxisValue, 0f);
		this.curXAxisValue = this.xAxisValue;
		this.xAxis.localRotation = Quaternion.Euler(0f, 0f, this.curXAxisValue);
	}

	private void DoFire()
	{
		Ray ray = default(Ray);
		float x = this.legOfRightTriangle_sin - UnityEngine.Random.RandomRange(0f, this.legOfRightTriangle_sin * 2f);
		float y = this.legOfRightTriangle_sin - UnityEngine.Random.RandomRange(0f, this.legOfRightTriangle_sin * 2f);
		ray.origin = this.cameraAnchor.position;
		Vector3 a = this.cameraAnchor.TransformPoint(x, y, this.legOfRightTriangle_cos);
		ray.direction = a - this.cameraAnchor.position;
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.fireAudio.clip, false);
		Physics.SphereCast(ray, 0.05f, out this.hit, this.attackRadius, 134253056);
		if (this.hit.collider == null)
		{
			return;
		}
		GameObject gameObject = this.hit.collider.gameObject;
		if ((gameObject.layer == 9 || gameObject.layer == 27) && gameObject.name.StartsWith("E_"))
		{
			string[] array = gameObject.name.Split(new char[]
			{
				'|'
			});
			string enemyID = array[0];
			string value = array[1];
			Bone bone = (Bone)Enum.Parse(typeof(Bone), value);
			Enemy enemyByID = this.gameScene.GetEnemyByID(enemyID);
			float damage;
			if (enemyByID.GetEnemyProbability() == EnemyProbability.NORMAL)
			{
				damage = enemyByID.MaxHp * this.attackPercentNormal;
			}
			else
			{
				damage = enemyByID.MaxHp * this.attackPercentElite;
			}
			DamageProperty dp = new DamageProperty(damage, 30f, ray.direction * this.hitForce);
			enemyByID.OnHit(dp, WeaponType.PLAYER_CANNON, this.hit.point, bone);
		}
		else
		{
			this.sparksPool.CreateObject(this.hit.point, -ray.direction);
			Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.Hitwall);
		}
	}

	public Animator animator;

	public Transform xAxis;

	public Transform yAxis;

	public Transform cameraAnchor;

	public Transform gunRoot;

	public float xAxisValue;

	public float yAxisValue;

	public float curXAxisValue;

	[CNName("射击间隔")]
	public float shootInterval = 0.1f;

	[CNName("攻击力--小怪")]
	public float attackPercentNormal = 0.2f;

	[CNName("攻击力--精英")]
	public float attackPercentElite = 0.05f;

	[CNName("射程")]
	public float attackRadius = 100f;

	[CNName("散射角度")]
	public float randomAngel = 5f;

	[CNName("怪物受击受力大小")]
	public float hitForce = 5f;

	[CNName("移动目标点")]
	public Transform targetDestination;

	[CNName("枪火")]
	public GameObject gunFire;

	[CNName("开枪音效")]
	public AudioSource fireAudio;

	[CNName("启动音效")]
	public AudioSource startAudio;

	[Header("角度偏转限制")]
	[CNName("上下---大")]
	public float minXAxisValue;

	[CNName("上下---小")]
	public float maxXAxisValue;

	[CNName("左右---大")]
	public float minYAxisValue;

	[CNName("左右---小")]
	public float maxYAxisValue;

	[CNName("射击震动--时间")]
	public float interval;

	[CNName("射击震动--幅度")]
	public float shotRange;

	[CNName("缩放节点")]
	public Transform scaleRoot;

	protected Vector3 shootShakeRange = Vector3.zero;

	protected Vector3 shootShakeRootPos;

	protected float hypotenuse = 10f;

	protected float legOfRightTriangle_sin;

	protected float legOfRightTriangle_cos;

	protected bool isShooting;

	protected float aniTargetSpeed;

	protected float aniSpeed;

	protected const string AniSpeedKey = "AniSpeed";

	protected RaycastHit hit;

	protected GameScene gameScene;

	private TimeScheduler shootScheduler;

	private TimeScheduler detectPosition;

	private ObjectPool sparksPool = new ObjectPool();

	private Quaternion startQuaternion;

	private Action reachTargetPointCallback;

	protected SceneNPC bindNPC;

	protected TimeScheduler shootShakeScheduler;
}
