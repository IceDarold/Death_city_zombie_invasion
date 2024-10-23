using System;
using UnityEngine;
using Zombie3D;

public interface IGameUIControl
{
	void MissionComplete();

	void MissionStart(Mission _mission);

	void SetMissionGuidePosition(Vector2 _pos);

	void SetInstantiatedInterface(IGameSceneControl _gameScene, IPlayerControl _player);

	void SetScore(int score);

	void GameFalied();

	int PlayerGameCoin();

	int PlayerGameScore();

	void SetUIDisplayMode(GamePlayingMode _mode);

	void SetUIDisplayEvnt(UIDisplayEvnt evnt, params float[] param);

	void SetProgressEnable(bool enable, bool isCircle);

	void SetReloadProgressPercent(float percent);

	void SetReloadProgressPercent(int cur, int max);

	void SetUIPlayerWeapon(string weapon);

	bool IsTouchInShootThumb(Vector2 touchPos);

	void SetTouchShootThumb(bool isShoot);

	void SetGameControlMode(GameControlMode mode);

	void SetPlayerEquipedWeaponData(Weapon wp);

	void SetLevelDescription(string intro, string description);
}
