using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {
	public Vector2 pos ;
	public Vector2 size ;
	public static bool lifeBarInitial=false;
	public static bool NoMoreCollide = false;
	public static bool OutOfLife=false;
	public float LifeBarIncreaseVar=.10f;
	public float LifeBarDecreaseVar=.009f;
	public float InitialLifeVal=.25f;

	// Use this for initialization
	void Start () {
		pos = new Vector2(50,50);
		size = new Vector2(100,20);
		lifeBarInitial = true;
		
	}
	
	// Update is called once per frame
	public float barDisplay=.07f; //current progress
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	void OnGUI() {
		//draw the background:
		//Color oldColor = GUI.color;
     	GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		//GUI.color = Color.white;
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		//GUI.color = Color.blue;
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
		//GUI.color = oldColor;
		
	}
	
	void Update() {
		if (lifeBarInitial == true) {
			barDisplay = InitialLifeVal;
			lifeBarInitial=false;
			OutOfLife=false;
			NoMoreCollide=false;
			if (barDisplay == 1.0f) {
				NoMoreCollide=true;
			}

		}
		if (BlobRegen.life == true) {
			//print ("comiiiiiinng in blobregen");
			barDisplay += LifeBarIncreaseVar;
            barDisplay = Mathf.Clamp(barDisplay, 0, 1);
			BlobRegen.life =false;
			OutOfLife=false;
			NoMoreCollide=false;
			if (barDisplay == 1.0f) {
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
		if (barDisplay == 1.0f) {
			NoMoreCollide=true;
		}



	}
}
