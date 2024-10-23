using System;
using UnityEngine;

public class OpenBoxAni : MonoBehaviour
{
	public void FinishAnimator(int num)
	{
		this.curBox.FinishAnimator(num);
	}

	public BoxPage curBox;
}
