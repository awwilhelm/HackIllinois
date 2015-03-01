using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	// Player Handling
	public float gravity = 20;
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float jumpHeight = 8;
	public float slideDeceleration = 10;
	public Collider lastHitCollided;
	
	// System
	private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	public GameObject lever1;

	public GameObject ghost;
	// States
	private bool jumping;
	public bool isGhost=false;
	public bool isGhostMode = false;
	public bool inLever1 = false;
	public bool inLever2 = false;

	private GameObject ghostInstance;
	public int playerID;

	private GameCamera cam;
	
	// Components
	private PlayerPhysics playerPhysics;
	//private Animator animator;
	
	
	void Start () {
		if (networkView.isMine) {
				playerPhysics = GetComponent<PlayerPhysics> ();
			lever1 = GameObject.Find("Lever1");
		} else {
			enabled = false;
		}
		//animator = GetComponent<Animator>();
		
		//animator.SetLayerWeight(1,1);
	}
	
	void Update () {
		// Reset acceleration upon collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		// If player is touching the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			
			// Jump logic
			if (jumping) {
				jumping = false;
				//animator.SetBool("Jumping",false);
			}		
			
			// Jump Input
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
				jumping = true;
					//animator.SetBool("Jumping",true);
			}
			// Active Input
			if (Input.GetButtonDown("Active")) {
				print ((GameObject.Find("GameManager").GetComponent<GameManager>().lever2PlayerPulled != playerID)+" "+inLever1);

				if(lastHitCollided!=null && inLever1 && 
				   GameObject.Find("GameManager").GetComponent<GameManager>().lever2PlayerPulled != playerID)
				{
					GameObject.Find("firstLever").GetComponent<LeverScript>().setLever1(true);
					Vector3 temp = lastHitCollided.transform.Find("Pull").transform.rotation.eulerAngles;
					temp.z = 325.0f;
					lastHitCollided.transform.Find("Pull").transform.rotation = Quaternion.Euler(temp);
					GameObject.Find("GameManager").GetComponent<GameManager>().lever1PlayerPulled = playerID;
				}
				if(lastHitCollided!=null && inLever2 &&
				   GameObject.Find("GameManager").GetComponent<GameManager>().lever1PlayerPulled != playerID)
				{
					GameObject.Find("firstLever").GetComponent<LeverScript>().setLever2(true);
					Vector3 temp = lastHitCollided.transform.Find("Pull").transform.rotation.eulerAngles;
					temp.z = 325.0f;
					lastHitCollided.transform.Find("Pull").transform.rotation = Quaternion.Euler(temp);
					GameObject.Find("GameManager").GetComponent<GameManager>().lever1PlayerPulled = playerID;
				}
			}


		}

		// Ghosting Input
		if (Input.GetButtonDown("Ghost")) {
			if(isGhost == false)
			{
				ghostInstance = (Network.Instantiate(ghost,new Vector3(transform.position.x, transform.position.y, 0),
				                                     Quaternion.identity, 0) as GameObject);
				isGhostMode = true;
				isGhost = true;
				ghostInstance.GetComponent<PlayerController>().isGhost = true;
				GameObject.FindGameObjectWithTag("myCamera").GetComponent<GameCamera>().SetTarget(ghostInstance.transform);
				ghostInstance.GetComponent<PlayerController>().playerID = playerID;
				//cam.SetTarget(ghostInstance.transform);
			}
			else if(isGhostMode == false)
			{
				//Destroy(gameObject);
				networkView.RPC("RemovePlayer", RPCMode.AllBuffered);
				//isGhost = false;
				isGhostMode = false;
			}
			else
			{
				isGhost = false;
				isGhostMode = false;
				GameObject.FindGameObjectWithTag("myCamera").GetComponent<GameCamera>().SetTarget(transform);
			}
		}

		if(!isGhostMode)
		{
			// Set animator parameters
			//animationSpeed = IncrementTowards(animationSpeed,Mathf.Abs(targetSpeed),acceleration);

			float speed = walkSpeed;  // (Input.GetButton("Run"))?runSpeed:walkSpeed;
			targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed,acceleration);
			
			// Face Direction
			float moveDir = Input.GetAxisRaw("Horizontal");
			if (moveDir !=0) {
				transform.eulerAngles = (moveDir>0)?Vector3.up * 180:Vector3.zero;
			}

			
			// Set amount to move
			amountToMove.x = currentSpeed;
			amountToMove.y -= gravity * Time.deltaTime;
			playerPhysics.Move(amountToMove * Time.deltaTime);
			if(transform.position.y < -10)
			{
				transform.position = GameObject.Find("SpawnPoint").transform.position;
			}
		}
		
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Lever" && other.name == "Lever1") {
			inLever1 = true;
			lastHitCollided = other;
			//lastHitTriggerID = other.GetComponent<LeverID>().getID();
		}
		else{
			inLever1 = false;
		}
		if (other.tag == "Lever" && other.name == "Lever2") {
			inLever2 = true;
			lastHitCollided = other;
			//lastHitTriggerID = other.GetComponent<LeverID>().getID();
		}
		else{
			inLever2 = false;
		}
	}
	// Increase n towards target by speed
	public float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}

	[RPC]
	void RemovePlayer()
	{
		Destroy (gameObject);

	}

	IEnumerator DelayIsGhost(bool value)
	{
		yield return new WaitForSeconds(0.5f);
		isGhost = value;
	}
}
