using System;
using UnityEngine;

public class WaitingPopup : GamePage
{
	public override void Show()
	{
		base.Show();
		this.isRotate = true;
	}

	public override void Hide()
	{
		base.Hide();
		this.isRotate = false;
	}

	private void Update()
	{
		if (this.isRotate)
		{
			this.Trans.localEulerAngles += new Vector3(0f, 0f, 180f * Time.deltaTime);
		}
	}

	public Transform Trans;

	private bool isRotate;
}
