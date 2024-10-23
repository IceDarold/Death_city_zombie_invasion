using System;
using UnityEngine;

[Serializable]
public class MissionItemInfo
{
	[NonSerialized]
	public GameObject instantiatedObject;

	public MissionItemType type;

	public bool needSubmit;

	public bool deleteWhenComplete;

	public string prefabName;

	public Vector3 localPos;

	public Vector3 localScale;

	public Vector3 localRotation;

	public bool alwaysShow;

	public bool forceStay;

	public float stayDuration;
}
