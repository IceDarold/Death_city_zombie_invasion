using System;
using UnityEngine;

[AddComponentMenu("TPS/TopWatchingCamera")]
public class TopWatchingCameraScript : BaseCameraScript
{
	public override global::CameraType GetCameraType()
	{
		return global::CameraType.TopWatchingCamera;
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public override void Init(PlayerSnipePoint snipeCameraCfg = null)
	{
		base.Init(null);
		this.moveTo = this.player.GetTransform().TransformPoint(this.cameraDistanceFromPlayer);
		this.absoluteDistanceFromPlayer = this.moveTo - this.player.GetTransform().position;
		base.transform.LookAt(this.player.GetTransform());
		this.started = true;
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
	}

	protected bool cameraset;

	protected Vector3 absoluteDistanceFromPlayer;
}
