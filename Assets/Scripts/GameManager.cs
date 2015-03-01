using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject player;
	public GameObject player2;
	private GameCamera cam;
	private int lever1PlayerPulled=0;
	private int lever2PlayerPulled=0;
	public int count;
	
	void Start () {
		count = 1;
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
		print (count);
		if(Network.connections.Length==1)
		{
			GameObject.Find ("Camera").SetActive (false);

			Transform playerTrans = ((Network.Instantiate(player,GameObject.Find("SpawnPoint").transform.position,Quaternion.identity, 0) as GameObject).transform);
			playerTrans.transform.tag = "Player";
			playerTrans.GetComponent<PlayerController> ().playerID = 1;
		} else if( Network.connections.Length==2)
		{
			GameObject.Find ("Camera").SetActive (false);
			
			Transform playerTrans = ((Network.Instantiate(player2,GameObject.Find("SpawnPoint").transform.position,Quaternion.identity, 0) as GameObject).transform);
			playerTrans.transform.tag = "Player";
			playerTrans.GetComponent<PlayerController> ().playerID = 2;
		}
		networkView.RPC ("addToCount", RPCMode.AllBuffered);
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

	public int getLever1PlayerPulled()
	{
		return lever1PlayerPulled;
	}

	public void setLever1PlayerPulled(int input)
	{
		networkView.RPC ("rpcLever1", RPCMode.AllBuffered, input);
	}

	public int getLever2PlayerPulled()
	{
		return lever2PlayerPulled;
	}

	public void setLever2PlayerPulled(int input)
	{
		networkView.RPC ("rpcLever2", RPCMode.AllBuffered, input);
	}



	[RPC]
	void rpcLever1(int input)
	{
		lever1PlayerPulled = input;
	}
	[RPC]
	void rpcLever2(int input)
	{
		lever2PlayerPulled = input;
	}

	[RPC]
	void addToCount()
	{
		count+=1;
	}



}
