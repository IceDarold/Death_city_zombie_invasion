using System;
using UnityEngine;

public class TestRelativeRotation : MonoBehaviour
{
	private void Awake()
	{
		this.relativePos = this.rootTrans.InverseTransformPoint(this.target.position);
		this.relativePos1 = this.rootTrans.InverseTransformPoint(this.target.position + this.target.forward);
	}

	private void LateUpdate()
	{
		this.target.position = base.transform.TransformPoint(this.relativePos);
		this.target.rotation = Quaternion.LookRotation(this.rootTrans.TransformPoint(this.relativePos1) - this.rootTrans.TransformPoint(this.relativePos));
	}

	public Transform target;

	public Transform rootTrans;

	protected Vector3 relativePos;

	protected Vector3 relativePos1;
}
