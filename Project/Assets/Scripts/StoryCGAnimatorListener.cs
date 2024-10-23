using System;
using UnityEngine;

public class StoryCGAnimatorListener : MonoBehaviour
{
	public void PlayParticle(int index)
	{
		if (this.CGParticles.allParticles.Length <= index)
		{
			return;
		}
		this.CGParticles.allParticles[index].Play();
	}

	public StoryCGParticles CGParticles;
}
