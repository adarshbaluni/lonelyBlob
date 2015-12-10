using UnityEngine;
using System.Collections;

public class movingHand3 : MonoBehaviour {
	public float t;
	public float l;
	
	public float posX;
	public float msgX1;
	public float msgX2;
	public float msgX3;
	public float msgX4;
	public int msgSize=30;
	// Use this for initialization
	public GUIStyle customGuiStyle;
	private bool printMessage=true;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (invisibleCollider1.printMessage) {
			Vector3 pos = new Vector3 (posX + Mathf.PingPong (15f * Time.time, t), l, 1);
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
				customGuiStyle.fontSize = msgSize;
				customGuiStyle.richText = true;
				GUI.Box (new Rect (msgX1, msgX2, msgX3, msgX4), "<color=white>Create a platform to move ahead!</color>", customGuiStyle);
			}
		}
		
	}
	
}

