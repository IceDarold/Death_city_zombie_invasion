using System;
using SplineUtilities;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("SuperSplines/Spline Mesh")]
public class SplineMesh : MonoBehaviour
{
	public Mesh BentMesh
	{
		get
		{
			return this.ReturnMeshReference();
		}
	}

	public bool IsSubSegment
	{
		get
		{
			return this.splineSegment != -1;
		}
	}

	private void Start()
	{
		if (this.spline != null)
		{
			this.spline.UpdateSpline();
		}
		this.UpdateMesh();
	}

	private void OnEnable()
	{
		if (this.spline != null)
		{
			this.spline.UpdateSpline();
		}
		this.UpdateMesh();
	}

	private void LateUpdate()
	{
		if (this.autoUpdater.Update())
		{
			this.UpdateMesh();
		}
	}

	public void UpdateMesh()
	{
		this.SetupMeshFilter();
		this.bentMesh.Clear();
		if (this.baseMesh == null || this.spline == null || this.segmentCount <= 0)
		{
			return;
		}
		SplineMesh.MeshData meshData = new SplineMesh.MeshData(this.baseMesh);
		SplineMesh.MeshData meshData2 = new SplineMesh.MeshData(meshData, this.segmentCount);
		SplineSegment splineSegment = null;
		if (this.IsSubSegment)
		{
			splineSegment = this.spline.SplineSegments[this.splineSegment];
		}
		for (int i = 0; i < this.segmentCount; i++)
		{
			float param;
			float param2;
			if (!this.IsSubSegment)
			{
				param = (float)i / (float)this.segmentCount;
				param2 = (float)(i + 1) / (float)this.segmentCount;
			}
			else
			{
				param = splineSegment.ConvertSegmentToSplineParamter((float)i / (float)this.segmentCount);
				param2 = splineSegment.ConvertSegmentToSplineParamter((float)(i + 1) / (float)this.segmentCount);
			}
			this.GenerateBentMensh(i, param, param2, meshData, meshData2);
		}
		meshData2.AssignToMesh(this.bentMesh);
	}

	private void GenerateBentMensh(int segmentIdx, float param0, float param1, SplineMesh.MeshData meshDataBase, SplineMesh.MeshData meshDataNew)
	{
		Vector3 a = this.spline.transform.InverseTransformPoint(this.spline.GetPositionOnSpline(param0));
		Vector3 b = this.spline.transform.InverseTransformPoint(this.spline.GetPositionOnSpline(param1));
		Quaternion a2 = this.spline.GetOrientationOnSpline(param0) * Quaternion.Inverse(this.spline.transform.rotation);
		Quaternion b2 = this.spline.GetOrientationOnSpline(param1) * Quaternion.Inverse(this.spline.transform.rotation);
		Quaternion rotation = Quaternion.identity;
		Vector3 vector = Vector3.zero;
		Vector2 a3 = Vector2.zero;
		int num = meshDataBase.VertexCount * segmentIdx;
		int i = 0;
		while (i < meshDataBase.VertexCount)
		{
			vector = meshDataBase.vertices[i];
			a3 = meshDataBase.uvCoord[i];
			float t = vector.z + 0.5f;
			rotation = Quaternion.Lerp(a2, b2, t);
			vector.Scale(new Vector3(this.xyScale.x, this.xyScale.y, 0f));
			vector = rotation * vector;
			vector += Vector3.Lerp(a, b, t);
			meshDataNew.vertices[num] = vector;
			if (meshDataBase.HasNormals)
			{
				meshDataNew.normals[num] = rotation * meshDataBase.normals[i];
			}
			if (meshDataBase.HasTangents)
			{
				meshDataNew.tangents[num] = rotation * meshDataBase.tangents[i];
			}
			if (this.uvMode == SplineMesh.UVMode.Normal)
			{
				a3.y = Mathf.Lerp(param0, param1, t);
			}
			else if (this.uvMode == SplineMesh.UVMode.Swap)
			{
				a3.x = Mathf.Lerp(param0, param1, t);
			}
			meshDataNew.uvCoord[num] = Vector2.Scale(a3, this.uvScale);
			i++;
			num++;
		}
		for (int j = 0; j < meshDataBase.TriangleCount; j++)
		{
			meshDataNew.triangles[j + segmentIdx * meshDataBase.TriangleCount] = meshDataBase.triangles[j] + meshDataBase.VertexCount * segmentIdx;
		}
	}

	private void SetupMeshFilter()
	{
		if (this.bentMesh == null)
		{
			this.bentMesh = new Mesh();
			this.bentMesh.name = "BentMesh";
			this.bentMesh.hideFlags = HideFlags.HideAndDontSave;
		}
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component.sharedMesh != this.bentMesh)
		{
			component.sharedMesh = this.bentMesh;
		}
		MeshCollider component2 = base.GetComponent<MeshCollider>();
		if (component2 != null)
		{
			component2.sharedMesh = null;
			component2.sharedMesh = this.bentMesh;
		}
	}

	private Mesh ReturnMeshReference()
	{
		return this.bentMesh;
	}

	public Spline spline;

	public AutomaticUpdater autoUpdater;

	public Mesh baseMesh;

	public int segmentCount = 50;

	public SplineMesh.UVMode uvMode;

	public Vector2 uvScale = Vector2.one;

	public Vector2 xyScale = Vector2.one;

	public int splineSegment = -1;

	private Mesh bentMesh;

	private class MeshData
	{
		public MeshData(Mesh mesh)
		{
			this.vertices = mesh.vertices;
			this.normals = mesh.normals;
			this.tangents = mesh.tangents;
			this.uvCoord = mesh.uv;
			this.triangles = mesh.triangles;
			this.bounds = mesh.bounds;
		}

		public MeshData(SplineMesh.MeshData mData, int segmentCount)
		{
			this.vertices = new Vector3[mData.vertices.Length * segmentCount];
			this.uvCoord = new Vector2[mData.uvCoord.Length * segmentCount];
			this.normals = new Vector3[mData.normals.Length * segmentCount];
			this.tangents = new Vector4[mData.tangents.Length * segmentCount];
			this.triangles = new int[mData.triangles.Length * segmentCount];
		}

		public bool HasNormals
		{
			get
			{
				return this.normals.Length > 0;
			}
		}

		public bool HasTangents
		{
			get
			{
				return this.tangents.Length > 0;
			}
		}

		public int VertexCount
		{
			get
			{
				return this.vertices.Length;
			}
		}

		public int TriangleCount
		{
			get
			{
				return this.triangles.Length;
			}
		}

		public void AssignToMesh(Mesh mesh)
		{
			mesh.vertices = this.vertices;
			mesh.uv = this.uvCoord;
			if (this.HasNormals)
			{
				mesh.normals = this.normals;
			}
			if (this.HasTangents)
			{
				mesh.tangents = this.tangents;
			}
			mesh.triangles = this.triangles;
		}

		public Vector3[] vertices;

		public Vector2[] uvCoord;

		public Vector3[] normals;

		public Vector4[] tangents;

		public int[] triangles;

		public Bounds bounds;
	}

	public enum UVMode
	{
		Normal,
		Swap,
		DontInterpolate
	}
}
