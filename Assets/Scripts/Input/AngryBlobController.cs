using UnityEngine;
using System.Collections;

public class AngryBlobController : MonoBehaviour {

    public bool m_isActive = false; //indicate if blob is in angry mode and can use this mechanic
    public GameObject m_FireballPrefab;
    public float m_ForceMultiplier;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0) && m_isActive)
        {
            //shoot a fireball
            Vector3 mouseWorldPos = InputGestureBlobDraw.MouseScreenToWorld(Input.mousePosition);
            Vector3 shootDirection = (mouseWorldPos - gameObject.transform.position).normalized;
            GameObject fireball = (GameObject)Instantiate(m_FireballPrefab, gameObject.transform.position + shootDirection * 30.0f,Quaternion.identity);
            fireball.GetComponent<Rigidbody2D>().AddForce(shootDirection * m_ForceMultiplier);
            
        }
	}
}
