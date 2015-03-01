using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject player;
	private GameCamera cam;
	public int lever1PlayerPulled=0;
	public int lever2PlayerPulled=0;
	
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

		Transform playerTrans = ((Network.Instantiate(player,GameObject.Find("SpawnPoint").transform.position,Quaternion.identity, 0) as GameObject).transform);
		playerTrans.transform.tag = "Player";
		playerTrans.GetComponent<PlayerController> ().playerID = Network.connections.Length;
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
