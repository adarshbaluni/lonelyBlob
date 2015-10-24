using UnityEngine;
using System.Collections;

public class invisibleCollider1 : MonoBehaviour {
	public static bool printMessage=true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D( Collider2D col) {  
		
		if (col.gameObject.name == "pf_Character") { 
			//print("Collider with invisible");
			printMessage=false;
			
		}
	}
}
