using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneObjectInfo : BindLevel
{
	public void CreateSceneGameObjects(GamePlotManager plotManager)
	{
		for (int i = 0; i < this.objectInfo.Count; i++)
		{
			GameObject gameObject = Resources.Load<GameObject>(this.objectInfo[i].prefabPath);
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError(this.objectInfo[i].prefabPath + "为空");
			}
			else
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, this.objectInfo[i].position, Quaternion.Euler(this.objectInfo[i].rotation), this.obj.transform);
				gameObject2.transform.localScale = this.objectInfo[i].scale;
				gameObject2.SetActive(this.objectInfo[i].visible);
				this.objectInfo[i].SetPlotRef(plotManager, gameObject2);
			}
		}
	}

	public GameObject obj;

	public List<ObjectInfo> objectInfo = new List<ObjectInfo>();
}
