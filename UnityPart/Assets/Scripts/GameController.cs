using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	//public RigidBodyFPSWalker body;
	// Use this for initialization
	void Start () {
		//body = transform.GetComponent<RigidBodyFPSWalker> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Y)&& transform.GetComponent<RigidBodyFPSWalker>().grounded) {
			//SpeedUp ();
		}
	}
	void ChangeSpeed(float speed) {
		transform.GetComponent<RigidBodyFPSWalker> ().speed = speed;
	}
}
