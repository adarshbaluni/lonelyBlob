using UnityEngine;
using System.Collections;

public class DynamicDoor : MonoBehaviour {

	public SpriteRenderer spriteRenderer; 
	public Vector3 originalSize;
	// Use this for initialization
	void Start () {
		originalSize = transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (DynamicSwitch.openStaticDoor&&!invisibleColliderSwitch1.mvdFrmSwitch) {
			print ("hellloooo1");
			transform.localScale -= new Vector3(.5f, 0, 0);		
		}
		
		if (DynamicSwitch.openStaticDoor&&invisibleColliderSwitch1.mvdFrmSwitch) {
			print ("hellloooo2");
			
			transform.localScale = 	originalSize;
			
			
		}
		if (invisibleColliderSwitch1.mvdFrmSwitch && transform.localScale == originalSize) {
			print ("hellloooo3");
			invisibleColliderSwitch1.mvdFrmSwitch = false;
			DynamicSwitch.openStaticDoor = false;
		}
		
		
	}
}
