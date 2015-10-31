using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {
	
	public Transform goal; // Goal Position
	public Transform player; //Player Position
	public Transform cameraGame; 
	//public Vector3 exit;
	
	public Vector3 dir;
	public float RotationSpeed =10.0f;
	void Start(){
		//goal.position.x = 329.5f;
		//goal.position.y = 97.1;
		//goal.position.z=0;
		//goal = new Transform();
		//goal.position = new Vector3(329.5f, 97.1f, 0.0f);
		//exit = new Vector3(329.5f,97.1f,0.0f);
        StartCoroutine(crSetReferences());
	}

    IEnumerator crSetReferences()
    {
        yield return null;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraGame = Camera.main.transform;
    }
	
	void Update(){
		
		Vector3 tempPos;
		dir = goal.position - player.position ;
		//Debug.Log(player.position);
		//dir = exit - player.position ;
		dir.Normalize();
		//dir.x = dir.x;
		//transform.rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.FromToRotation(Vector3.up,dir);
		tempPos =  new Vector3(player.position.x,cameraGame.position.y + 56.0f,player.position.z);
		transform.position = tempPos;
		
		
	}
	
	
}





//+++++++++==+++++++++++++++++++++++++++++++++++++++++++++++++++++++
/*
using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {
	
	
	
	public Transform goal; // Goal Position
	public Transform player; //Player Position
	//public Vector3 exit;
    bool isInitialized = false;

	public Vector3 dir;
	public float RotationSpeed =10.0f;
	void Start(){
		//goal.position.x = 329.5f;
		//goal.position.y = 97.1;
		//goal.position.z=0;
		//goal = new Transform();
		//goal.position = new Vector3(329.5f, 97.1f, 0.0f);
		//exit = new Vector3(329.5f,97.1f,0.0f);

        StartCoroutine(crSetupReferences());
        
	}

    IEnumerator crSetupReferences()
    {
        yield return null;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isInitialized = true;
    }
	
	void Update(){

        if (!isInitialized) return;

		dir = goal.position - player.position ;
		//Debug.Log(player.position);
		//dir = exit - player.position ;
		dir.Normalize();
		dir.x = -dir.x;
		//transform.rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.FromToRotation(Vector3.up,dir);
		
		
	}
	
	
}
*/
