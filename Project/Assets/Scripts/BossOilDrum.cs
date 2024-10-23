using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class BossOilDrum : MonoBehaviour, BreakAble
{
	public void Init(Vector3 _endPos, float _dmg2Player, float _bombRadius, float controlOffset, GameObject _warning, Camera _main)
	{
		this.endPos = _endPos;
		this.dmg2Player = _dmg2Player;
		this.bombRadius = _bombRadius;
		this.startPos = base.transform.position;
		Vector3 p = this.startPos + (this.endPos - this.startPos) / 2f + Vector3.up * controlOffset;
		this.moveBezier = new ThreePointBezier(this.startPos, p, this.endPos);
		this.rotateAxis = base.transform.forward * UnityEngine.Random.Range(0f, 1f) + base.transform.up * UnityEngine.Random.Range(0f, 1f) + base.transform.right * UnityEngine.Random.Range(0f, 1f);
		Vector3 vector = this.endPos - this.startPos;
		Vector2 vector2 = new Vector2(vector.x, vector.z);
		float magnitude = vector2.magnitude;
		this.bezierTime = magnitude / this.speed;
		this.startBezier = true;
		GameApp.GetInstance().GetGameScene().allBossOilDrums.Add(this);
		GameApp.GetInstance().GetGameScene().allBreakAbleDrums.Add(this);
		this.warning = _warning;
		this.warning.transform.localScale = new Vector3(this.bombRadius / 4f, 1f, this.bombRadius / 4f);
		this.mainCamera = _main;
		this.uiInGame = Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>();
		this.pointTag = this.uiInGame.GetPointTagRectTransform();
		this.uiCamera = Singleton<UiManager>.Instance.UiCamera;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	private IEnumerator SetPointTagTrue()
	{
		yield return new WaitForSeconds(0.3f);
		this.pointTag.gameObject.SetActive(true);
		yield break;
	}

	public void Update()
	{
		if (!this.startBezier)
		{
			return;
		}
		base.transform.position = this.moveBezier.GetPointAtTime(this.movePercent / this.bezierTime);
		if (this.movePercent / this.bezierTime >= 1f)
		{
			this.DoExplod(true);
			return;
		}
		this.movePercent += Time.deltaTime;
		base.transform.Rotate(this.rotateAxis, this.rotateSpeed * Time.deltaTime, Space.Self);
		Vector3 position = this.mainCamera.WorldToScreenPoint(base.transform.position);
		Vector3 position2 = this.uiCamera.ScreenToWorldPoint(position);
		this.pointTag.position = position2;
	}

	public void OnTriggerEnter(Collider other)
	{
	}

	private void DoExplod(bool hurt2Player)
	{
		this.pointTag.gameObject.SetActive(false);
		this.startBezier = false;
		base.gameObject.SetActive(false);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.woodExplode);
		gameObject.transform.position = base.transform.position;
		float magnitude = (GameApp.GetInstance().GetGameScene().GetPlayer().GetTransform().position - base.transform.position).magnitude;
		this.warning.SetActive(false);
		UnityEngine.Object.Destroy(this.warning);
		UnityEngine.Object.Destroy(base.gameObject);
		GameApp.GetInstance().GetGameScene().allBossOilDrums.Remove(this);
		GameApp.GetInstance().GetGameScene().allBreakAbleDrums.Remove(this);
		if (!hurt2Player)
		{
			return;
		}
		Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
		Vector3 position = player.GetTransform().position;
		if ((position - this.endPos).magnitude > this.bombRadius)
		{
			return;
		}
		player.OnHit(this.dmg2Player, true, AttackType.BOMB);
	}

	public void OnHit(DamageProperty dp, WeaponType weaponType, Vector3 pos, Bone _bone)
	{
		this.hp -= dp.damage;
		if (this.hp <= 0f)
		{
			this.DoExplod(false);
		}
	}

	public List<WeaponHitInfo> SimulateKillEnemy()
	{
		return null;
	}

	[CNName("飞行速度--水平面")]
	public float speed = 1f;

	[CNName("旋转速度")]
	public float rotateSpeed = 180f;

	[CNName("生命值")]
	public float hp = 100f;

	[CNName("爆炸范围")]
	public float attackRange = 2f;

	private Vector3 endPos;

	private Vector3 startPos;

	private Vector3 rotateAxis;

	private float dmg2Player;

	private float movePercent;

	private float bezierTime;

	private float bombRadius = 1f;

	private ThreePointBezier moveBezier;

	private bool startBezier;

	protected GameObject warning;

	public const float Warning_template = 4f;

	protected Camera mainCamera;

	protected RectTransform pointTag;

	protected InGamePage uiInGame;

	protected Camera uiCamera;
}
