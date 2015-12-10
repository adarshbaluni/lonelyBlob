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
	public bool landscape=false;
    //Audio
    public AudioSource RegenAudio;
	//public float floatStrength=1;
	//public Vector2 floatStrength=1;
	//public float originalY;

	// Use this for initialization
	void Start () {
		if (Screen.width > Screen.height) {
			//print ("in landscape");
			landscape = true;
		} else {
			//print("in potrait");
		}

		pos = new Vector2((float)0.05*Screen.width,(float)0.0118*Screen.height);
	

		//print ("Width=" + Screen.width + "Length=" + Screen.height);
		if (landscape) {
			//print("in landscape");
			size = new Vector2 ((float)0.21 * Screen.width, (float).078 * Screen.height);
		}
		else
			if(!landscape)
			size = new Vector2((float)0.2*Screen.width,(float)0.04*Screen.height);
		lifeBarInitial = true;

		//this.originalY =transform.position.y;

	}
	
	// Update is called once per frame
	public float barDisplay=.07f; //current progress
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public Texture2D warningTex;
	
	void OnGUI() {
		//print ("sizeof  here");
		//draw the background:
		//Color oldColor = GUI.color;
     	GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		//GUI.color = Color.white;
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		if(landscape)
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, (float).078*Screen.height));
		else
			if(!landscape)
				GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, (float).04*Screen.height));

		//GUI.color = Color.blue;
		if (barDisplay <= .25f) {
			//print ("coming here in warning");
			if (Time.time % 1< .65) {
			//GUI.color.a = 0;//Mathf.Lerp(1,0,Time.time*0.5);
				if(landscape)
				GUI.Box (new Rect (0, 0,(float) 0.21*Screen.width, (float).078*Screen.height), warningTex);
				else
					if(!landscape)
						GUI.Box (new Rect (0, 0,(float) 0.2*Screen.width, (float)0.04*Screen.height), warningTex);

			}
		
		} else {
			//GUI.color.a = 0;
			//print ("coming here in normal");
			if(landscape)
				GUI.Box (new Rect (0, 0,(float) 0.21*Screen.width, (float).078*Screen.height), fullTex);
			else
				if(!landscape)
					GUI.Box (new Rect (0, 0,(float) 0.2*Screen.width, (float)0.04*Screen.height), fullTex);

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
                RegenAudio.Play();
			}else if(barDisplay>=1.0f){
				NoMoreCollide=true;
			}


			
		}

		//&& Input.touchCount==1
	if (!InputGestureBlobDraw.Draw) {
			if (InputGestureBlobDraw.platformCreate && Input.touchCount==1)

				if (barDisplay >= 0) {
					barDisplay -= LifeBarDecreaseVar;
				}
				if (barDisplay <= 0) {
					OutOfLife = true;
				}
				if (barDisplay >= 0.0f) {
					NoMoreCollide = false;
				}

			}
		 else {

			if (InputGestureBlobDraw.platformCreate) {// && Input.touchCount==1)
				
				if (barDisplay >= 0) {
					barDisplay -= LifeBarDecreaseVar;
				}
				if (barDisplay <= 0) {
					OutOfLife = true;
				}
				if (barDisplay >= 0.0f) {
					NoMoreCollide = false;
				}
				
			}



		}

		if (barDisplay >= 1.0f) {
			NoMoreCollide=true;
		}



	}


	

}