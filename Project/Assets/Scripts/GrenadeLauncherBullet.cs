using System;
using UnityEngine;
using Zombie3D;

[ExecuteInEditMode]
public class GrenadeLauncherBullet : MonoBehaviour
{
	public void Init(float _damage)
	{
		this.damage = _damage;
		this.startPos = base.transform.position;
		float x = base.transform.rotation.eulerAngles.x;
		this.speedH = Mathf.Cos(-x * 0.0174532924f) * this.startSpeed;
		this.speedV = Mathf.Sin(-x * 0.0174532924f) * this.startSpeed;
		Vector3 forward = new Vector3(base.transform.forward.x, 0f, base.transform.forward.z);
		this.cameraLocal2World = Matrix4x4.TRS(base.transform.position, Quaternion.LookRotation(forward), Vector3.one);
		base.gameObject.SetActive(true);
		this.startMoveTime = Time.time;
		this.canMove = true;
	}

	private void DoExplode()
	{
		base.gameObject.SetActive(false);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Singleton<ResourceConfigScript>.Instance.bombParticle, base.transform.position, Quaternion.identity);
		GameApp.GetInstance().GetGameScene().Bomb2Enemies(this.damage, base.transform.position, 400f, 5f, WeaponType.GrenadeRifle, true, null);
		Singleton<GameAudioManager>.Instance.PlayGlobalEffect(GlobalEffect.GrenadeRifeBomb);
	}

	public void Update()
	{
		if (!this.canMove)
		{
			return;
		}
		float num = Time.time - this.startMoveTime;
		float z = this.speedH * num;
		float y = this.speedV * num - this.gravity * num * num / 2f;
		Vector3 vector = new Vector3(0f, y, z) + base.transform.position;
		Vector3 position = this.cameraLocal2World.MultiplyPoint3x4(new Vector3(0f, y, z));
		base.transform.position = position;
	}

	public void OnTriggerEnter(Collider other)
	{
		this.canMove = false;
		this.DoExplode();
	}

	public float startSpeed = 1f;

	public float gravity = 1f;

	public float radius = 1f;

	[CNName("爆炸力")]
	public float force = 400f;

	private Matrix4x4 cameraLocal2World;

	private float speedH;

	private float speedV;

	private bool canMove;

	private float startMoveTime;

	private Vector3 startPos;

	private float damage;
}
