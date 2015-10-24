using UnityEngine;
using System.Collections;

public class invisibleCollider2 : MonoBehaviour {

	public static bool printMessage1=true;
	// Use this for initialization// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D( Collider2D col) {  
		
		if (col.gameObject.name == "pf_Character") { 
			print("Collider with invisible");
			printMessage1=false;
			
		}
	}
}
