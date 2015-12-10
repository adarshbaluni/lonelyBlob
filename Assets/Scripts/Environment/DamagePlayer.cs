
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class DamagePlayer : MonoBehaviour
{
    public Texture2D loseTex;
    public GameObject losePanel;
    public static bool isLost;
    public CharacterControl Character;





    // Use this for initialization
    void Start()
    {
        isLost = false;
        losePanel = UIManager.s_losePanel;
    }

    // Update is called once per frame
    void Update()
    {

		if (Angrypower.angry==false && this.gameObject.tag=="MagmaWall") {
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger=false;		
		}
        PauseGame(isLost);
    }
    void PauseGame(bool state)
    {
        losePanel.SetActive(state);

    }
	
	
    public void OnCollisionEnter2D(Collision2D col)
    {	

		if (col.gameObject.tag == "Player") {

			if (Angrypower.angry == true) {

				if (this.gameObject.tag == "Bat" || this.gameObject.tag == "MagmaWall" || this.gameObject.tag == "Snake") {
					Destroy (this.gameObject);
				}

			}else if (this.gameObject.tag != "MagmaWall") {
				
					isLost = true;
					CharacterControl.totalTime = 0f;
					//Destroy (col.gameObject);
					Angrypower.angry = false;
					Character.OnDeath ();
					Destroy (col.gameObject);
				
			}
		
		
		}
	}
}