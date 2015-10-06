using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	public float force;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody rb = hit.gameObject.GetComponent<Rigidbody> ();
		
		if (rb != null) {
			rb.AddForce(force*transform.forward);
		}
	}
}
