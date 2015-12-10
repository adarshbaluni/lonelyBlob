using UnityEngine;
using System.Collections;

public class movingHand2 : MonoBehaviour {
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (invisibleCollider2.printMessage1 && !invisibleCollider1.printMessage) {
			Vector3 pos = new Vector3 (posX + Mathf.PingPong (15f * Time.time, t), l, 1);
			transform.position = pos;
		}
			/*if(isIntroShown) 
        {
            timeIntroShow-=Time.deltaTime;
            if (timeIntroShow<0.0f)
            {
                isIntroShown = false;
            }
        }*/
	
	}
		void OnGUI()
		{
			if (Time.time % 1 < .65) {
				if (invisibleCollider2.printMessage1&&!invisibleCollider1.printMessage) {
					//GUIStyle myStyle = new GUIStyle();
					//myStyle.font = 15;
					//GUI.Color = new Color(1.0f, 1.0f, 1.0f, 0.5f); //0.5 is half opacity 
				customGuiStyle.fontSize = msgSize;
					customGuiStyle.richText = true;
				GUI.Box (new Rect (msgX1, msgX2, msgX3, msgX4), " Beware of spikes below! Create another platform", customGuiStyle);
				}
			}
		}
		
}
