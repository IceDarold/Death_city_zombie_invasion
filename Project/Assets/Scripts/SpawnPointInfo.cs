using System;
using UnityEngine;

[Serializable]
public class SpawnPointInfo
{
	public string pt_name;

	public Transform trans;

	public Collider collider;

	public bool alwaysSpawn;

	public bool enableElite;
}
