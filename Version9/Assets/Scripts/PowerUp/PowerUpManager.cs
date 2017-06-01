using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
	private bool powerupActive;

	private bool devilwalk;
	private bool speedup;
	private bool addBombStr;
	private bool addBombNum;
	private bool pushBomb;
	private float powerupLengthCounter;
	private AbilityController logicman;
	// Use this for initialization
	void Start () {
		logicman = gameObject.GetComponent<AbilityController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (powerupActive) {
			powerupLengthCounter -= Time.deltaTime;

			if (speedup) {
				logicman.AddSpeed();
				speedup = false;
				powerupActive = false;
			}
			if (devilwalk) {
				logicman.DoDevil();
				devilwalk = false;
			}
			if (addBombStr) {
				logicman.AddBombStr ();
				addBombStr = false;
			}
			if (addBombNum) {
				logicman.AddBombNum ();
				addBombNum = false;
			}
			if (pushBomb) {
				logicman.doPushable ();
				pushBomb = false;
			}

			if (powerupLengthCounter <= 0) {
				if (logicman.isNeg()) {
					logicman.DoDevil ();
				}
				if (logicman.Pushable) {
					logicman.doPushable();
				}
				devilwalk = false;
				powerupActive = false;
			}
		}
	}
	public void ActivatePowerUp(bool point, bool speed, bool strength, bool num, bool push, float time) {
		devilwalk  = point;
		speedup    = speed;
		addBombNum = num;
		addBombStr = strength;
		pushBomb   = push;
		powerupActive = true;
		powerupLengthCounter = time;
	}
}
