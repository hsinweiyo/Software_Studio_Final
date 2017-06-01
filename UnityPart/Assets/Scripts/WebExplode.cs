using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebExplode : MonoBehaviour {
	private bool exploded = false;
	public float moveSpeed = 10f;
	public int bombStr;

	private bool Moveable;
	private Vector3 bombDir;
	private RigidBodyFPSWalker logicman;
	// Use this for initialization
	void Start () {
		Invoke ("Explode", 5f);
		logicman = FindObjectOfType<RigidBodyFPSWalker> ();
	}

	// Update is called once per frame
	void Update () {
		bombStr = logicman.strengthOfBomb;

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

		logicman.releaseBomb();
	}

	//COROUTINE FUNCTION
	private IEnumerator CreateExplosion (Vector3 direction){
		for (int i = 1; i < bombStr; i++) {
			RaycastHit hit;
			Physics.Raycast (transform.position + new Vector3 (0, .78f, 0), direction, out hit, i*1.5f);
			//Debug.Log ("Tag:" + hit.collider.gameObject.tag);
			if (!hit.collider || hit.transform.tag == ("Treasure") || hit.transform.tag == "Player") {
				//生成火光
				PhotonNetwork.Instantiate ("Explosion", transform.position + i* direction, transform.rotation, 0);
			} else {
				Debug.Log (hit.transform.tag);
				/*if (hit.transform.tag == "Player") {

					hit.collider.transform.GetComponent<PhotonView> ().RPC ("applyDead", PhotonTargets.AllBuffered,null);
				}*/
				break;
			}
		}
		yield return new WaitForSeconds (.05f);
	}

	//炸彈炸到炸彈會一起爆炸，防止二次爆炸
	public void OnTriggerEnter(Collider other){
		if (!exploded && other.CompareTag ("Explosion")) {
			CancelInvoke ("Explode");
			Explode();
		}

	}

	public void OnCollisionEnter(Collision other) {
		if (other.rigidbody.CompareTag ("Player") && logicman.Pushable) {
			Moveable = true;
			//bombDir = other.relativeVelocity;
			bombDir = -other.contacts [0].point + transform.position;
			//bombDir.Normalize ();
		} else {
			Moveable = false;
		}
	}
}
