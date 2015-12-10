using UnityEngine;
using System.Collections;

public class StaticDoor : MonoBehaviour {

		
		public SpriteRenderer spriteRenderer; 
		public Vector3 originalSize;
		public Vector3 originalPos;
		public float doorSpeed=50f;
		
		
	bool stopMove=false;
		
		// Use this for initialization
		void Start () {
			originalSize = transform.localScale;
			originalPos = transform.position;
		}
		
		// Update is called once per frame
		void Update () {
		if (StaticSwitch.charOnSwitch) {
			//&StaticSwitch.currSprite1 == false
			//print ("why its coming here");
				//transform.position += new Vector3 (0.0f, doorSpeed* Time.deltaTime , 0.0f);
			transform.position += new Vector3(0.0f, doorSpeed*Time.deltaTime*15, 0.0f);
		
			} 

	}
}


	

