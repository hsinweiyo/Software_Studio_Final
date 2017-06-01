using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	/* 倒著走 */
	public bool devilwalk;
	/* 加速 */
	public bool speedup;
	/* 增加炸彈威力 */
	public bool addBombStr;
	/* 增加可以放的炸彈數 */
	public bool addBombNum;
	/* 可以推炸彈 */
	public bool pushBomb;
	/* 可以丟炸彈 */
	public bool throwBomb;
	/* 道具可以持續的時間 */
	public float powerupLength;
	/* 定義道具旋轉速度 */
	public float RotateSpeed = 50f;
	/* 定義道具消失的時間 */
	public float DestroyTime = 5f;
	/* 道具控制管理員 */
	private PowerUpManager manager;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		/* 讓道具旋轉 */
		transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed,Space.World);
	}
	void OnTriggerEnter(Collider other) {
		/* 人撿到道具 */
		if (other.CompareTag("Player")) {
			/* manager 控制道具功能 */
			manager = other.gameObject.GetComponent<PowerUpManager> ();
			/* 確認 manager 有得到值 */
			if (manager != null) {
				/* 傳進去所有可能的道具參數 */
				manager.ActivatePowerUp (devilwalk, speedup, addBombStr, addBombNum, pushBomb, throwBomb, powerupLength);
				/* 把道具吃掉 */
				Destroy (gameObject);
			}
		} else if (other.CompareTag("Explosion")) {
			/* 道具被炸彈炸到就不見了 */
			Destroy(gameObject);
		}
		gameObject.SetActive (false);
	}
}