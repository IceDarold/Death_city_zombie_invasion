using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class SubwayCameraShaker : MonoBehaviour
{
	public IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		this.tpsCamera = GameApp.GetInstance().GetGameScene().GetCamera();
		this.tpsCamera.RegisterCamerControl(new Action<GameObject>(this.ApplyCameraShake));
		yield break;
	}

	public void ApplyCameraShake(GameObject camera)
	{
		camera.transform.localPosition = new Vector3(this.xPositionOffset.Evaluate(Time.time), 0f, 0f);
		Vector3 eulerAngles = camera.transform.localRotation.eulerAngles;
		camera.transform.localRotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, this.zRotationOffset.Evaluate(Time.time)));
	}

	public AnimationCurve xPositionOffset;

	public AnimationCurve zRotationOffset;

	protected BaseCameraScript tpsCamera;
}
