using System;
using UnityEngine;

[ExecuteInEditMode]
public class ShowNormals : MonoBehaviour
{
	private void Update()
	{
		Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		Vector3[] normals = sharedMesh.normals;
		for (int i = 0; i < normals.Length; i++)
		{
			Vector3 vector = vertices[i];
			vector.x *= base.transform.localScale.x;
			vector.y *= base.transform.localScale.y;
			vector.z *= base.transform.localScale.z;
			vector += base.transform.position + this.bias;
			UnityEngine.Debug.DrawLine(vector, vector + normals[i] * this.length, Color.red);
		}
	}

	public float length = 1f;

	public Vector3 bias;
}
