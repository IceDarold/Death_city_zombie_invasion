using System;
using UnityEngine;

public class PlayerSnipePoint : MonoBehaviour
{
	[CNName("俯仰最大值")]
	public float maxAngelV;

	[CNName("仰角最小值")]
	public float minAngelV;

	[CNName("水平视角转动限制")]
	public float maxAngelH;

	[Header("相机转轴中心点")]
	public Vector3 snipeCameraAnchor;

	[CNName("相机转轴长度")]
	public float snipeCameraDistances;

	[Space]
	[EnumLabel("角色姿势")]
	public PlayerPose playerPose;
}
