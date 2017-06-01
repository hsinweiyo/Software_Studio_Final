using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBomb : MonoBehaviour {

	private AbilityController logicman;

	void Start()
	{
		/* 拿到 player object */
		logicman = gameObject.GetComponent<AbilityController> ();
	}
		
	void Update () {
		/* 檢查可不可以放炸彈 */
		if (Input.GetMouseButtonDown (0) && transform.GetComponent<RigidBodyFPSWalker>().grounded && logicman.isAbaleBomb()) {
			/* 可放炸彈數減少 */
			logicman.useBomb ();
			/* 放炸彈 */
			DropBumb ();
			/* 可以放的炸彈數在炸彈引爆後恢復 */
			Invoke("plusBombNum", 3f);
		}
	}
	public void DropBumb()
	{
		Vector3 newPos = new Vector3 ( transform.position.x,  transform.position.y-1f,  transform.position.z);
		/* 複製炸彈 (包括底下所有的 component) */
		GameObject bomb = PhotonNetwork.Instantiate("Bomb", newPos, transform.rotation,0);
		/* 檢查是有成功生成炸彈的 */
		if (bomb != null) {
			/* 把炸彈底下的 WebExplode 裡面的炸彈威力設定為放炸彈者的炸彈威力 */
			WebExplode exploder = bomb.GetComponent<WebExplode> ();
			exploder.bombStr  = logicman.strengthOfBomb;
		}
	}

	//player 碰到炸彈後會死掉
	public void OnTriggerEnter(Collider other){
		/* 人撞到 Explosion 會死掉 */
		if (other.CompareTag ("Explosion") && gameObject.GetComponent<RigidBodyFPSWalker>().dead.Equals("LIVE")) {
			Debug.Log ("hit");
			/* 呼叫在 RigidbodyFPSWalker 的 applyDead */
			transform.GetComponent<PhotonView> ().RPC ("applyDead", PhotonTargets.AllBuffered);

		} /*
		if (other.CompareTag ("Glove")&& gameObject.GetComponent<RigidBodyFPSWalker>().pickUp==false) {
			Debug.Log ("hit");
			//call在 RigidbodyFPSWalker
			Vector3 pos = new Vector3(0,2f,0); 
			other.gameObject.transform.position = transform.position + pos;
			other.gameObject.GetComponent<SphereCollider> ().enabled = false;
			other.gameObject.transform.parent = transform;
			gameObject.GetComponent<RigidBodyFPSWalker>().pickUp=true;
		} */

	}

	void plusBombNum() {
		logicman.releaseBomb ();	
	}
}
