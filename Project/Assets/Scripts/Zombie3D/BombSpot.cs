using System;
using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class BombSpot
	{
		public BombSpot.BombSpotState GetSpotState()
		{
			return this.bss;
		}

		public void Init()
		{
			this.spotTransform = this.bombSpotObj.transform;
			this.gameScene = GameApp.GetInstance().GetGameScene();
		}

		public void DoLogic()
		{
			if (this.bss != BombSpot.BombSpotState.Installing || Time.time - this.lastInstallTime > this.installTimeTakes)
			{
			}
			if (this.bss == BombSpot.BombSpotState.Installing)
			{
				Hashtable enemies = this.gameScene.GetEnemies();
				IEnumerator enumerator = enemies.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Enemy enemy = (Enemy)obj;
						if (enemy.GetState() != Enemy.DEAD_STATE && (enemy.GetPosition() - this.spotTransform.position).sqrMagnitude < this.spotRadius * this.spotRadius)
						{
							this.bss = BombSpot.BombSpotState.UnInstalled;
							break;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		public bool CheckInSpot()
		{
			if (this.bss != BombSpot.BombSpotState.UnInstalled)
			{
				return false;
			}
			Player player = this.gameScene.GetPlayer();
			float sqrMagnitude = (player.GetTransform().position - this.spotTransform.position).sqrMagnitude;
			return sqrMagnitude < this.spotRadius * this.spotRadius;
		}

		public void Install()
		{
			this.lastInstallTime = Time.time;
			this.bss = BombSpot.BombSpotState.Installing;
		}

		public bool isInstalling()
		{
			return this.bss == BombSpot.BombSpotState.Installing;
		}

		public float GetInstallingProgress()
		{
			return (Time.time - this.lastInstallTime) / this.installTimeTakes;
		}

		public GameScene gameScene;

		public GameObject bombSpotObj;

		public float spotRadius = 5f;

		public float installTimeTakes = 5f;

		protected float lastInstallTime;

		protected BombSpot.BombSpotState bss;

		protected Transform spotTransform;

		public enum BombSpotState
		{
			UnInstalled,
			Installing,
			Installed
		}
	}
}
