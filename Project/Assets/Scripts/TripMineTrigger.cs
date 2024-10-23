using System;
using UnityEngine;
using Zombie3D;

public class TripMineTrigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9 || other.gameObject.layer == 27 || other.gameObject.layer == 8)
		{
			base.GetComponent<Collider>().enabled = false;
			this.selfDrum.OnHit(new DamageProperty(float.PositiveInfinity), WeaponType.NoGun, Vector3.zero, Bone.None);
		}
	}

	public OilDrum selfDrum;
}
