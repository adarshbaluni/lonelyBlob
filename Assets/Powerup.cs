using UnityEngine;
using System.Collections;

public class AddPowerup : MonoBehaviour {
	public int typePower = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			CharacterControl player = collider.gameObject.GetComponent<CharacterControl>();
			
			//Call Function
			
			Destroy(gameObject);
		}
	}
}
