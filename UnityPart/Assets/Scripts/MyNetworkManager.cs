using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyNetworkManager : Photon.PunBehaviour{
	public Camera standbyCamera;
	SpawnSpot[] spawnSpots;
	// Use this for initialization
	int numberPlayers;
	void Start () {
		//PhotonNetwork.offlineMode = true;
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		PhotonNetwork.ConnectUsingSettings ("Logic Bomb 1.0");

	}

	public override void OnConnectedToMaster(){
		PhotonNetwork.JoinRandomRoom ();
	}
	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}
	public override void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}
	void OnPhotonRandomJoinFailed(){
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		SpawnMyplayer ();
	}
	public void SpawnMyplayer()
	{
		if (spawnSpots == null) {
			Debug.LogError ("no SpawnSpot");
			return;
		}
		Debug.Log (spawnSpots.Length);
		CheckPlayers ();
		SpawnSpot mySpawnSpot = spawnSpots[numberPlayers%spawnSpots.Length];
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("logicman2", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation,0);
		standbyCamera.enabled = false;

		//ENABLED THE DISABLED COMPONENT IN THE PLAYER
		//為了不會控制到別人的PLAYER
		myPlayerGO.GetComponent<RigidBodyFPSWalker> ().enabled = true;
		myPlayerGO.GetComponent<WebBomb> ().enabled = true;
		myPlayerGO.GetComponent<playertransition> ().enabled = true;
		myPlayerGO.transform.FindChild ("Camera").gameObject.SetActive (true);
	}
	/*public void enabledCamera(){
		standbyCamera.enabled = true;
	}
	public void disabledCamera(){
		standbyCamera.enabled = false;
	}*/
	void CheckPlayers()
	{
		//CHECK HOW MANY PLAYERS IN THE ROOM
		numberPlayers = PhotonNetwork.countOfPlayers;
		//if the number of player is heigher than the number of spawnpoint in the game (in this case 4),
		//spawn the players in round order
		/*for (int i = 0; i <= numberPlayers; i++)
		{
			if (numberPlayers > 4)
			{
				numberPlayers = numberPlayers % 4+1;
			}

		}*/
	}
}
