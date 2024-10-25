using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
	public class SimpleActivatorMenu : MonoBehaviour
	{
		private void OnEnable()
		{
			this.m_CurrentActiveObject = 0;
			this.camSwitchButton.text = this.objects[this.m_CurrentActiveObject].name;
		}

		public void NextCamera()
		{
			int num = (this.m_CurrentActiveObject + 1 < this.objects.Length) ? (this.m_CurrentActiveObject + 1) : 0;
			for (int i = 0; i < this.objects.Length; i++)
			{
				this.objects[i].SetActive(i == num);
			}
			this.m_CurrentActiveObject = num;
			this.camSwitchButton.text = this.objects[this.m_CurrentActiveObject].name;
		}

		public Text camSwitchButton;

		public GameObject[] objects;

		private int m_CurrentActiveObject;
	}
}
