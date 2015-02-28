using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject player;
	private GameCamera cam;
	public GameObject player1;
	public GameObject player2;
	
	void Start () {
		//cam = player1.GetComponent<GameCamera>();
		MultiplayerScript a = GameObject.Find ("MultiplayerManager").GetComponent<MultiplayerScript> ();
		print (a.serverStartedAndNoPlayersConnected);
		//SpawnPlayer();
	}
	void OnConnectedToServer()
	{
		SpawnPlayer ();
	}
	// Spawn player
	public void SpawnPlayer() {
		Transform playerTrans = ((Network.Instantiate(player,new Vector3(0, 5, 0),Quaternion.identity, 0) as GameObject).transform);
		cam = playerTrans.transform.FindChild ("CameraHead").GetComponent<GameCamera> ();
		cam.transform.parent = null;
		cam.SetTarget(playerTrans.transform);
	}
}
