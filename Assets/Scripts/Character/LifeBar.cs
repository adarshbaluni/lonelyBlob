using UnityEngine;
using System;


public class LifeBar : MonoBehaviour {
	public Vector2 pos ;
	public Vector2 size ;
	public static bool lifeBarInitial=false;
	public static bool NoMoreCollide = false;
	public static bool OutOfLife=false;
	public float LifeBarIncreaseVar=.10f;
	public float LifeBarDecreaseVar=.009f;
	public float InitialLifeVal=.25f;

	//public float floatStrength=1;
	//public Vector2 floatStrength=1;
	//public float originalY;

	// Use this for initialization
	void Start () {
		pos = new Vector2(50,50);
		size = new Vector2(205,33);
		lifeBarInitial = true;

		//this.originalY =transform.position.y;

	}
	
	// Update is called once per frame
	public float barDisplay=.07f; //current progress
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public Texture2D warningTex;
	
	void OnGUI() {
		print ("coming here");
		//draw the background:
		//Color oldColor = GUI.color;
     	GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		//GUI.color = Color.white;
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, 120));
		//GUI.color = Color.blue;
		if (barDisplay <= .25f) {
			print ("coming here in warning");
			if (Time.time % 1< .65) {
			//GUI.color.a = 0;//Mathf.Lerp(1,0,Time.time*0.5);
			GUI.Box (new Rect (0, 0, 210, 120), warningTex);
			}
		
		} else {
			//GUI.color.a = 0;
			print ("coming here in normal");
			GUI.Box (new Rect (0, 0, 210, 120), fullTex);
		}
		GUI.EndGroup();
		GUI.EndGroup();
		//GUI.color = oldColor;
		
	}
	
	void Update() {

		//transform.position = new Vector3(transform.position.x,originalY + ((float)Mathf.Sin(Time.time) * floatStrength),transform.position.z);
		if (lifeBarInitial == true) {
			barDisplay = InitialLifeVal;
			lifeBarInitial=false;
			OutOfLife=false;
			NoMoreCollide=false;
			if (barDisplay>=1.0f) {
				NoMoreCollide=true;
			}

		}
		if (BlobRegen.life == true) {
			//print ("comiiiiiinng in blobregen");
			if (barDisplay < 1.0f) {
			barDisplay += LifeBarIncreaseVar;
            barDisplay = Mathf.Clamp(barDisplay, 0, 1);
			BlobRegen.life =false;
			OutOfLife=false;
			NoMoreCollide=false;
			}else if(barDisplay>=1.0f){
				NoMoreCollide=true;
			}


			
		}

		if(InputGestureBlobDraw.platformCreate){
			//print ("cominnng here");
			if(barDisplay>=0){
				barDisplay-=LifeBarDecreaseVar;
			}
			if (barDisplay<=0){
				OutOfLife=true;
			}
			if (barDisplay >=0.0f) {
				NoMoreCollide=false;
			}
		}
		if (barDisplay >= 1.0f) {
			NoMoreCollide=true;
		}



	}


	

}
