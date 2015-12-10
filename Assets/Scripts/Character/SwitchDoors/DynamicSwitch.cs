using UnityEngine;
using System.Collections;

public class DynamicSwitch : MonoBehaviour {

	public static bool openDynamicDoor=false;
	public  static SpriteRenderer spriteRenderer;
	public  Sprite sprite1; 
	public  Sprite sprite2;
	public static bool currSprite1=false;
	public static bool charOnSwitch=false;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
			spriteRenderer.sprite = sprite1; // set the sprite to sprite1
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//print ("current sprite" + spriteRenderer.sprite);)

		if(spriteRenderer.sprite ==sprite2)
			currSprite1=true;
		ChangeTheSprite ();
			
		
	}
	void OnTriggerEnter2D( Collider2D col) {  
		
		if (col.gameObject.name == "pf_Character(Clone)") {
		//	print ("hitting the switch");
			charOnSwitch=true;
			//print ("value of  charOnSwitch"+charOnSwitch);
	}
	
}
	void ChangeTheSprite(){
		if (charOnSwitch) {
			spriteRenderer.sprite = sprite2;
		}
		else
		if (spriteRenderer.sprite == sprite1 && !charOnSwitch) {
			spriteRenderer.sprite = sprite1;

		}
		else
		
		if (spriteRenderer.sprite == sprite2 && !charOnSwitch) {
			spriteRenderer.sprite = sprite1;
			
		}
			
	}
}
