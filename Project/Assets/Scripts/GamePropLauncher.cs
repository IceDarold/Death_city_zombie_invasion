using System;
using UnityEngine;

public class GamePropLauncher : MonoBehaviour
{
	private void OnEnable()
	{
		this.CurrentProp.Activate(Vector3.zero);
	}

	public GameProp CurrentProp;
}
