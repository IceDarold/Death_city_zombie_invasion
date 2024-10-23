using System;
using UnityEngine;

public class ConnectScript : MonoBehaviour
{
	private void Start()
	{
		//Network.Connect("192.168.2.103", 25000);
	}

	private void Update()
	{
	}

	private void OnConnectedToServer()
	{
		UnityEngine.Debug.Log("Get Connected...");
		
	}

	

	private void OnDisconnectedFromServer()
	{
		UnityEngine.Debug.Log("DisConnected...");
	}
}
