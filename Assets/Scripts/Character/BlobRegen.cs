using UnityEngine;
using System.Collections;

public class BlobRegen : MonoBehaviour {
	public static bool life=false;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D ( Collider2D col) {  

		if (col.gameObject.name == "Character"&& LifeBar.NoMoreCollide!=true) { 
			print("Collider enter");
			life=true;
			Destroy (gameObject);	
		}
	}
}
