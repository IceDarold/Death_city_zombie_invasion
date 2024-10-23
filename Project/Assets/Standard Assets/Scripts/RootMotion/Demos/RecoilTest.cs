using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Recoil))]
	public class RecoilTest : MonoBehaviour
	{
		private void Start()
		{
			this.recoil = base.GetComponent<Recoil>();
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
			{
				this.recoil.Fire(this.magnitude);
			}
		}

		private void OnGUI()
		{
			GUILayout.Label("Press R or LMB for procedural recoil.", new GUILayoutOption[0]);
		}

		public float magnitude = 1f;

		private Recoil recoil;
	}
}
