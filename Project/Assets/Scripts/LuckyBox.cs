using System;
using UnityEngine;
using Zombie3D;

public class LuckyBox : MonoBehaviour
{
	private void Awake()
	{
		this.player = GameApp.GetInstance().GetGameScene().GetPlayer();
	}

	private void Update()
	{
		Vector3 vector = this.player.GetTransform().InverseTransformPoint(base.transform.position);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Player")
		{
		}
	}

	public BuffType bType;

	protected Player player;
}
