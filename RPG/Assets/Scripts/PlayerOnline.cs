using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerOnline : MonoBehaviour {

	public PhotonPlayer Player { get; set;}
	public string PlayerName { get; set;}

	private Coroutine routine;

	void Start(){
		transform.GetChild (0).GetComponent<Text> ().text = PlayerName;
	}

	void FixedUpdate(){

		List<PhotonPlayer> players = new List<PhotonPlayer> (PhotonNetwork.otherPlayers);

		if (!players.Contains (Player)) {
		
			DestroyImmediate (this.gameObject);
		}
	}
		
	public void InvitePlayer(){
	
		transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = "Waiting";
		transform.GetChild (1).GetComponent<Button> ().interactable = false;
		object[] content = new object[] {""};
		RaiseEventOptions opt = new RaiseEventOptions ();
		opt.TargetActors = new int[1]{this.Player.ID};
		PhotonNetwork.RaiseEvent (100, content, true, opt);

		this.routine = StartCoroutine (Wait (11.0f));
	}

	IEnumerator Wait(float time){
	
		yield return new WaitForSeconds (time);

		transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = "Invite";
		transform.GetChild (1).GetComponent<Button> ().interactable = true;

		routine = null;
	}


	public void JoinedGroup(){

		if(routine!=null)
		StopCoroutine (routine);

		transform.GetChild (1).GetComponent<Button> ().interactable = false;
		transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = "In Group";
	
		Group.k_Group.AddToGroup (Player);

		CreationManager.k_Manager.ResetPositions ();

	}
}
