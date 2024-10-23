using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Parabola : MonoBehaviour
{
	public void OnRenderObject()
	{
		this.CreateLineMaterial();
		this.m_LineMat.SetPass(0);
		Vector3 position = base.transform.position;
		Vector3 b = base.transform.rotation * Vector3.forward * 0.2f;
		Vector3 vector = position;
		Vector3 a = vector;
		this.m_List.Add(vector);
		int i = 0;
		float num = 0f;
		while (num < this.maxLength)
		{
			i++;
			vector = a + b + Vector3.up * (float)i * -this.gravity * 0.1f;
			num += Vector3.Distance(a, vector);
			this.m_List.Add(vector);
			a = vector;
		}
		GL.Begin(1);
		int count = this.m_List.Count;
		for (i = 0; i < count; i++)
		{
			GL.Vertex(this.m_List[i]);
		}
		GL.End();
		this.m_List.Clear();
	}

	private void CreateLineMaterial()
	{
		if (!this.m_LineMat)
		{
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			this.m_LineMat = new Material(shader);
			this.m_LineMat.hideFlags = HideFlags.HideAndDontSave;
			this.m_LineMat.SetInt("_SrcBlend", 5);
			this.m_LineMat.SetInt("_DstBlend", 10);
			this.m_LineMat.SetInt("_Cull", 0);
			this.m_LineMat.SetInt("_ZWrite", 0);
		}
	}

	[Range(0f, 1f)]
	public float gravity = 0.13f;

	public float maxLength = 50f;

	private const float length = 0.2f;

	private List<Vector3> m_List = new List<Vector3>();

	private Material m_LineMat;
}
