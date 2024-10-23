using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CombineMeshes : MonoBehaviour
{
	private void disableThis()
	{
		base.enabled = false;
	}

	private void ableThis()
	{
		base.enabled = true;
	}

	private void CombineWithLightMap()
	{
		MeshFilter[] componentsInChildren = base.transform.GetComponentsInChildren<MeshFilter>();
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		this.c_verts = null;
		this.c_triangles = null;
		this.c_uvs = null;
		this.c_uv2s = null;
		this.c_tangens = null;
		int num = 0;
		int lightmapIndex = -1;
		Matrix4x4 matrix4x = Matrix4x4.identity;
		Vector3 vector = Vector3.zero;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != base.gameObject)
			{
				Mesh sharedMesh = componentsInChildren[i].sharedMesh;
				if (this.c_verts == null)
				{
					this.c_verts = new Vector3[sharedMesh.vertices.Length * componentsInChildren.Length];
					this.c_uvs = new Vector2[sharedMesh.vertices.Length * componentsInChildren.Length];
					this.c_uv2s = new Vector2[sharedMesh.vertices.Length * componentsInChildren.Length];
					this.c_triangles = new int[sharedMesh.triangles.Length * componentsInChildren.Length];
					this.c_tangens = new Vector4[sharedMesh.vertices.Length * componentsInChildren.Length];
				}
				GameObject gameObject = componentsInChildren[i].gameObject;
				matrix4x = worldToLocalMatrix * gameObject.transform.localToWorldMatrix;
				if (gameObject.GetComponent<Renderer>().lightmapIndex != -1)
				{
					lightmapIndex = gameObject.GetComponent<Renderer>().lightmapIndex;
				}
				Vector4 lightmapScaleOffset = gameObject.GetComponent<Renderer>().lightmapScaleOffset;
				Vector2 vector2 = new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
				Vector2 a = new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);
				Vector3[] vertices = sharedMesh.vertices;
				Vector2[] uv = sharedMesh.uv;
				Vector2[] uv2 = sharedMesh.uv2;
				int[] triangles = sharedMesh.triangles;
				Vector4[] tangents = sharedMesh.tangents;
				int num4 = sharedMesh.vertices.Length;
				for (int j = 0; j < num4; j++)
				{
					vector = matrix4x.MultiplyPoint3x4(vertices[j]);
					this.c_verts[num2] = vector;
					if (uv.Length > j)
					{
						this.c_uvs[num2] = uv[j];
					}
					if (tangents.Length > j)
					{
						this.c_tangens[num2] = tangents[j];
					}
					if (uv2.Length > j)
					{
						Vector2 b;
						b.x = vector2.x * uv2[j].x;
						b.y = vector2.y * uv2[j].y;
						this.c_uv2s[num2] = a + b;
					}
					num2++;
				}
				num4 = 0;
				if (sharedMesh.triangles != null)
				{
					num4 = sharedMesh.triangles.Length;
				}
				for (int k = 0; k < num4; k++)
				{
					this.c_triangles[num3] = triangles[k] + num;
					num3++;
				}
				num += vertices.Length;
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = this.c_verts;
		mesh.triangles = this.c_triangles;
		mesh.tangents = this.c_tangens;
		mesh.uv = this.c_uvs;
		mesh.uv2 = this.c_uv2s;
		mesh.RecalculateNormals();
		base.GetComponent<MeshFilter>().sharedMesh = mesh;
		base.GetComponent<MeshRenderer>().lightmapIndex = lightmapIndex;
		this.c_verts = null;
		this.c_triangles = null;
		this.c_uvs = null;
		this.c_uv2s = null;
		GC.Collect();
		UnityEngine.Object.Destroy(this);
	}

	private void combineMesh()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.position += base.transform.position;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		this.meshFilters = base.GetComponentsInChildren<MeshFilter>();
		this.combine = new CombineInstance[this.meshFilters.Length - 1];
		int num = 0;
		Vector4 lightmapScaleOffset = Vector4.zero;
		int num2 = -1;
		for (int i = 0; i < this.meshFilters.Length; i++)
		{
			if (!(this.meshFilters[i].sharedMesh == null))
			{
				if (num2 == -1)
				{
					num2 = this.meshFilters[i].gameObject.GetComponent<Renderer>().lightmapIndex;
					lightmapScaleOffset = this.meshFilters[i].gameObject.GetComponent<Renderer>().lightmapScaleOffset;
				}
				this.combine[num].mesh = this.meshFilters[i].sharedMesh;
				this.combine[num].transform = this.meshFilters[i].transform.localToWorldMatrix;
				this.meshFilters[i].GetComponent<Renderer>().enabled = false;
				num++;
			}
		}
		this.mMesh = new Mesh();
		this.mMesh.CombineMeshes(this.combine, true, true);
		base.GetComponent<Renderer>().material = this.meshFilters[1].GetComponent<Renderer>().sharedMaterial;
		base.GetComponent<MeshFilter>().mesh = this.mMesh;
		base.gameObject.GetComponent<Renderer>().lightmapIndex = num2;
		base.gameObject.GetComponent<Renderer>().lightmapScaleOffset = lightmapScaleOffset;
	}

	private void Awake()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.isCombined)
		{
			return;
		}
		this.isCombined = true;
		this.CombineWithLightMap();
	}

	private void OnEnable()
	{
		if ((base.gameObject.GetComponent<MeshCollider>() != null || base.gameObject.GetComponent<BoxCollider>() != null) && base.gameObject.GetComponent<MeshRenderer>() != null)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	[ContextMenu("GetChildMaterial")]
	public void GetChildMaterial()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (componentsInChildren.Length > 1)
		{
			component.material = componentsInChildren[1].sharedMaterial;
			MonoBehaviour.print("添加完毕");
		}
	}

	private MeshFilter[] meshFilters;

	private CombineInstance[] combine;

	private List<Material> materialList = new List<Material>();

	public bool isCombined;

	private Mesh mMesh;

	private Vector3[] c_verts;

	private Vector4[] c_tangens;

	private int[] c_triangles;

	private Vector2[] c_uvs;

	private Vector2[] c_uv2s;
}
