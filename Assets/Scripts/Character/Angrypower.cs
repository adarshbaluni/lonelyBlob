using UnityEngine;
using System.Collections;

public class Angrypower : MonoBehaviour {
	public static bool angry=false;
	public static bool sticky=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D  ( Collider2D col) {  
		if (this.gameObject.tag == "Angry_power") {
			if (col.gameObject.tag == "Player") { 
				Destroy (this.gameObject);
				angry = true;
				print ("Collider enter");
				CharacterControl.m_animator.Play ("AngryBlobwalk");
			
			}
		}
			else if(this.gameObject.name=="pf_sticky")
			{
				if (col.gameObject.name == "pf_Character") { 
					Destroy (this.gameObject);
					sticky= true;
					print ("Collider enter");
					CharacterControl.m_animator.Play ("AngryBlobwalk");

			}
			}
		
		
		}


}
