using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class DamagePlayer : MonoBehaviour {
	public Texture2D loseTex;

	public Transform youLooseTransform;


	// Use this for initialization
	void Start () {
		youLooseTransform.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Character") {
			//Player ran into spikes, so he lose the game
			Debug.Log ("You Lose");
			if(youLooseTransform != null){
				youLooseTransform.gameObject.SetActive(true);
				Destroy(GameObject.Find ("Character"));
				Destroy(GameObject.Find ("Gun"));
			}
		}
	}
}
