using UnityEngine;
using System.Collections;

public class ControlDoor1 : MonoBehaviour {

	public GameObject myDoor1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			myDoor1.animation["door1"].speed = 1;
			myDoor1.animation ["door1"].time = 0;
			myDoor1.animation.Play("door1");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			myDoor1.animation["door1"].speed = -1;
			myDoor1.animation ["door1"].time = myDoor1.animation ["door1"].length;
			myDoor1.animation.Play("door1");
		}
	}


}
