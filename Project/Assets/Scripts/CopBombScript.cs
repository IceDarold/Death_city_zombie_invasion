using System;
using UnityEngine;
using Zombie3D;

public class CopBombScript : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.startTime > this.explodeTime)
		{
			ResourceConfigScript resourceConfig = GameApp.GetInstance().GetResourceConfig();
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			float num = Mathf.Sqrt((base.transform.position - player.GetTransform().position).sqrMagnitude);
			if (num < this.radius)
			{
				Ray ray = new Ray(base.transform.position, player.GetTransform().position - base.transform.position);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, num, 67840))
				{
					UnityEngine.Debug.Log(raycastHit.collider.gameObject.name);
					if (raycastHit.collider.gameObject.name == "Player")
					{
					}
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected float startTime;

	public float explodeTime = 4f;

	public float radius = 5f;

	public float damage = 20f;

	public Vector3 speed;
}
