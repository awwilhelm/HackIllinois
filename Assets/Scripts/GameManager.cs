﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject player;
	private GameCamera cam;
	
	void Start () {
	}

	void Update()
	{

	}

	void OnConnectedToServer()
	{
		SpawnPlayer ();
	}
	// Spawn player
	public void SpawnPlayer() {
		GameObject.Find ("Camera").SetActive (false);
		Transform playerTrans = ((Network.Instantiate(player,new Vector3(0, 5, 0),Quaternion.identity, 0) as GameObject).transform);
		playerTrans.transform.tag = "Player";
		//cam = playerTrans.transform.FindChild ("CameraHead").GetComponent<GameCamera> ();
//		if(networkView.isMine)
//		{
//			cam.transform.parent = null;
//			cam.SetTarget(playerTrans.transform);
//		}
//		else
//		{
//			cam.transform.parent = null;
//			//cam.enabled = false;
//		}
	}
}
