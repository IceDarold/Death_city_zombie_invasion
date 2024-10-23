using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestParabola : MonoBehaviour
{
	public void OnRenderObject()
	{
		this.CreateLineMaterial();
		this.m_LineMat.SetPass(0);
		Vector3 a = this.obj.forward + Mathf.Atan(0.0174532924f * Mathf.Abs(this.elevation)) * this.obj.up;
		Vector3 vector = new Vector3(0f, this.gravity, 0f);
		Vector3 vector2 = this.obj.position;
		Vector3 a2 = vector2;
		this.m_List.Add(vector2);
		int i = 0;
		while (i < this.maxVirtNum)
		{
			i++;
			a -= Vector3.up * this.gravity * 0.0166666675f;
			vector2 = a2 + a * this.speed * 0.0166666675f;
			this.m_List.Add(vector2);
			a2 = vector2;
		}
		GL.Begin(1);
		for (int j = 0; j < this.m_List.Count; j++)
		{
			GL.Vertex(this.m_List[j]);
		}
		GL.End();
		this.m_List.Clear();
	}

	[ContextMenu("Clear")]
	private void Clear()
	{
	}

	[ContextMenu("start")]
	private void StartMove()
	{
		this.dir = this.obj.forward + Mathf.Atan(0.0174532924f * Mathf.Abs(this.elevation)) * this.obj.up;
		this.moveObj.position = this.obj.position;
		this.isStart = true;
	}

	public void Update()
	{
		if (!this.isStart)
		{
			return;
		}
		this.dir -= Vector3.up * this.gravity * Time.deltaTime;
		this.moveObj.Translate(this.dir * this.speed * Time.deltaTime);
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

	public Transform obj;

	public Transform moveObj;

	public Transform target;

	public float gravity;

	public float speed = 1f;

	public int maxVirtNum = 100;

	public float elevation;

	public bool isStart;

	private List<Vector3> m_List = new List<Vector3>();

	private Material m_LineMat;

	private Vector3 dir = Vector3.zero;
}
