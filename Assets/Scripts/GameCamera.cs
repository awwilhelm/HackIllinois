using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	private Transform target;
	public float trackSpeed = 25;
	public bool zoom = true;
	//public GameObject parent;

	void Start()
	{
		if(networkView.isMine)
		{
			target = transform.parent.transform;
			transform.parent = null;
			transform.tag = "MainCamera";
		}
		else
		{
			transform.parent = null;
			gameObject.SetActive(false);
		}
	}
	void Update()
	{
//		if(zoom == true)
//		{
//			gameObject.GetComponent<Camera>().orthographicSize = 30;  //17
//		}
//		else
//		{
//			gameObject.GetComponent<Camera>().orthographicSize = 10;
//		}
	}


	
	// Set target
	public void SetTarget(Transform t) {
		target = t;
	}
	
	// Track target
	void LateUpdate() {
		if (target) {
			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
			transform.position = new Vector3(x,y, transform.position.z);
		}
	}
	
	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
