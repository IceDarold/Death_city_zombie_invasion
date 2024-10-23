using System;
using UnityEngine;

namespace Zombie3D
{
	public interface IGameSceneControl
	{
		void DoControlCannonShoot(bool isShooting);

		SceneMissions GetSceneMissionMgr();

		Vector3[] GetPlayerMissionPath();

		void Revive();

		MissionPath GetMissionPath();

		Player GetPlayer();

		BaseCameraScript GetCamera();

		bool GetGamePlotState();
	}
}
