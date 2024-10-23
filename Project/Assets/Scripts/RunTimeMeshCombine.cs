using System;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeMeshCombine : SceneBindAble
{
	public bool ContainObject(string name)
	{
		for (int i = 0; i < this.allObjects.Count; i++)
		{
			if (this.allObjects[i].childname == name)
			{
				return true;
			}
		}
		return false;
	}

	public void AddObject(GameObject obj)
	{
		MeshRenderer component = obj.GetComponent<MeshRenderer>();
		BakedLightInfo item = new BakedLightInfo(obj.name, component.lightmapIndex, component.lightmapScaleOffset, string.Empty);
		this.allObjects.Add(item);
	}

	public void Load(string path, int level)
	{
		if (!base.CanUse(level))
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.allObjects.Count; i++)
		{
			GameObject gameObject = base.transform.Find(this.allObjects[i].childname).gameObject;
			if (gameObject != null)
			{
				list.Add(gameObject);
				gameObject.isStatic = false;
			}
			else
			{
				gameObject = (UnityEngine.Object.Instantiate(Resources.Load(path + "/" + this.allObjects[i].childname), base.transform) as GameObject);
				list.Add(gameObject);
				Renderer component = gameObject.GetComponent<Renderer>();
				component.lightmapIndex = this.allObjects[i].lightMapIndex;
				component.lightmapScaleOffset = this.allObjects[i].lightmapTillingOffset;
				gameObject.isStatic = false;
			}
		}
	}

	private void CombineMesh(List<GameObject> allObjs)
	{
		GameObject[] array = allObjs.ToArray();
		MeshFilter[] array2 = new MeshFilter[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = array[i].GetComponent<MeshFilter>();
		}
		CombineInstance[] array3 = new CombineInstance[array2.Length];
		MeshRenderer[] array4 = new MeshRenderer[array.Length];
		for (int j = 0; j < array4.Length; j++)
		{
			array4[j] = array[j].GetComponent<MeshRenderer>();
		}
		Material[] array5 = new Material[array4.Length];
		GameObject gameObject = new GameObject("CombinedMesh");
		gameObject.transform.parent = base.transform;
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		for (int k = 0; k < array4.Length; k++)
		{
			Mesh sharedMesh = array2[k].sharedMesh;
			sharedMesh.uv = array2[k].sharedMesh.uv;
			array3[k].mesh = sharedMesh;
			array3[k].transform = array2[k].transform.localToWorldMatrix;
			array2[k].gameObject.SetActive(false);
		}
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array3);
		meshFilter.mesh = mesh;
		meshRenderer.sharedMaterials = array4[0].sharedMaterials;
		gameObject.isStatic = true;
	}

	public void DetachAllChildren()
	{
		int i = 0;
		int count = this.allObjects.Count;
		while (i < count)
		{
			if (!(base.transform.Find(this.allObjects[i].childname) == null))
			{
				GameObject gameObject = base.transform.Find(this.allObjects[i].childname).gameObject;
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			i++;
		}
	}

	public List<BakedLightInfo> allObjects = new List<BakedLightInfo>();
}
