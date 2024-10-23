using System;
using UnityEngine;
using Zombie3D;

public class ControllerHitScript : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string name = hit.collider.name;
        if (name.StartsWith("E_"))
        {
            Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(name);
            Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
            if (attachedRigidbody == null || attachedRigidbody.isKinematic)
            {
                return;
            }
            float d = 2f;
            Vector3 a = new Vector3(hit.moveDirection.z, hit.moveDirection.y, hit.moveDirection.x);
            attachedRigidbody.velocity = a * d;
        }
    }
}
