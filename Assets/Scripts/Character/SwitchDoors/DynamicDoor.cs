using UnityEngine;
using System.Collections;

public class DynamicDoor : MonoBehaviour {

	public SpriteRenderer spriteRenderer; 
	public Vector3 originalSize;
	public Vector3 originalPos;
	public float doorSpeed=50f;



	// Use this for initialization
	void Start () {
		originalSize = transform.localScale;
		originalPos = transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		if (DynamicSwitch.charOnSwitch) {

			transform.position += new Vector3(0.0f, doorSpeed*Time.deltaTime*5, 0.0f);
		}
		else
		if (!DynamicSwitch.charOnSwitch&&transform.position.y>originalPos.y) {

			//print ("decreases the pos of dynamic");
			transform.position -= new Vector3(0.0f, doorSpeed*Time.deltaTime*10, 0.0f);
			//transform.position = originalPos;

			
			
			
		}
		
	}



}
