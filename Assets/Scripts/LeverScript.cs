using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour {

	public GameObject stairsMove1;
	public bool moveStairs1=false;
	public GameObject otherLever;

	private bool lever1=false;
	private bool lever2=false;
	private bool playedOnce=false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(lever1 == true && lever2 == true && playedOnce == false)
		{
			stairsMove1.animation.Play("stairsMove1");

			playedOnce = true;
		}
	}

	public void setLever1(bool l1)
	{
		lever1 = l1;
	}
	
	public void setLever2(bool l2)
	{
		lever2 = l2;
	}

	private void replayVid()
	{
		stairsMove1.animation["stairsMove1"].speed = -1;
		stairsMove1.animation ["stairsMove1"].time = stairsMove1.animation ["stairsMove1"].length;
		stairsMove1.animation.Play("stairsMove1");
	}
}
