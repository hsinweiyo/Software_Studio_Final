using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {
	/*
	GameObject mainCamera;
	public bool carrying;
	GameObject carryObject;
	public float distance;*/
	GameObject mainCamera;
	GameObject carriedObject;
	bool carrying;
	public float distance;
	public float smooth;
	public float rayDistance;
	float startYRotation;
	float deltaRotation;
	float yRotation;
	float previousUp;
	Quaternion offset;
	// Use this for initialization
	void Start () {
		carriedObject = GameObject.FindWithTag ("test");
	}

	// Update is called once per frame
	/*
	void Update () {
		if (mainCamera == null) {
			mainCamera = GameObject.FindWithTag ("PlayerCamera");
		}
		if (carryObject == null) {
			carryObject = GameObject.FindWithTag ("test");
		}
		if (carrying) {
			carry (carryObject);

		} else {
			pickup ();

		}
	}
	void carry(GameObject obj){
		obj.GetComponent<Rigidbody> ().isKinematic = true;
		obj.transform.position = mainCamera.transform.position + mainCamera.transform.forward;

	}
	void pickup() {
		if (Input.GetKeyDown (KeyCode.E)) {
			//Debug.Log ("Press");
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera> ().ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				//if(p != null) {
					carrying = true;
					carryObject = p.gameObject;
				//}
			}
		}

	}*/
	void Update () {
		if (mainCamera == null) {
			mainCamera = GameObject.FindWithTag ("PlayerCamera");
		}
		if (carriedObject == null) {
			carriedObject = GameObject.FindWithTag ("test");
		}
		if (carrying) {
			carry (carriedObject);
			checkDrop ();
		} else {
			pickup ();
		}
	}

	void carry(GameObject o) {
		o.transform.position = Vector3.Lerp (o.transform.position, mainCamera.transform.position +   mainCamera.transform.forward * distance, Time.deltaTime*smooth);

		deltaRotation = previousUp - mainCamera.transform.eulerAngles.y;
		yRotation = startYRotation - deltaRotation;

		Quaternion target = Quaternion.Euler (0, yRotation, 0);
		o.transform.rotation = Quaternion.Slerp (o.transform.rotation, target, Time.deltaTime * 3);
	}

	void pickup() {
		if(Input.GetKeyDown (KeyCode.E)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;

			//if(Physics.Raycast(ray, out hit,rayDistance)) {
				//Pickupable p = hit.collider.GetComponent<Pickupable>();

				//if(p!=null) {
					//if (Input.GetKeyDown (KeyCode.E)) {
						carrying = true;
						//carriedObject = p.gameObject;
						//p.GetComponent<Rigidbody> ().useGravity = false;
			carriedObject = transform.GetComponent<Rigidbody>().gameObject;
						previousUp = mainCamera.transform.eulerAngles.y; 
						startYRotation = carriedObject.transform.eulerAngles.y;

					//}
				//}
			//}
		}
	}

	void checkDrop() {
		if(Input.GetKeyDown (KeyCode.E)){
			dropObject();
		}
	}


	void dropObject () {
		carrying = false;
		carriedObject.GetComponent<Rigidbody> ().useGravity = true;
		carriedObject = null;
	}
}
