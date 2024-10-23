using System;
using UnityEngine;

[AddComponentMenu("TPS/PrefabObjectManager")]
public class ResourceConfigScript : Singleton<ResourceConfigScript>
{
	public GameObject hitBlood
	{
		get
		{
			return this.hitBlood_ref.resource;
		}
	}

	public GameObject headShotBlood
	{
		get
		{
			return this.headShotBlood_ref.resource;
		}
	}

	public GameObject headShotEffect
	{
		get
		{
			return this.headShotEffect_ref.resource;
		}
	}

	public GameObject deadBlood
	{
		get
		{
			return this.deadBlood_ref.resource;
		}
	}

	public GameObject deadFoorblood
	{
		get
		{
			return this.deadFoorblood_ref.resource;
		}
	}

	public GameObject deadFoorblood2
	{
		get
		{
			return this.deadFoorblood2_ref.resource;
		}
	}

	public GameObject hitparticles
	{
		get
		{
			return this.hitparticles_ref.resource;
		}
	}

	public GameObject boomerExplosion
	{
		get
		{
			return this.boomerExplosion_ref.resource;
		}
	}

	public GameObject fireline
	{
		get
		{
			return this.fireline_ref.resource;
		}
	}

	public GameObject bullets
	{
		get
		{
			return this.bullets_ref.resource;
		}
	}

	public GameObject woodExplode
	{
		get
		{
			return this.woodExplode_ref.resource;
		}
	}

	public GameObject headShootParticle
	{
		get
		{
			return this.headShootParticle_ref.resource;
		}
	}

	public GameObject spitterBullet
	{
		get
		{
			return this.spitterBullet_ref.resource;
		}
	}

	public GameObject despotBullet
	{
		get
		{
			return this.despotBullet_ref.resource;
		}
	}

	public GameObject bombParticle
	{
		get
		{
			return this.bombParticle_ref.resource;
		}
	}

	public GameObject GrenadeLauncherBullet
	{
		get
		{
			return this.GrenadeLauncherBullet_ref.resource;
		}
	}

	public GameObject EnemyComeOutParticle
	{
		get
		{
			return this.EnemyComeOutParticle_ref.resource;
		}
	}

	public GameObject Bomber2Explod
	{
		get
		{
			return this.Bomber2Explod_ref.resource;
		}
	}

	public GameObject Bomber2Screen
	{
		get
		{
			return this.Bomber2Screen_ref.resource;
		}
	}

	public GameObject SnipeNormalBullet
	{
		get
		{
			return this.snipeNormalBullet_ref.resource;
		}
	}

	public GameObject SnipeFinalBullet
	{
		get
		{
			return this.snipeFinalBullet_ref.resource;
		}
	}

	public GameObject SnipeLastShootGunFire
	{
		get
		{
			return this.snipeLastShootGunFire_ref.resource;
		}
	}

	public EffectResource hitBlood_ref;

	public EffectResource headShotBlood_ref;

	public EffectResource headShotEffect_ref;

	public EffectResource deadBlood_ref;

	public EffectResource deadFoorblood_ref;

	public EffectResource deadFoorblood2_ref;

	public EffectResource hitparticles_ref;

	public EffectResource boomerExplosion_ref;

	public EffectResource fireline_ref;

	public EffectResource bullets_ref;

	public EffectResource woodExplode_ref;

	public EffectResource headShootParticle_ref;

	public EffectResource spitterBullet_ref;

	public EffectResource despotBullet_ref;

	public EffectResource bombParticle_ref;

	public EffectResource GrenadeLauncherBullet_ref;

	public EffectResource EnemyComeOutParticle_ref;

	public EffectResource Bomber2Explod_ref;

	public EffectResource Bomber2Screen_ref;

	public EffectResource snipeNormalBullet_ref;

	public EffectResource snipeFinalBullet_ref;

	public EffectResource snipeLastShootGunFire_ref;
}
