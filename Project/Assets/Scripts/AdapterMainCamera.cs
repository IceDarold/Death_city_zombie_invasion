using System;
using UnityEngine;

public class AdapterMainCamera : MonoBehaviour
{
	public void Awake()
	{
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		if (this.theCamera == null)
		{
			this.theCamera = Camera.main;
		}
		this.tx = this.theCamera.transform;
		if (this.theCamera.orthographic)
		{
			this.ScaleByOrthographicCamera();
		}
		else
		{
			this.ScaleThis();
		}
	}

	private void ScaleByOrthographicCamera()
	{
		float orthographicSize = this.theCamera.orthographicSize;
		float aspect = this.theCamera.aspect;
		float num = orthographicSize * 2f;
		float num2 = num * aspect;
		Vector3[] vertices = this.mesh.vertices;
		float num3 = num2 / Mathf.Abs(vertices[0].x * 2f);
		float num4 = num / Mathf.Abs(vertices[0].z * 2f);
		base.transform.localScale = new Vector3(num3 + 0.01f, 1f, num4 + 0.01f);
	}

	private void ScaleThis()
	{
		float z = base.transform.localPosition.z;
		Vector3[] corners = this.GetCorners(z);
		float magnitude = (corners[0] - corners[1]).magnitude;
		float magnitude2 = (corners[1] - corners[3]).magnitude;
		Vector3[] vertices = this.mesh.vertices;
		float x = magnitude / Mathf.Abs(vertices[0].x * 2f);
		float y = magnitude2 / Mathf.Abs(vertices[0].y * 2f);
		base.transform.localScale = new Vector3(x, y, 1f);
	}

	private Vector3[] GetCorners(float distance)
	{
		Vector3[] array = new Vector3[4];
		float f = this.theCamera.fieldOfView * 0.5f * 0.0174532924f;
		float aspect = this.theCamera.aspect;
		float num = distance * Mathf.Tan(f);
		float d = num * aspect;
		array[0] = this.tx.position - this.tx.right * d;
		array[0] += this.tx.up * num;
		array[0] += this.tx.forward * distance;
		array[1] = this.tx.position + this.tx.right * d;
		array[1] += this.tx.up * num;
		array[1] += this.tx.forward * distance;
		array[2] = this.tx.position - this.tx.right * d;
		array[2] -= this.tx.up * num;
		array[2] += this.tx.forward * distance;
		array[3] = this.tx.position + this.tx.right * d;
		array[3] -= this.tx.up * num;
		array[3] += this.tx.forward * distance;
		return array;
	}

	protected Mesh mesh;

	protected Transform tx;

	public Camera theCamera;
}
