using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {
	
	public bool devilwalk;
	public bool speedup;
	public bool addBombStr;
	public bool addBombNum;
	public bool pushBomb;
	public float powerupLength;

	/* 定義道具旋轉速度 */
	public float RotateSpeed = 50f;
	/* 定義道具消失的時間 */
	public float DestroyTime = 5f;

	private PowerupManager manager;
	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<PowerupManager> ();
		//Destroy(gameObject, DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up*Time.deltaTime*RotateSpeed,Space.World);
	}
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			manager.ActivatePowerUp (devilwalk, speedup, addBombStr, addBombNum, pushBomb,
									 powerupLength, other.GetComponent<RigidBodyFPSWalker>());
		} else if (other.CompareTag("Explosion")) {
			Destroy(gameObject);
		}
		gameObject.SetActive (false);
	}
}
