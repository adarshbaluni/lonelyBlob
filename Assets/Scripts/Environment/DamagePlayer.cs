
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
        PauseGame(isLost);
    }
    void PauseGame(bool state)
    {
        losePanel.SetActive(state);

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (Angrypower.angry == true)
        {
            if (col.gameObject.tag == "Player")
            {
                if (this.gameObject.tag == "Bat")
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }



    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
            isLost = true;
			CharacterControl.totalTime = 0f;
            //Destroy (col.gameObject);
            Angrypower.angry = false;
            Character.OnDeath();
            Destroy (col.gameObject);
        }
    }
}