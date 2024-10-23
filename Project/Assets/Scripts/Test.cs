using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombie3D;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
	private IEnumerator Start()
	{
		//TODO:Cash
		// while (!Caching.ready)
		// {
		// 	yield return null;
		// }
		this.www = WWW.LoadFromCacheOrDownload("http://testweixin.joyapi.com/gpgame/thirdshoot/v1/common", 5);
		yield return this.www;
		if (!string.IsNullOrEmpty(this.www.error))
		{
			UnityEngine.Debug.Log(this.www.error);
		}
		AssetBundle myLoadedAssetBundle = this.www.assetBundle;
		UnityEngine.Object asset = myLoadedAssetBundle.mainAsset;
		yield break;
	}

	public void Update()
	{
		UnityEngine.Debug.Log(this.www.progress);
	}

	public void LateUpdate()
	{
		this.allDirs.Clear();
	}

	[ContextMenu("Wrap2Target")]
	public void Wrap2Target()
	{
		this.nav.Warp(this.target.position);
	}

	[ContextMenu("WrapPlayer2Target")]
	public void WrapPlayer2Target()
	{
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePause;
		GameApp.GetInstance().GetGameScene().SetPlayerAndCameraToTarget(this.target);
		GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GamePlaying;
	}

	[ContextMenu("Has Path")]
	public void HasPath()
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		bool flag = NavMesh.CalculatePath(this.start.position, this.end.position, 1, navMeshPath);
		UnityEngine.Debug.LogError(flag + "||" + navMeshPath.status);
	}

	[ContextMenu("Find Path")]
	private void FindPath()
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		bool flag = NavMesh.CalculatePath(this.start.position, this.end.position, 1, navMeshPath);
		this.corners = navMeshPath.corners;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			navMeshPath.corners.Length,
			"-------",
			flag,
			"--------",
			navMeshPath.status
		}));
	}

	public void OnDrawGizmos()
	{
	}

	public NavMeshAgent nav;

	public Transform target;

	public float aa = 2f;

	protected NavMeshHit navHit;

	public Transform start;

	public Transform end;

	public Vector3[] corners;

	public AnimationCurve xPositionOffset;

	public AnimationCurve zRotationOffset;

	public Camera cam;

	public Transform trans;

	public RectTransform rectTrans;

	public Vector3 dir = Vector3.forward;

	public Vector3 axis = Vector3.up;

	public Vector3 finalDir = Vector3.forward;

	public float rotate;

	public float deltaAngel = 2.6f;

	public int num = 10;

	protected List<Vector3> allDirs = new List<Vector3>();

	protected WWW www;
}
