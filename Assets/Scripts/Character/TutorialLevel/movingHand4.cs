using UnityEngine;
using System.Collections;

public class movingHand4 : MonoBehaviour {
	public float t=.35f;
	public float l=10f;
	public float posX=1f;
	// Use this for initialization
	public GUIStyle customGuiStyle;
	private bool printMessage=true;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (invisibleCollider1.printMessage) {
			Vector3 pos = new Vector3 (posX + Mathf.PingPong (15f * Time.time, 70), 40, 1);
			transform.position = pos;
			/*if(isIntroShown) 
		{
			timeIntroShow-=Time.deltaTime;
			if (timeIntroShow<0.0f)
			{
				isIntroShown = false;
			}
		}*/
		}
		if(!invisibleCollider1.printMessage){
			Destroy (gameObject);		
		}
		
	}
	void OnGUI()
	{
		if (Time.time % 1 < .65) {
			if (invisibleCollider1.printMessage) {
				//print("yaha!!");
				//GUIStyle myStyle = new GUIStyle();
				//myStyle.font = 15;
				//GUI.Color = new Color(1.0f, 1.0f, 1.0f, 0.5f); //0.5 is half opacity 
				customGuiStyle.fontSize = 20;
				customGuiStyle.richText = true;
				GUI.Box (new Rect (200, 200, 750, 50), "<color=white>Spikes ahead! Create a wall!</color>", customGuiStyle);
			}
		}
		
	}
	
}
