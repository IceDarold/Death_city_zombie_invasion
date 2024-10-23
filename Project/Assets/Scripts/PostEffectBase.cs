using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour
{
	public Material _Material
	{
		get
		{
			if (this._material == null)
			{
				this._material = this.GenerateMaterial(this.shader);
			}
			return this._material;
		}
	}

	protected Material GenerateMaterial(Shader shader)
	{
		if (shader == null)
		{
			return null;
		}
		if (!shader.isSupported)
		{
			return null;
		}
		Material material = new Material(shader);
		material.hideFlags = HideFlags.DontSave;
		if (material)
		{
			return material;
		}
		return null;
	}

	public Shader shader;

	private Material _material;
}
