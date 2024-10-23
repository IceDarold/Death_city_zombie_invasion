using System;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using UnityEngine;
using Zombie3D;

public class SceneLoader : MonoBehaviour
{
	public void DoLoad(int levelIndex)
	{
		base.StartCoroutine(this.LoadScene(levelIndex));
	}

	private IEnumerator LoadScene(int levelIndex)
	{
		yield return null;
		for (int i = 0; i < this.allLoaders.Count; i++)
		{
			this.allLoaders[i].DoLoad(levelIndex);
		}
		for (int j = 0; j < this.allSceneBindAble.Count; j++)
		{
			this.allSceneBindAble[j].DoActive(levelIndex);
		}
		for (int k = 0; k < this.allEnemySpawns.Count; k++)
		{
			this.allEnemySpawns[k].DoActive(levelIndex);
		}
		GameApp.GetInstance().GetGameScene().gamePlotManager.SetPlotEnableByLevel(levelIndex);
		yield break;
	}

	public void LoadSceneObjects(int levelIndex, Action callback, bool loadFromAB)
	{
		this.scenePrefab.DoLoad(levelIndex, delegate()
		{
			this.StartCoroutine(this.SetPlayingState(levelIndex));
			this.StartCoroutine(this.LoadScene(levelIndex));
			GameApp.GetInstance().GetGameScene().CreateLimbsAndHitBlood();
			if (callback != null)
			{
				callback();
			}
		}, loadFromAB);
	}

	private IEnumerator SetPlayingState(int levelIndex)
	{
		yield return new WaitForSeconds(0.5f);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene.PlayingState == PlayingState.Changing)
		{
			Singleton<FadeAnimationScript>.Instance.FadeOutBlack(0.5f, delegate
			{
				if (gameScene.PlayingMode == GamePlayingMode.SnipeMode)
				{
					CheckpointData checkPointDataBySceneID = CheckpointDataManager.GetCheckPointDataBySceneID(levelIndex);
					gameScene.GameUI.SetLevelDescription(checkPointDataBySceneID.Name, checkPointDataBySceneID.Describe);
				}
				else
				{
					gameScene.PlayingState = PlayingState.GamePlaying;
				}
			});
			gameScene.OnSceneSplashFadeOut();
		}
		Singleton<UiManager>.Instance.SetUIEnable(true);
		yield break;
	}

	public Transform GetPlayerSpawnPoint(int levelIndex)
	{
		PlayerSpawnScript playerSpawnScript = UnityEngine.Object.FindObjectOfType<PlayerSpawnScript>();
		return playerSpawnScript.GetPlayerSpawn(levelIndex).transform;
	}

	public ScenePrefabs scenePrefab;

	public List<SceneBatches> allLoaders = new List<SceneBatches>();

	public List<BaseEnemySpawn> allEnemySpawns = new List<BaseEnemySpawn>();

	public List<SceneBindAble> allSceneBindAble = new List<SceneBindAble>();

	[CNName("炮台位置")]
	public Transform cannonTrans;

	[Space(10f)]
	[SerializeField]
	[Header("--------------------------------------------------------------")]
	[CNName("当前角色")]
	public AvatarType curAvatar;

	[CNName("当前关卡（编辑器模式）")]
	public int curLevelIndex;

	[CNName("怪物血量")]
	public float enemyHp = 500f;

	[CNName("怪物攻击力")]
	public float enemyAttack = 1f;

	[CNName("武器1")]
	public string weapon1 = "AK47";

	[CNName("武器2")]
	public string weapon2 = "GLK18";
}
