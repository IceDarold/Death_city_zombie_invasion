using System;
using UnityEngine;

public class WeaponModelEffect : MonoBehaviour
{
	private void Awake()
	{
		this._animator = base.GetComponent<Animator>();
		for (int i = 0; i < this.Meshs.Length; i++)
		{
			if (this.Meshs[i] != null)
			{
				this._defaults[i] = this.Meshs[i].material;
			}
		}
	}

	public void ShowAction(string action)
	{
		this._animator.Play(action);
	}

	public void SetDefault()
	{
		for (int i = 0; i < this.Meshs.Length; i++)
		{
			if (this.Meshs[i] != null)
			{
				this.Meshs[i].material = this._defaults[i];
			}
		}
	}

	public void SetHighLinghting(int _index)
	{
		for (int i = 0; i < this.Meshs.Length; i++)
		{
			if (this.Meshs[i] != null)
			{
				if (i == _index)
				{
					this.Meshs[i].material = this.HighLightingMaterial;
					this._texture = this._defaults[i].GetTexture("_MainTex");
					this.HighLightingMaterial.SetTexture("_TextureDiffuse", this._texture);
				}
				else
				{
					this.Meshs[i].material = this._defaults[i];
				}
			}
		}
	}

	private void Update()
	{
		if (base.gameObject.activeSelf && this.HighLightingMaterial != null)
		{
			this.dt += Time.deltaTime * 7f;
			this.HighLightingMaterial.SetFloat("_RimPower", 23f + Mathf.PingPong(this.dt, 7f));
		}
	}

	public Material HighLightingMaterial;

	public MeshRenderer[] Meshs = new MeshRenderer[7];

	private Material[] _defaults = new Material[7];

	private Texture _texture;

	private Animator _animator;

	private float dt;
}
