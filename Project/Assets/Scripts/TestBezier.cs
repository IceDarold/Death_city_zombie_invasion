using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestBezier : MonoBehaviour
{
	public void OnRenderObject()
	{
		this.CreateLineMaterialBezier();
		this.m_LineMat.SetPass(0);
		this.control = base.transform.position + (this.target.position - base.transform.position) / 2f + Vector3.up * this.deltaY;
		this.bz = new ThreePointBezier(base.transform.position, this.control, this.target.position);
		for (int i = 0; i < 100; i++)
		{
			this.m_List.Add(this.bz.GetPointAtTime(0.01f * (float)i));
		}
		GL.Begin(1);
		for (int j = 0; j < this.m_List.Count; j++)
		{
			GL.Vertex(this.m_List[j]);
		}
		GL.End();
		this.m_List.Clear();
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawSphere(this.control, 0.1f);
	}

	public void Update()
	{
		if (!this.moveObject)
		{
			return;
		}
		if (this.reset)
		{
			this.percent = 0f;
			this.reset = false;
		}
		this.moveObject.transform.position = this.bz.GetPointAtTime(this.percent);
		this.percent += Time.deltaTime;
	}

	private void CreateLineMaterialBezier()
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

	public Transform target;

	public float deltaY;

	public GameObject moveObject;

	public bool reset;

	private List<Vector3> m_List = new List<Vector3>();

	private Material m_LineMat;

	private Vector3 control;

	private ThreePointBezier bz;

	protected float percent;
}
