using System;
using UnityEngine;
using Zombie3D;

public class RagdollAttr : BoneInfo, ICombineMesh
{
	public void Reset()
	{
		this.dissolveOver = false;
		for (int i = 0; i < this.dissolveMesh.Length; i++)
		{
			this.dissolveMesh[i].material.shader = Shader.Find("Custom/Dissolve");
			this.dissolveMesh[i].sharedMaterial.SetFloat("_DissolveThreshold", 0f);
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < this.dissolveMesh.Length; i++)
		{
			this.dissolveMesh[i].material.shader = Shader.Find("Custom/Dissolve");
			this.dissolveMesh[i].sharedMaterial.SetColor("_Diffuse", Color.white);
			this.dissolveMesh[i].sharedMaterial.SetColor("_DissolveColor", Color.black);
			this.dissolveMesh[i].sharedMaterial.SetColor("_DissolveEdgeColor", new Color(1f, 0.352f, 0f, 0f));
			this.dissolveMesh[i].sharedMaterial.SetTexture("_DissolveMap", Resources.Load("DissolveTemplete") as Texture2D);
			this.dissolveMesh[i].sharedMaterial.SetFloat("_DissolveThreshold", 0f);
			this.dissolveMesh[i].sharedMaterial.SetFloat("_ColorFactor", 0.556f);
			this.dissolveMesh[i].sharedMaterial.SetFloat("_DissolveEdge", 0.112f);
		}
		int j = 0;
		int num = this.joints.Length;
		while (j < num)
		{
			this.joints[j].connectedAnchor = this.connectAnchor[j];
			j++;
		}
		this.dissolveStartTime = Time.time;
	}

	public void Update()
	{
		if (this.dissolveOver)
		{
			return;
		}
		float value = (Time.time - this.dissolveStartTime - this.dissolveDelay) / this.dissolveDuration;
		float num = Mathf.Clamp(value, 0f, 1f);
		for (int i = 0; i < this.dissolveMesh.Length; i++)
		{
			this.dissolveMesh[i].material.SetFloat("_DissolveThreshold", num);
		}
		if (num == 1f)
		{
			this.dissolveOver = true;
			this.DissolveOver();
		}
	}

	private void DissolveOver()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[ContextMenu("合并网格")]
	public void CombineMesh()
	{
		base.CombineMesh(base.gameObject);
	}

	[ContextMenu("开启enable projection")]
	private void StartEnableProjection()
	{
		CharacterJoint[] componentsInChildren = base.gameObject.GetComponentsInChildren<CharacterJoint>();
		foreach (CharacterJoint characterJoint in componentsInChildren)
		{
			characterJoint.enableProjection = true;
		}
	}

	[SerializeField]
	[Header("刚体属性")]
	public Rigidbody[] rigidbody;

	[SerializeField]
	[Header("溶解网格")]
	public Renderer[] dissolveMesh;

	[SerializeField]
	[Header("关节脚本")]
	public CharacterJoint[] joints;

	[SerializeField]
	[Header("关节锚点")]
	public Vector3[] connectAnchor;

	[Space]
	[CNName("溶解延时")]
	public float dissolveDelay = 0.5f;

	[CNName("溶解持续时间")]
	public float dissolveDuration = 1f;

	protected bool dissolveOver;

	protected float dissolveStartTime;
}
