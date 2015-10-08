using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {
	public Transform youWinPose;
	public Transform creditsPose;
	public GameObject winPanel;
	public static bool isWon;
	// Use this for initialization
	void Start () {
		youWinPose.gameObject.SetActive (false);
		isWon = false;
		//creditsPose.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		PauseGame (isWon);

	}

	void PauseGame(bool state){
        //if (state) {
			
        //    Time.timeScale = 0.0f;
        //} 
        //else {
        //    Time.timeScale = 1.0f;
			
        //}
		winPanel.SetActive (state);
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("Entered Trigger");
		if (col.gameObject.name == "Character") {
			isWon=true;
			youWinPose.gameObject.SetActive (true);
			creditsPose.gameObject.SetActive (true);
		}
	}
}
