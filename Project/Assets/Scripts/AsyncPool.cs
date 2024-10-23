using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncPool : MonoBehaviour
{
	public static AsyncPool Init(string poolName, string path)
	{
		GameObject gameObject = new GameObject(poolName);
		AsyncPool asyncPool = gameObject.AddComponent<AsyncPool>();
		asyncPool.prefabPath = path;
		return asyncPool;
	}

	public void CreateObject(Vector3 position, Quaternion rotation, Action<GameObject> loadCallBack)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (!this.objects[i].activeSelf)
			{
				this.objects[i].SetActive(true);
				this.objects[i].transform.position = position;
				this.objects[i].transform.rotation = rotation;
				loadCallBack(this.objects[i]);
				return;
			}
		}
		if (this.template != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.template, position, rotation);
			gameObject.transform.parent = base.transform;
			this.objects.Add(gameObject);
			loadCallBack(gameObject);
		}
		else
		{
			base.StartCoroutine(this.LoadTemplate(position, rotation, loadCallBack));
		}
	}

	private IEnumerator LoadTemplate(Vector3 position, Quaternion rotation, Action<GameObject> cb)
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(this.prefabPath);
		while (!resourceRequest.isDone)
		{
			yield return null;
		}
		this.template = (resourceRequest.asset as GameObject);
		this.CreateObject(position, rotation, cb);
		yield break;
	}

	protected GameObject template;

	protected List<GameObject> objects = new List<GameObject>();

	public string prefabPath;
}
