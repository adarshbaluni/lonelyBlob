using UnityEngine;
using System.Collections;

public class ShotScript : MonoBehaviour {

	public int damage = 1;

	public bool isEnemyHit = false;

	// Use this for initialization
	void Start () {
	
		Destroy (gameObject, 5);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
