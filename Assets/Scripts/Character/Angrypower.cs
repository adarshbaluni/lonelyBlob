using UnityEngine;
using System.Collections;

public class Angrypower : MonoBehaviour {
	public static bool angry=false;
	public static bool sticky=false;
	public static float  time ;

	// Use this for initialization
	void Start () {
	time=16f;
	}
	
	// Update is called once per frame
	void Update () {
	Debug.Log(angry);
		if (angry) {
		Debug.Log(time);
			time -= Time.deltaTime;
			if(time<0){
	Debug.Log("Hey");
				CharacterControl.m_animator.Play ("lAND");
				angry=false;
CharacterControl.timerTextt.text="";
CharacterControl.totalTimee=16f;
time=10f;
			}
				}

	



}

	void OnTriggerEnter2D  ( Collider2D col) {  
		if (this.gameObject.tag == "Angry_power") {
			if (col.gameObject.tag == "Player" ) { 
				Destroy (this.gameObject);
				angry = true;
Debug.Log(angry);
				GameObject.FindWithTag("MagmaWall").GetComponent<BoxCollider2D>().isTrigger=true;
				print ("Collider enter");
				CharacterControl.m_animator.Play ("AngryBlobwalk");
			
			}
		}
	}	
}
