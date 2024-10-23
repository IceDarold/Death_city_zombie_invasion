using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	public void Init(string poolName, GameObject prefab, int initNum, float life)
	{
		this.objects = new List<GameObject>();
		this.transforms = new List<Transform>();
		this.createdTime = new List<float>();
		this.life = life;
		this.folderObject = new GameObject(poolName);
		this.templateObject = prefab;
		for (int i = 0; i < initNum; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			this.objects.Add(gameObject);
			this.transforms.Add(gameObject.transform);
			this.createdTime.Add(0f);
			gameObject.SetActive(false);
			gameObject.transform.parent = this.folderObject.transform;
			gameObject.SetActive(false);
		}
	}

	public GameObject ShowObject(Vector3 position, Quaternion rotation)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (!this.objects[i].activeSelf)
			{
				this.objects[i].SetActive(true);
				this.objects[i].transform.position = position;
				this.objects[i].transform.rotation = rotation;
				this.createdTime[i] = Time.time;
				return this.objects[i];
			}
		}
		return null;
	}

	public GameObject CreateObject(Vector3 position, Quaternion rotation)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (!this.objects[i].activeSelf)
			{
				this.objects[i].SetActive(true);
				this.objects[i].transform.position = position;
				this.objects[i].transform.rotation = rotation;
				this.createdTime[i] = Time.time;
				return this.objects[i];
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.templateObject);
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		this.objects.Add(gameObject);
		this.transforms.Add(gameObject.transform);
		this.createdTime.Add(0f);
		gameObject.name = this.objects[0].name;
		gameObject.transform.parent = this.folderObject.transform;
		gameObject.SetActive(true);
		return gameObject;
	}

	public T CreateObject<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
	{
		return this.CreateObject(position, rotation).GetComponent<T>();
	}

	public GameObject CreateObject(Vector3 position, Vector3 lookAtRotation)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (!this.objects[i].activeSelf)
			{
				this.objects[i].SetActive(true);
				this.transforms[i].position = position;
				this.objects[i].transform.rotation = Quaternion.LookRotation(lookAtRotation);
				this.createdTime[i] = Time.time;
				return this.objects[i];
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.templateObject);
		this.objects.Add(gameObject);
		this.transforms.Add(gameObject.transform);
		this.createdTime.Add(0f);
		gameObject.name = this.objects[0].name;
		gameObject.transform.parent = this.folderObject.transform;
		gameObject.SetActive(true);
		return gameObject;
	}

	public void AutoDestruct()
	{
		if (this.objects == null)
		{
			return;
		}
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (this.objects[i].activeSelf && Time.time - this.createdTime[i] > this.life)
			{
				this.objects[i].SetActive(false);
			}
		}
	}

	public GameObject DeleteObject(GameObject obj)
	{
		obj.SetActive(false);
		return obj;
	}

	protected List<GameObject> objects;

	protected List<Transform> transforms;

	protected List<float> createdTime;

	protected float life;

	protected bool hasAnimation;

	protected bool hasParticleEmitter;

	protected GameObject folderObject;

	protected GameObject templateObject;
}
