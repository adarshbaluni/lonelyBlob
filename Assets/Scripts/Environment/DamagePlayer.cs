using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class DamagePlayer : MonoBehaviour {
	public Texture2D loseTex;
	public GameObject losePanel;
	public static bool isLost;
	public Transform youLooseTransform;


	// Use this for initialization
	void Start () {
		youLooseTransform.gameObject.SetActive (false);
		isLost = false;
	}
	
	// Update is called once per frame
	void Update () {
		PauseGame (isLost);
	}

	void PauseGame(bool state){

        //if (state) {
			
        //    Time.timeScale = 0;
			
        //}else {
			
        //    Time.timeScale = 1;
			
        //}
		
		losePanel.SetActive (state);

	}

	void OnCollisionEnter2D(Collision2D col){
		
		if (col.gameObject.name == "Character") {
			
			//Player ran into spikes, so he lose the game
			Debug.Log ("You Lose");
			if(youLooseTransform != null){
				isLost = true;
				youLooseTransform.gameObject.SetActive(true);
				Destroy(GameObject.Find ("Character"));
				Destroy(GameObject.Find ("Gun"));
			}
		}
		
	}



}
