using System;
using UnityEngine;

public class MissionPathElf : MonoBehaviour
{
	public void Show(Vector3[] _path)
	{
		this.Path = _path;
		this.index = 1;
		this.countdown = 0f;
		base.transform.position = this.Path[0];
		this.Child.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.Path = null;
		base.gameObject.SetActive(false);
	}

	private void MoveAsPath()
	{
		if (this.index < this.Path.Length)
		{
			float num = this.Speed * Time.deltaTime;
			float num2 = Vector3.Distance(base.transform.position, this.Path[this.index]);
			if (num > num2)
			{
				this.index++;
			}
			else
			{
				base.transform.LookAt(this.Path[this.index]);
				base.transform.Translate(Vector3.forward * this.Speed * Time.deltaTime);
			}
		}
		else
		{
			this.Hide();
		}
	}

	private void Update()
	{
		if (this.Path != null && base.gameObject.activeSelf)
		{
			this.countdown += Time.deltaTime;
			if (this.countdown > 0.5f && this.countdown <= 3f)
			{
				this.Child.gameObject.SetActive(true);
			}
			else if (this.countdown > 3f)
			{
				this.Child.gameObject.SetActive(false);
			}
			this.MoveAsPath();
		}
	}

	public Transform Child;

	public float Speed;

	public float Rotate = 1f;

	private Vector3[] Path;

	private int index;

	private float countdown;
}
