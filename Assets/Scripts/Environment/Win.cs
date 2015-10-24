using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {
	public Transform creditsPose;
	public GameObject winPanel;
	public static bool isWon;

	// Use this for initialization
	void Start () {
		isWon = false;
        winPanel = UIManager.s_winPanel;
	}
	
	// Update is called once per frame
	void Update () {
		PauseGame (isWon);
	}

	void PauseGame(bool state){
		winPanel.SetActive (state);
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("Entered Trigger");
		if (col.gameObject.tag == "Player") {
			isWon=true;
		}
	}
}
