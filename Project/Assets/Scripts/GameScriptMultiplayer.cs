using System;
using UnityEngine;
using Zombie3D;

public class GameScriptMultiplayer : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("start");
		GameApp.GetInstance().Init();
		GameApp.GetInstance().CreateScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		GameApp.GetInstance().AddMultiplayerComponents();
		this.lastUpdateTime = Time.time;
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		GameApp.GetInstance().Loop(this.deltaTime);
		this.deltaTime = 0f;
	}

	protected float lastUpdateTime;

	protected float deltaTime;
}
