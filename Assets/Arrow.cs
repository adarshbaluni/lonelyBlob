using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {


	public Texture2D compass; // compass image
	public Texture2D needle; // needle image
	public Rect r; // rect where to draw compass
	public float angle; // angle to rotate the needle
	public Transform task; // Task Position
	public Transform player; //Player Position
	
	
	void Start(){
		r =  new Rect(10, 10, 200, 200);
		needle = new Texture2D(10,20);
		//tex.LoadImage(imageAsset.bytes);
		//GetComponent<Renderer>().material.mainTexture = tex;
	}
	
	void OnGUI(){
		
		GUI.DrawTexture(r, compass); // draw the compass...
		Vector2 p = new Vector2(r.x+r.width/2,r.y+r.height/2); // find the center
		Matrix4x4 svMat = GUI.matrix; // save gui matrix
		GUIUtility.RotateAroundPivot(angle,p); // prepare matrix to rotate
		GUI.DrawTexture(r,needle); // draw the needle rotated by angle
		GUI.matrix = svMat; // restore gui matrix
	}
	
	void Update(){
		
		Vector3 dir = task.position - player.position;
		dir.y = 0; // remove the vertical component, if any
		dir.Normalize();
		
		Vector3 forw= player.forward;
		forw.y = 0;
		forw.Normalize();
		
		angle = AngleOffAroundAxis(dir, forw, Vector3.up);
		//my algorithm returned as radians, convert to degrees
		angle *= 180 / Mathf.PI;
	}
	
	float AngleOffAroundAxis( Vector3 v, Vector3 forward, Vector3 axis)
	{
		Vector3 right = Vector3.Cross(axis, forward);
		forward = Vector3.Cross(right, axis);
		Vector2 v2= new Vector2(Vector3.Dot(v, forward), Vector3.Dot(v, right));
		v2.Normalize();
		return Mathf.Atan2(v2.y, v2.x);
	}

}
