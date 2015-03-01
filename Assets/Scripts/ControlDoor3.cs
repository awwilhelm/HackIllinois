using UnityEngine;
using System.Collections;

public class ControlDoor3 : MonoBehaviour {

	public GameObject door2;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			door2.animation["door2"].speed = 1;
			door2.animation ["door2"].time = 0;
			door2.animation.Play("door2");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag != "cameraZoom")
		{
			door2.animation["door2"].speed = -1;
			door2.animation ["door2"].time = door2.animation ["door2"].length;
			door2.animation.Play("door2");
		}
	}


}
