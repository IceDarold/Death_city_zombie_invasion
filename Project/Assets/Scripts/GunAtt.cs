using System;
using UnityEngine;

public class GunAtt : MonoBehaviour
{
	private void Awake()
	{
		this.Caculate_RandomRange_And_ZorderDistances();
	}

	private void Caculate_RandomRange_And_ZorderDistances()
	{
		this.legOfRightTriangle_sin = this.hypotenuse * Mathf.Sin(this.randomAngel * 0.0174532924f);
		this.legOfRightTriangle_cos = this.hypotenuse * Mathf.Cos(this.randomAngel * 0.0174532924f);
	}

	public void RandomFireDetectedRay(ref Ray ray, Transform rayOrigionTrans)
	{
		float x = this.legOfRightTriangle_sin - UnityEngine.Random.RandomRange(0f, this.legOfRightTriangle_sin * 2f);
		float y = this.legOfRightTriangle_sin - UnityEngine.Random.RandomRange(0f, this.legOfRightTriangle_sin * 2f);
		Vector3 a = rayOrigionTrans.TransformPoint(x, y, this.legOfRightTriangle_cos);
		ray.direction = a - rayOrigionTrans.position;
	}

	public void OnPickOutCharger(Transform playerHand, out GameObject pickOutCharger, out GameObject pickOnCharger)
	{
		pickOutCharger = UnityEngine.Object.Instantiate<GameObject>(this.charger, this.charger.transform.position, this.charger.transform.rotation, playerHand);
		this.charger.SetActive(false);
		pickOnCharger = UnityEngine.Object.Instantiate<GameObject>(this.charger, playerHand);
		pickOnCharger.transform.localPosition = this.pickOnChargerPosition;
		pickOnCharger.transform.localRotation = Quaternion.Euler(this.pickOnChargerRotation);
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.uninstallCharger, false);
	}

	public void OnPickOnCharger()
	{
		if (this.charger != null)
		{
			this.charger.SetActive(true);
		}
		Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.installCharger, false);
	}

	public void Reset()
	{
		this.animator.SetTrigger("Reset");
	}

	public void PlayGunAni(int _aniID)
	{
		this.animator.SetTrigger(string.Format("AniID{0}", _aniID));
		if (this.allAudioClips.Length > _aniID)
		{
			Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.allAudioClips[_aniID], false);
		}
	}

	[Space]
	[CNName("发射的子弹数")]
	public int oneShotBullets = 1;

	[CNName("多子弹偏转角")]
	public float angelOffset;

	[CNName("多子弹上下随机偏转")]
	public float angelRandom;

	[CNName("相机后移距离")]
	public float shotgunShootOffset = 1f;

	[Header("射击相关参数:")]
	[CNName("静止偏移")]
	public float originalOffset = 2.5f;

	[CNName("射击偏移")]
	public float shootOffsetMin = 6f;

	[CNName("射击偏移抖动区间")]
	public float shootOffsetMax = 9f;

	[CNName("不稳定扩散比例")]
	public float scaleRatio = 1.2f;

	public GunShootShakeInfo shootShakeInfo;

	public GunStability stability;

	[Space]
	[CNName("散射随机角度")]
	public float randomAngel = 5f;

	[CNName("枪械ID-动画用")]
	public int gunID;

	[CNName("枪的射击力度")]
	public float hitForce = 5f;

	[CNName("动画逐个上弹")]
	public bool reloadOneByOne;

	[CNName("弹夹")]
	public GameObject charger;

	[CNName("枪械动画")]
	public Animator animator;

	[CNName("瞄准圆柱半径")]
	public float aimRadius = 0.1f;

	[CNName("射击圆柱半径")]
	public float shootRadius = 0.1f;

	[Space]
	[CNName(0f, 1f, "射击震动--平衡")]
	public float balance;

	[CNName("射击震动--时间")]
	public float shakeTime;

	[CNName("射击震动--幅度")]
	public float shakeRange;

	[SerializeField]
	[Header("各种音效")]
	public AudioClip[] allAudioClips;

	[CNName("拆弹夹")]
	public AudioClip uninstallCharger;

	[CNName("装弹夹")]
	public AudioClip installCharger;

	private const string AniID = "AniID";

	public Vector3 pickOnChargerPosition;

	public Vector3 pickOnChargerRotation;

	protected float hypotenuse = 10f;

	protected float legOfRightTriangle_sin;

	protected float legOfRightTriangle_cos;
}
