using UnityEngine;
using System.Collections;

public class DynamicSwitch : MonoBehaviour {

	public static bool openStaticDoor=false;
	public  SpriteRenderer spriteRenderer;
	public Sprite sprite1; 
	public Sprite sprite2;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
			spriteRenderer.sprite = sprite1; // set the sprite to sprite1
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (invisibleColliderSwitch1.mvdFrmSwitch)
			
			ChangeTheSprite ();
		
	}
	void OnTriggerEnter2D( Collider2D col) {  
		
		if (col.gameObject.name == "pf_Character") { 
			ChangeTheSprite();
			print("AJAJAAA");
			openStaticDoor=true;
			
		}
	}
	void ChangeTheSprite ()
	{
		if (!invisibleColliderSwitch1.mvdFrmSwitch&&spriteRenderer.sprite == sprite1) // if the spriteRenderer sprite = sprite1 then change to sprite2
		{
			spriteRenderer.sprite = sprite2;
		}
		else
			
		{
			spriteRenderer.sprite = sprite1; // otherwise change it back to sprite1
		}
	}
}
