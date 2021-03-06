﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {
	private const float MaxWalk    = 15f;
	private const int 	MaxBombNum = 10;
	private const int 	MaxBombStr = 20;

	public float speed;
	public int numberOfBomb;
	public int currentBomb;
	public int strengthOfBomb;
	public bool Pushable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddSpeed()
	{
		if (speed < MaxWalk) {
			speed += 2f;
		}
	}

	public void useBomb()
	{
		currentBomb--;
	}

	public void releaseBomb()
	{
		currentBomb++;
	}
	public bool isAbaleBomb()
	{
		if (currentBomb > 0) {
			return true;
		} else {
			return false;
		}
	}
	public void AddBombNum()
	{
		if (numberOfBomb < MaxBombNum) {
			numberOfBomb++;
			currentBomb++;
		}
	}

	public void AddBombStr()
	{
		if (strengthOfBomb < MaxBombStr) {
			strengthOfBomb += 2;
		}
	}
	public void doPushable()
	{
		Pushable = !Pushable;
	}

	public void DoDevil()
	{
		speed = -speed;
	}

	public bool isNeg()
	{
		if (speed > 0f) {
			return false;
		} else {
			return true;
		}
	}
	/*
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody item = hit.collider.attachedRigidbody;
		if (item.CompareTag ("Bomb") && Pushable) {
			Vector3 direction = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
			item.velocity = direction * 6f;
		} else {
			return;
		}
	}*/
}
