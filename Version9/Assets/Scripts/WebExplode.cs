﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebExplode : MonoBehaviour {
	private bool exploded = false;
	/* The limit length of explosion */
	public int bombStr;
	/* Use this for initialization */
	public bool Moveable;

	public float moveSpeed = 10f;
	private Vector3 bombDir;
	void Start () {
		Invoke ("Explode", 4f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Moveable) {
			transform.Translate (bombDir * moveSpeed * Time.deltaTime);
		}
	}
	void Explode(){
		PhotonNetwork.Instantiate ("Explosion", transform.position, transform.rotation, 0);

		//跑四個方向爆炸
		StartCoroutine(CreateExplosion(Vector3.forward));
		StartCoroutine(CreateExplosion(Vector3.right));
		StartCoroutine(CreateExplosion(Vector3.left));
		StartCoroutine(CreateExplosion(Vector3.back));

		//把炸彈隱形(但其實還存在)
		GetComponent<MeshRenderer> ().enabled = false;
		exploded = true;
		//關掉COLLIDER
		//transform.FindChild ("Collider").gameObject.SetActive (false);
		Destroy (gameObject, .3f);

	}

	//COROUTINE FUNCTION
	private IEnumerator CreateExplosion (Vector3 direction){
		/* create fire depend on the bomb strength */
		for (int i = 1; i < bombStr; i++) {
			RaycastHit hit;
			Physics.Raycast (transform.position + new Vector3 (0, .78f, 0), direction, out hit, i*1.5f);
			if (!hit.collider || hit.transform.tag == "Player") {
				//生成火光
				PhotonNetwork.Instantiate ("Explosion", transform.position + i* direction, transform.rotation, 0);
			} else {
				/* Apply death at webbomb */
				Debug.Log (hit.transform.tag);
				break;
			}
		}
		yield return new WaitForSeconds (.05f);
	}

	/* 炸彈炸到炸彈會一起爆炸，防止二次爆炸 */
	public void OnTriggerEnter(Collider other){
		if (!exploded && other.CompareTag ("Explosion")) {
			CancelInvoke ("Explode");
			Explode ();
		} 
	}

	void OnTriggerExit(Collider other) {
		Invoke ("triggerBomb", 0.5f);
	}

	public void OnCollisionEnter(Collision other) {
		if (other.rigidbody.CompareTag ("Player") && other.rigidbody.GetComponent<AbilityController> ().Pushable) {
			Moveable = true;
			Vector3 pos = new Vector3 (transform.position.x - other.rigidbody.position.x, 0f, transform.position.z - other.rigidbody.position.z);
			bombDir = pos;
		} else {
			Moveable = false;
		}
	}

	private void triggerBomb() {
		gameObject.GetComponent<SphereCollider> ().isTrigger = false;
	}
}
