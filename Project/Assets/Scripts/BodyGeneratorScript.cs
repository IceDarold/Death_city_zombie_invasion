using System;
using UnityEngine;

public class BodyGeneratorScript : MonoBehaviour
{
	private void Start()
	{
		this.rConfig = GameObject.Find("ResourceConfig").GetComponent<ResourceConfigScript>();
		this.timer.SetTimer(4f, false);
	}

	public void PlayDead()
	{
	}

	public void PlayBodyExlodeEffect()
	{
	}

	public void PlayBloodEffect()
	{
		GameObject deadBlood = this.rConfig.deadBlood;
		int num = UnityEngine.Random.Range(0, 100);
		float y = 0.02f;
		GameObject original;
		if (num > 50)
		{
			original = this.rConfig.deadFoorblood;
		}
		else
		{
			original = this.rConfig.deadFoorblood2;
			y = 0.01f;
		}
		UnityEngine.Object.Instantiate<GameObject>(deadBlood, base.transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
		UnityEngine.Object.Instantiate<GameObject>(original, new Vector3(base.transform.position.x, y, base.transform.position.z), Quaternion.Euler(270f, 0f, 0f));
	}

	private void Update()
	{
		if (this.timer.Ready())
		{
			this.PlayDead();
			this.timer.Do();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(base.transform.position, 1f);
	}

	public Timer timer = new Timer();

	public ResourceConfigScript rConfig;
}
