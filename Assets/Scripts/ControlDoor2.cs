using UnityEngine;
using System.Collections;

public class ControlDoor2 : MonoBehaviour {

	public GameObject door1;
	public GameObject door2;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			door1.animation["door1"].speed = 1;
			door1.animation ["door1"].time = 0;
			door1.animation.Play("door1");

			door2.animation["door2"].speed = 1;
			door2.animation ["door2"].time = 0;
			door2.animation.Play("door2");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			door1.animation["door1"].speed = -1;
			door1.animation ["door1"].time = door1.animation ["door1"].length;
			door1.animation.Play("door1");

			door2.animation["door2"].speed = -1;
			door2.animation ["door2"].time = door1.animation ["door2"].length;
			door2.animation.Play("door2");
		}
	}






}
