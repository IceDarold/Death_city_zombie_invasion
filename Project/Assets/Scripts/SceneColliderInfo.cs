using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneColliderInfo : BindLevel
{
	private SceneColliderInfo()
	{
	}

	public SceneColliderInfo(GameObject _obj)
	{
		this.obj = _obj;
	}

	public void CreateSceneColliders()
	{
		int i = 0;
		int count = this.colliderInfos.Count;
		while (i < count)
		{
			ColliderInfo colliderInfo = this.colliderInfos[i];
			GameObject gameObject = new GameObject(colliderInfo.name);
			gameObject.transform.parent = this.obj.transform;
			gameObject.transform.localPosition = colliderInfo.position;
			gameObject.transform.localRotation = Quaternion.Euler(colliderInfo.rotation);
			gameObject.transform.localScale = colliderInfo.scale;
			gameObject.layer = colliderInfo.layer;
			if (colliderInfo.type == ColliderType.BOX)
			{
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				boxCollider.center = colliderInfo.center;
				boxCollider.size = colliderInfo.size;
			}
			else if (colliderInfo.type == ColliderType.CAPSULE)
			{
				CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider.center = colliderInfo.center;
				capsuleCollider.radius = colliderInfo.radius;
				capsuleCollider.height = colliderInfo.height;
				capsuleCollider.direction = colliderInfo.direction;
			}
			i++;
		}
	}

	public GameObject obj;

	public List<ColliderInfo> colliderInfos = new List<ColliderInfo>();
}
