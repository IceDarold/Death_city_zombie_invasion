using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class PlatformSpecificContent : MonoBehaviour
	{
		private void OnEnable()
		{
			this.CheckEnableContent();
		}

		private void CheckEnableContent()
		{
			if (this.m_BuildTargetGroup == PlatformSpecificContent.BuildTargetGroup.Mobile)
			{
				this.EnableContent(true);
			}
			else
			{
				this.EnableContent(false);
			}
		}

		private void EnableContent(bool enabled)
		{
			if (this.m_Content.Length > 0)
			{
				foreach (GameObject gameObject in this.m_Content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (this.m_ChildrenOfThisObject)
			{
				IEnumerator enumerator = base.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						transform.gameObject.SetActive(enabled);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			if (this.m_MonoBehaviours.Length > 0)
			{
				foreach (MonoBehaviour monoBehaviour in this.m_MonoBehaviours)
				{
					monoBehaviour.enabled = enabled;
				}
			}
		}

		[SerializeField]
		private PlatformSpecificContent.BuildTargetGroup m_BuildTargetGroup;

		[SerializeField]
		private GameObject[] m_Content = new GameObject[0];

		[SerializeField]
		private MonoBehaviour[] m_MonoBehaviours = new MonoBehaviour[0];

		[SerializeField]
		private bool m_ChildrenOfThisObject;

		private enum BuildTargetGroup
		{
			Standalone,
			Mobile
		}
	}
}
