using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBomb : MonoBehaviour {
	private RigidBodyFPSWalker logicman;

	// Use this for initialization

	void Start(){
		logicman = transform.GetComponent<RigidBodyFPSWalker> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)&& logicman.grounded && logicman.isAbaleBomb()) {
			logicman.useBomb();
			DropBumb ();
		}
	}
	public void DropBumb()
	{
		
		Vector3 newPos = new Vector3 ( transform.position.x+4f,  transform.position.y-1f,  transform.position.z+1.5f);
		//生成炸彈
		PhotonNetwork.Instantiate("Bomb", newPos, transform.rotation,0);
	}

	//player 碰到炸彈後會死掉
	public void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Explosion")&& gameObject.GetComponent<RigidBodyFPSWalker>().dead.Equals("LIVE")) {
			Debug.Log ("hit");
			//call在 RigidbodyFPSWalker 的 applyDead
			transform.GetComponent<PhotonView> ().RPC ("applyDead", PhotonTargets.AllBuffered);

		} 

	}
}
