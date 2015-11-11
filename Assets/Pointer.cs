using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {
	
	public Transform goal; // Goal Position
	public Transform player; //Player Position
	public Transform cameraGame;
    //---------------------------------- Nov 2 2015

	public Transform host;

    public Transform ArrowOverGoal;
    public Renderer rend;
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
       
        rend = GetComponent<Renderer>();
        rend.enabled = true;
	}

    IEnumerator crSetReferences()
    {
        yield return null;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraGame = Camera.main.transform;
		ArrowOverGoal = GameObject.FindGameObjectWithTag("ArrowOverGoal").transform;
		host = GameObject.FindGameObjectWithTag("ArrowSprite").transform;


    }
	
	void Update(){
		
		Vector3 tempPos;
		dir = goal.position - player.position ;
		//Debug.Log(player.position);
		//dir = exit - player.position ;
		dir.Normalize();
		//dir.x = dir.x;
		//transform.rotation = Quaternion.LookRotation(dir);
		host.rotation = Quaternion.FromToRotation(Vector3.up,dir);
		tempPos =  new Vector3(player.position.x,cameraGame.position.y + 56.0f,player.position.z);
		host.position = tempPos;

		//      if (ArrowOverGoal.position.y <= transform.position.y)
		if(host.position.y >= ArrowOverGoal.position.y)
        {
            // transform.GetComponent.enabled = false;
			host.gameObject.SetActive(false);

        }
//		if (transform.position.y >= ArrowOverGoal.position.y) {
//			
//		}
		else 
		{ 
			host.gameObject.SetActive(true); 
		}
		
		
	}
	
	
}




