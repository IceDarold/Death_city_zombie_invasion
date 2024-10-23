using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class TestRagdoll : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		this.gameScene = GameApp.GetInstance().GetGameScene();
		yield break;
	}

	[ContextMenu("Create Ragdoll")]
	private void CreateRagdoll()
	{
	}

	[ContextMenu("Disable RigidBody")]
	private void DisableRigidBody()
	{
		foreach (GameObject gameObject in this.ragdollInfo.bones)
		{
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	protected GameScene gameScene;

	protected BoneInfo ragdollInfo;
}
