using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
public class RigidBodyFPSWalker : AbilityController {

	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool  canJump = true;
	public float jumpHeight = 2.0f;
	public bool  grounded = false;
	public string dead = "LIVE";
	public GameObject fpsCamera;
	public GameObject standbyCamera;
	//控制哪個相機的視角
	bool cameraState=false;
	SpawnSpot[] spawnSpots;
	void Awake () {
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;
	}
	void Start () {
		
	}
	void FixedUpdate () {
		
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity  = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Apply a force that attempts to reach our target velocity
			Vector3 velocity 	   = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
		if (grounded) {
			// Jump
			if (canJump && Input.GetButton("Jump")) {
				GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
		}

		// We apply gravity manually for more tuning control
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));

		grounded = false;

		//如果已經死亡可以開啟另一個相機
		if (dead.Equals("DEAD") && Input.GetKeyDown (KeyCode.F)) {
			if (cameraState == false) {
				transform.FindChild ("Camera").gameObject.SetActive (false);
				GameObject.Find("StandByCamera").GetComponent<Camera>().enabled = true;
				cameraState = true;
			} else {
				transform.FindChild ("Camera").gameObject.SetActive (true);
				GameObject.Find("StandByCamera").GetComponent<Camera>().enabled = false;
				cameraState = false;
			}
		}
	}

	void OnCollisionStay () {
		grounded = true;    
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	void OnGUI(){
		GUI.Box (new Rect (10, 10, 100, 30), "Mode: " + dead);
	}

	[PunRPC]
	public void applyDead(){
			//把放炸彈跟人物顯示關掉
			gameObject.GetComponent<WebBomb> ().enabled = false;
			foreach (Transform child in transform) {
				if(child.gameObject.GetComponent<MeshRenderer> ()!=null)
					child.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				if(child.gameObject.GetComponent<SkinnedMeshRenderer> ()!=null)
					child.gameObject.GetComponent<SkinnedMeshRenderer> ().enabled = false;
			gameObject.SetActive (false);
			//3秒後重生
			Invoke ("ReSpawn", 3f);
			

		}
	}
	public void ReSpawn(){
		//重生
		//CANNOT SEEN BY OTHERS AND CANNOT PUT THE BOMB;
		gameObject.SetActive (true);
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		SpawnSpot newSpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		transform.position = newSpawn.transform.position;
		transform.rotation = newSpawn.transform.rotation;
		dead= "DEAD";
	}

}
