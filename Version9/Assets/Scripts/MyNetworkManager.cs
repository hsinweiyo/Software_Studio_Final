using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyNetworkManager : Photon.PunBehaviour{
	public Camera standbyCamera;
	SpawnSpot[] spawnSpots;
	public string roomName = "Room 01";
	public string playerName = "Player 001";

	public bool isConnected = false;
	public bool isInRoom= false;

	int[] playerUsed ;

	public int playerPrefabsIndex=0;
	//player reference
	public InputField nameInputField;
	public InputField roomInputField;

	public RectTransform MenuPanel;
	public RectTransform RoomListPanel;
	public RectTransform RoomPanel;
	// Use this for initialization
	public int numberPlayers;

	//Button
	/*public Texture joinButtonImg;*/
	public GUIStyle buttonStyle;
	public Button joinButton;
	public Button LeaveButton;
	public Button playerButton;

	//Sprites
	public Sprite Player1;
	public Sprite Player2;
	public Sprite Player3;
	public Sprite Player4;

	//Buttons
	public void OnStart () {
		PhotonNetwork.autoJoinLobby = true;
		roomName = "Room " + Random.Range (0, 999);
		//PhotonNetwork.offlineMode = true;
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		PhotonNetwork.ConnectUsingSettings ("Logic Bomb 1.0");
		playerUsed = new int[10] ;
		MenuPanel.gameObject.SetActive (false);
		RoomListPanel.gameObject.SetActive (true);

		nameInputField.onEndEdit.RemoveAllListeners ();
		nameInputField.text = "Player " + Random.Range (0, 999); 
		roomInputField.onEndEdit.RemoveAllListeners ();
		roomInputField.text = "Room " + Random.Range (0, 999); 
	}

	public void OnCreateButton(){
		playerName = nameInputField.text;
		roomName = roomInputField.text;
		//RoomListPanel.gameObject.SetActive (false);
		PhotonNetwork.JoinOrCreateRoom (roomName,null,null);
	}

	public void OnJoinButton(){
		//SpawnMyplayer ();
		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
		hash.Add ("state", "start");
		PhotonNetwork.room.SetCustomProperties (hash);
	}

	public void OnLeaveButton(){
		isInRoom = false;
		joinButton.gameObject.SetActive (false);
		RoomPanel.gameObject.SetActive (false);
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		PhotonNetwork.LeaveRoom ();
		OnLeftRoom ();
	}

	public void OnPlayerButton(){
		//can't choose the same player
		playerPrefabsIndex=(playerPrefabsIndex+1)%4;
		if (playerUsed [playerPrefabsIndex] == 1) {
			playerPrefabsIndex=(playerPrefabsIndex+1)%4;;
			for (int i = 0; i <3; i++) {
				if (playerUsed [playerPrefabsIndex] != 1)
					break;
				else
					playerPrefabsIndex=(playerPrefabsIndex+1)%4;;
			}
		}
			
		ExitGames.Client.Photon.Hashtable index = new ExitGames.Client.Photon.Hashtable ();
		index.Add ("PlayerIndex", playerPrefabsIndex);
		PhotonNetwork.player.SetCustomProperties (index);

		if (playerPrefabsIndex == 0)
			playerButton.GetComponent<Image> ().sprite = Player1;
		else if (playerPrefabsIndex == 1)
			playerButton.GetComponent<Image> ().sprite = Player2;
		else if (playerPrefabsIndex == 2)
			playerButton.GetComponent<Image> ().sprite = Player3;
		else
			playerButton.GetComponent<Image> ().sprite = Player4;
	}


	void Update(){

	}
	public override void OnConnectedToMaster(){
		OnJoinedLobby ();
		//isConnected = true;
	}
	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		if (isConnected) {
			GUILayout.BeginArea (new Rect (Screen.width / 2-250, Screen.height / 2-50,500,500));
			foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
				Debug.Log ("Room");
				if (GUILayout.Button (game.Name + " " + game.PlayerCount + "/" + game.MaxPlayers)) {
					//get the player name from the input field
					playerName = nameInputField.text;

					PhotonNetwork.JoinOrCreateRoom (game.Name,null,null);
				}
			}
			GUILayout.EndArea ();
		}
		if (isInRoom) {
			//LeaveButton.gameObject.SetActive (true);
			GUILayout.BeginArea (new Rect (Screen.width / 2-250, Screen.height / 2-200,500,500));
			if (PhotonNetwork.isMasterClient) {
				joinButton.gameObject.SetActive (true);
			}
			foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
				GUILayout.Box (pl.ID + " | " + pl.NickName,buttonStyle);				
			}
			// if the state of the room is start Game , the game start
			if (PhotonNetwork.room.CustomProperties ["state"].ToString () == "start") {
				//OnJoinButton ();
				SpawnMyplayer ();
				isInRoom = false;
			}
			GUILayout.EndArea ();
			//Update the player been choose
			for(int i=0;i<5;i++)
			{
				playerUsed [i] = 0;
				
			}
			foreach (PhotonPlayer player in PhotonNetwork.otherPlayers) {
				//Debug.Log (player.CustomProperties ["PlayerIndex"].GetHashCode());
				playerUsed [(int)player.CustomProperties ["PlayerIndex"]] = 1;
			}


		}

	}


	public override void OnJoinedLobby(){
		isConnected = true;
		RoomListPanel.gameObject.SetActive (true);
		Debug.Log ("Lobby");
		//PhotonNetwork.JoinRandomRoom ();
	}
	void OnPhotonRandomJoinFailed(){
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}
	public override void OnJoinedRoom()
	{
		RoomListPanel.gameObject.SetActive(false);
		RoomPanel.gameObject.SetActive (true);
		isInRoom = true;
		//set the room state to wait
		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
		hash.Add ("state", "wait");
		PhotonNetwork.room.SetCustomProperties (hash);

		//canit choose the same player;
		for(int i=0;i<5;i++)
		{
			playerUsed [i] = 0;

		}
		foreach (PhotonPlayer player in PhotonNetwork.otherPlayers) {
			//Debug.Log (player.CustomProperties ["PlayerIndex"].GetHashCode());
			playerUsed [(int)player.CustomProperties ["PlayerIndex"]] = 1;
		}
		if (playerUsed [playerPrefabsIndex] == 1) {
			playerPrefabsIndex=(playerPrefabsIndex+1)%4;;
			for (int i = 0; i <3; i++) {
				if (playerUsed [playerPrefabsIndex] != 1)
					break;
				else
					playerPrefabsIndex=(playerPrefabsIndex+1)%4;;
			}
		}
		ExitGames.Client.Photon.Hashtable index = new ExitGames.Client.Photon.Hashtable ();
		index.Add ("PlayerIndex", playerPrefabsIndex);
		PhotonNetwork.player.SetCustomProperties (index);

		if (playerPrefabsIndex == 0)
			playerButton.GetComponent<Image> ().sprite = Player1;
		else if (playerPrefabsIndex == 1)
			playerButton.GetComponent<Image> ().sprite = Player2;
		else if (playerPrefabsIndex == 2)
			playerButton.GetComponent<Image> ().sprite = Player3;
		else
			playerButton.GetComponent<Image> ().sprite = Player4;


		Debug.Log("OnJoinedRoom");
		PhotonNetwork.playerName = playerName;
		isConnected = false;


	}

	public void SpawnMyplayer()
	{
		RoomPanel.gameObject.SetActive (false);
		if (spawnSpots == null) {
			Debug.LogError ("no SpawnSpot");
			return;
		}
		Debug.Log (spawnSpots.Length);
		CheckPlayers ();
		Debug.Log (numberPlayers);

		//use the playerID to decide where to  spawn
		SpawnSpot mySpawnSpot = spawnSpots[playerPrefabsIndex%4];
		GameObject myPlayerGO;
		if(playerPrefabsIndex == 0)
			myPlayerGO = (GameObject)PhotonNetwork.Instantiate("Player", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation,0);
		else if(playerPrefabsIndex == 1)
			myPlayerGO = (GameObject)PhotonNetwork.Instantiate("logicman2", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation,0); 
		else if(playerPrefabsIndex == 2)
			myPlayerGO = (GameObject)PhotonNetwork.Instantiate("logicman2", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation,0);
		else
			myPlayerGO = (GameObject)PhotonNetwork.Instantiate("logicman2", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation,0);

		//ENABLED THE DISABLED COMPONENT IN THE PLAYER
		//為了不會控制到別人的PLAYER
		myPlayerGO.GetComponent<RigidBodyFPSWalker> ().enabled = true;
		myPlayerGO.GetComponent<WebBomb> ().enabled = true;
		//myPlayerGO.GetComponent<playertransition> ().enabled = true;
		myPlayerGO.transform.FindChild ("Camera").gameObject.SetActive (true);
		standbyCamera.enabled = false;
		
	}

	public int CheckReadyPlayers()
	{
		//CHECK HOW MANY PLAYERS ready IN THE ROOM
		int	readyPlayers = 0;
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			if (pl.CustomProperties["ready"] == "Ready")
				readyPlayers++;
			
		}
		return readyPlayers;

	}
	void CheckPlayers()
	{
		//CHECK HOW MANY PLAYERS IN THE ROOM
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			numberPlayers++;

		}
	}
}
