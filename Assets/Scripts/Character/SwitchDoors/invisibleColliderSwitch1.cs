using UnityEngine;
using System.Collections;

public class invisibleColliderSwitch1 : MonoBehaviour {

	public static bool mvdFrmSwitch=false;
	private bool beingHandled = false;

	// Use this for initialization// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D( Collider2D col) {  
		if (col.gameObject.name == "pf_Character(Clone)"&&DynamicSwitch.currSprite1==true&& !beingHandled ){
			//DynamicSwitch.charOnSwitch=false;
			StartCoroutine(HandleIt());

		}else
		if (col.gameObject.name == "pf_Character(Clone)"&&DynamicSwitch.currSprite1==false) { 
		//	print ("collided with invisble collider");
			//print ("value of charOnSwitch"+DynamicSwitch.charOnSwitch);
		}
		if (col.gameObject.name == "pf_Character(Clone)"&&StaticSwitch.currSprite1==true) { 
			print ("collided with collider STATIC 2nd time!!!!!!!!! ");
			StaticSwitch.charOnSwitch=false;
			
			//print ("value of charOnSwitch"+StaticSwitch.charOnSwitch);
		}else
		if (col.gameObject.name == "pf_Character(Clone)"&&StaticSwitch.currSprite1==false) { 
			print ("collided with invisble collider");
			//print ("value of charOnSwitch"+StaticSwitch.charOnSwitch);
		}

		
}
	private IEnumerator HandleIt()
	{
		beingHandled = true;
		print(Time.time);

		yield return new WaitForSeconds( 2.0f );
		DynamicSwitch.charOnSwitch=false;
		beingHandled = false;
		print(Time.time);
	}
}
