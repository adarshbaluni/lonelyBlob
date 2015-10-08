using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updatePosition(Vector3 newPos){
		transform.position = newPos;
	}

    public Vector3 getPostion()
    {
        return transform.position;
    }
}
