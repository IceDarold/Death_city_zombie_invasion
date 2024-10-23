using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoneInfo : MonoBehaviour
{
	public GameObject CombineMesh(SkinnedMeshRenderer[] smrs, GameObject root)
	{
		CombineInstance[] array = new CombineInstance[smrs.Length];
		Material[] array2 = new Material[smrs.Length];
		Texture2D[] array3 = new Texture2D[smrs.Length];
		this.combineMesh = new GameObject("combineMesh");
		this.combineMesh.transform.parent = root.transform;
		SkinnedMeshRenderer skinnedMeshRenderer = this.combineMesh.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		skinnedMeshRenderer.receiveShadows = false;
		List<Transform> list = new List<Transform>();
		int i = 0;
		int num = smrs.Length;
		while (i < num)
		{
			if (!(smrs[i].transform == root.transform))
			{
				Mesh mesh = this.CreatMeshWithMesh(smrs[i].sharedMesh);
				list.AddRange(smrs[i].bones);
				array[i].mesh = mesh;
				array[i].transform = smrs[i].transform.localToWorldMatrix;
			}
			i++;
		}
		Mesh mesh2 = new Mesh();
		mesh2.CombineMeshes(array, true, false);
		skinnedMeshRenderer.bones = list.ToArray();
		skinnedMeshRenderer.rootBone = root.transform;
		skinnedMeshRenderer.sharedMesh = mesh2;
		skinnedMeshRenderer.sharedMaterial = smrs[0].sharedMaterial;
		skinnedMeshRenderer.updateWhenOffscreen = true;
		for (int j = 0; j < smrs.Length; j++)
		{
			smrs[j].enabled = false;
		}
		return this.combineMesh;
	}

	public void CombineMesh(GameObject root)
	{
		SkinnedMeshRenderer[] componentsInChildren = root.GetComponentsInChildren<SkinnedMeshRenderer>();
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		Material[] array2 = new Material[componentsInChildren.Length];
		Texture2D[] array3 = new Texture2D[componentsInChildren.Length];
		SkinnedMeshRenderer skinnedMeshRenderer = new GameObject("combineMesh")
		{
			transform = 
			{
				parent = root.transform
			}
		}.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		skinnedMeshRenderer.receiveShadows = false;
		List<Transform> list = new List<Transform>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			if (!(componentsInChildren[i].transform == root.transform))
			{
				Mesh mesh = this.CreatMeshWithMesh(componentsInChildren[i].sharedMesh);
				list.AddRange(componentsInChildren[i].bones);
				array[i].mesh = mesh;
				array[i].transform = componentsInChildren[i].transform.localToWorldMatrix;
			}
			i++;
		}
		Mesh mesh2 = new Mesh();
		mesh2.CombineMeshes(array, true, false);
		skinnedMeshRenderer.bones = list.ToArray();
		skinnedMeshRenderer.rootBone = root.transform;
		skinnedMeshRenderer.sharedMesh = mesh2;
		skinnedMeshRenderer.sharedMaterial = componentsInChildren[0].sharedMaterial;
		skinnedMeshRenderer.updateWhenOffscreen = true;
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].enabled = false;
		}
	}

	private Mesh CreatMeshWithMesh(Mesh mesh)
	{
		return new Mesh
		{
			vertices = mesh.vertices,
			name = mesh.name,
			uv = mesh.uv,
			uv2 = mesh.uv2,
			bindposes = mesh.bindposes,
			boneWeights = mesh.boneWeights,
			bounds = mesh.bounds,
			colors = mesh.colors,
			colors32 = mesh.colors32,
			normals = mesh.normals,
			subMeshCount = mesh.subMeshCount,
			tangents = mesh.tangents,
			triangles = mesh.triangles
		};
	}

	public void SetArmorsAndWeapons(int armorID, int weaponID)
	{
		for (int i = 0; i < this.armors.Count; i++)
		{
			for (int j = 0; j < this.armors[i].armors.Length; j++)
			{
				foreach (GameObject gameObject in this.armors[i].armors)
				{
					gameObject.SetActive(i == armorID);
				}
			}
		}
		for (int l = 0; l < this.weapons.Length; l++)
		{
			this.weapons[l].SetActive(l == weaponID);
		}
	}

	[HideInInspector]
	public GameObject[] bones = new GameObject[11];

	[SerializeField]
	[Header("特殊骨骼")]
	public GameObject[] specialBones;

	[HideInInspector]
	public SkinnedMeshRenderer[] meshs = new SkinnedMeshRenderer[11];

	[SerializeField]
	[Header("记录骨骼的原始名称")]
	public string[] boneNames = new string[11];

	[Space]
	[SerializeField]
	[Header("特殊骨骼的名称")]
	public string[] specialBoneNames;

	protected GameObject combineMesh;

	[SerializeField]
	[Header("护甲")]
	public List<EnemyArmor> armors;

	[SerializeField]
	[Header("武器")]
	public GameObject[] weapons;
}
