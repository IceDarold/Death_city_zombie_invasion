using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionPath : MonoBehaviour
{
	private void Awake()
	{
		this.PathMat = this.PathRender.materials[0];
	}

	public void Init(Transform _player)
	{
		this.player = _player;
	}

	public void Show(Mission _mission)
	{
		this.curMission = _mission;
		base.gameObject.SetActive(true);
		this.canDraw = true;
	}

	public void Hide()
	{
		this.canDraw = false;
		base.gameObject.SetActive(false);
	}

	public void Reset(Vector3[] _path)
	{
		if (_path == null)
		{
			return;
		}
		this.path = _path;
	}

	private bool CheckMission()
	{
		if (this.missions == null || this.player == null)
		{
			return false;
		}
		this.curMission = this.missions.CurMission;
		return this.curMission != null && this.curMission.mType != EMission.KILL_ENEMY && this.curMission.mType != EMission.KILL_WAVE && this.curMission.mType != EMission.NONE;
	}

	private MissionPathElf GetIdleElf()
	{
		for (int i = 0; i < this.Elfs.Count; i++)
		{
			if (!this.Elfs[i].gameObject.activeSelf)
			{
				return this.Elfs[i];
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ElfObject);
		gameObject.transform.SetParent(base.transform);
		MissionPathElf component = gameObject.GetComponent<MissionPathElf>();
		this.Elfs.Add(component);
		return component;
	}

	public void DrowMissionPath()
	{
		this.PathRender.positionCount = this.path.Length;
		for (int i = 0; i < this.path.Length; i++)
		{
			this.PathRender.SetPosition(i, this.path[i] + Vector3.up * 0.1f);
		}
		MissionPathElf idleElf = this.GetIdleElf();
		this.path[0] = this.player.position;
		idleElf.Show(this.path);
	}

	private void Update()
	{
		if (!this.canDraw)
		{
			return;
		}
		if (this.ct < this.CheckTime)
		{
			this.ct += Time.deltaTime;
		}
		else
		{
			this.ct = 0f;
			this.DrowMissionPath();
		}
		this.PathRender.SetPosition(0, this.player.position + Vector3.up * 0.1f);
	}

	public LineRenderer PathRender;

	public GameObject ElfObject;

	public List<MissionPathElf> Elfs;

	public float CheckTime = 1f;

	private Material PathMat;

	private Vector3[] path;

	private Transform player;

	private SceneMissions missions;

	private Mission curMission;

	private float ct;

	private float offsetX;

	protected bool canDraw;
}
