using System;
using UnityEngine;

public class RayCastScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		GameObject gameObject = GameObject.Find("Begin");
		GameObject gameObject2 = GameObject.Find("End");
		Ray ray = new Ray(gameObject.transform.position, gameObject.transform.position - gameObject.transform.position);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, 2048))
		{
			UnityEngine.Debug.Log(gameObject.transform.position + "," + gameObject2.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		GameObject gameObject = GameObject.Find("Begin");
		GameObject gameObject2 = GameObject.Find("End");
		Ray ray = new Ray(gameObject.transform.position, gameObject.transform.position - gameObject.transform.position);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, 2048))
		{
			Gizmos.DrawLine(gameObject.transform.position, gameObject2.transform.position);
		}
	}

	public float life;
}
