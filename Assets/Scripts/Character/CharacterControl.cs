using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour {
    //Camera parameters
    public Camera cam;
    public int cameraYOffset = 30;
    public int cameraXOffset = 10;
    bool freeCamera = false;

    //Movement params
    public static Rigidbody2D myBody;
    private int new_direction = 1;
	public int velocity = 50;
    public float moveForce = 5, boostMultiplier = 2;
    public bool m_useAutomaticMovement = true;
    public float m_autoMoveSpeed = 5.0f;
    public int m_velDir = -1;
	//timer in game
	public Text timerText;
	protected float  totalTime = 0f;
    //Jump params
	public int jumpForce = 5;
    public Vector2 direction = new Vector2(1f, 0f);
    bool fly;
    float lastFrameVelY = 0;
    float playerAccel = 0;
    float lastPlayerAccel = 0;
	public static bool m_canDraw=false;
	public static bool isTap=true;
    //Deprecated params ?
	Sprite playerFwd;
	Sprite playerReverse;
	Sprite playerSticky;

    enum CharacterState { GROUND, JUMPING, FALLING };
	public enum InputState { TAP, GESTURE, NOTHING};
	CharacterState m_state;
	public static InputState i_state;
    public static Animator m_animator;

	// Use this for initialization
	void Start () {
		//powerMgr = (PowerupController)gun.GetComponent ("PowerupController");
		new_direction = (int) direction.x;
		playerFwd = Resources.Load<Sprite> ("Character");
		playerSticky = Resources.Load<Sprite> ("CharacterSticky"); 
		playerReverse = playerSticky;//Resources.Load<Sprite> ("Character_reversed"); 

		myBody = this.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        cam = Camera.main;

        StartCoroutine(crSetReferences());
	}

    IEnumerator crSetReferences()
    {
        yield return null;
        timerText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();

    }

	void FixedUpdate ()
	{
		if(UIManager.isPaused || Win.isWon || DamagePlayer.isLost){

			return;
		}

		Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),0) * moveForce;

        if (!m_useAutomaticMovement && Input.GetAxis("Horizontal") != 0)
        {
            ManualCharacterMovement();
        }
        else
        {
            AutomaticCharacterMovement();   
        }
        
       if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            OnJumpButtonClicked();
            
        }
	
		if (m_state==CharacterState.GROUND) {
			//Jump
			
			
			if (Input.touchCount == 2 && Input.GetTouch (0).phase== TouchPhase.Stationary && Input.GetTouch (1).phase==TouchPhase.Stationary) 
			{
				m_canDraw = false;
				OnJumpButtonClicked();				
				
			} 	else if(Input.touchCount==1 && Input.GetTouch(0).phase==TouchPhase.Moved) {
				m_canDraw=true;	
			}		
		} else {
			
			if (Input.touchCount == 1 && Input.GetTouch(0).phase==TouchPhase.Moved) {
				
				m_canDraw = true;
			}
			
		}
		/* if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
				i_state = InputState.TAP;
		} else if (Input.GetTouch (0).phase == TouchPhase.Moved && Input.touchCount==1) {

				i_state = InputState.GESTURE;
			}
				
		if (m_state == CharacterState.GROUND) {

			if (i_state == InputState.TAP) {
				m_animator.Play ("JUMP");
				m_animator.SetBool ("Airborn", true);
				myBody.AddForce (Vector2.up * jumpForce * 1000);
				m_state = CharacterState.JUMPING;
				i_state = InputState.NOTHING;
			} 
		}
	
		
	*/


        UpdateCharacterState();
	}
	
    void UpdateCameraPosition()
    {
        MoveObject camScr = cam.GetComponent("MoveObject") as MoveObject;
        //Update camera position
        if (m_velDir == -1)
        {
            camScr.updateGoalPosition(new Vector3(myBody.transform.position.x -50,camScr.getPostion().y,camScr.getPostion().z));
        }
        else
        {
            camScr.updateGoalPosition(new Vector3(myBody.transform.position.x + 50, camScr.getPostion().y, camScr.getPostion().z));
        }
        float offSet = (myBody.transform.position.x - camScr.getPostion().x);
        float absOffSet = offSet > 0 ? offSet : -offSet;
        

        //Vertical Camera follow - it's ok for now
        offSet = (myBody.transform.position.y - camScr.getPostion().y);
        absOffSet = offSet > 0 ? offSet : -offSet;
        if (absOffSet > 20)
        {

            // camScr.updatePosition(new Vector3(myBody.transform.position.x, camScr.getPostion().y, camScr.getPostion().z));
            if (offSet > 0)
            {
                camScr.updatePosition(new Vector3(camScr.getPostion().x, myBody.transform.position.y - 20, camScr.getPostion().z));
            }
            else
            {
                camScr.updatePosition(new Vector3(camScr.getPostion().x, myBody.transform.position.y + 20, camScr.getPostion().z));
            }
        }
        //float maxBound = LevelParser.tileSize/2.0f;
        //float minBound = LevelParser.MapBounds.x;
        //float clampX = Mathf.Clamp(camScr.getPostion().x,minBound,maxBound);
        //camScr.updatePosition(new Vector3(clampX, camScr.getPostion().y, camScr.getPostion().z));

 		if (camScr.getPostion().x < -300)
        {
            camScr.updatePosition(new Vector3(-300, camScr.getPostion().y, camScr.getPostion().z));
        }
        else if (camScr.getPostion().x > 20)
        {
            camScr.updatePosition(new Vector3(20, camScr.getPostion().y, camScr.getPostion().z));
        }
    }

	// Update is called once per frame
	void Update () {
		int tempScore = 0;
        UpdateCameraPosition();

		//added timer ingame
		if(!Win.isWon){
			totalTime += Time.deltaTime;
			tempScore = (int) totalTime;
			timerText.text = "Time: " + tempScore.ToString ();
		}
		if(Input.GetAxis("Vertical") != 0){
			//Create move vector based on keyboard input
			Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical"), 0);

			//Make the camera move freely by player
			freeCamera = true;

			//Make movement FPS independent and add constant velocity
			move *= Time.deltaTime * velocity;	
			cam.transform.Translate (move);
		}

       
	}
    
    void UpdateCharacterState()
    {
        float playerAccel = myBody.velocity.y - lastFrameVelY;
        float deltaAccel = playerAccel - lastPlayerAccel; //change in acceleration
        float fallingAccelTolerance = -50.0f;
        if (m_state == CharacterState.JUMPING && lastFrameVelY > 0 && myBody.velocity.y <= 0)
        {
            //velocity just changed from positive to negative so character now is falling
            m_state = CharacterState.FALLING;
        }else if (m_state == CharacterState.GROUND && myBody.velocity.y < -50.0f)
        {
            m_state = CharacterState.FALLING;
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");
			}
			else
			{
            m_animator.Play("AIRBORN");
			}
        }
        else if ( ( m_state == CharacterState.FALLING  ) && deltaAccel < fallingAccelTolerance)
        {
            //Character decelerated so it is landing
            m_state = CharacterState.GROUND;
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");


			}

			else{
            m_animator.Play("lAND");
			}
			}

        lastPlayerAccel = playerAccel;
        lastFrameVelY = myBody.velocity.y;
	}

    void ManualCharacterMovement()
    {
      /*  //Move character
       float inputX = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(-inputX, 0, 0);
        myBody.velocity = new Vector2(moveDirection.x * Time.deltaTime * velocity * 50.0f, myBody.velocity.y); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    */}

    void AutomaticCharacterMovement()
    {
        float deltaAccel = playerAccel - lastPlayerAccel;
        if(myBody.velocity.x == 0) /*&& m_state == CharacterState.GROUND*/
        {
            if(Mathf.Abs(deltaAccel) < 1)
            {
                //We have collided and not moving anymore, so change direction
                m_velDir = (m_velDir == 1) ? -1 : 1;
                Vector3 TempScale = transform.localScale;
                TempScale.x *= -1;
                transform.localScale = TempScale;
            }
        }
        
        float velX = m_velDir * Time.deltaTime * m_autoMoveSpeed * 50.0f;
        float velY = myBody.velocity.y;
        if (m_state == CharacterState.JUMPING || m_state == CharacterState.FALLING)
        {
            velY -= (Time.deltaTime * 300.0f);
        }
        myBody.velocity = new Vector2(velX, velY); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    }


/*	void OnCollisionEnter2D(Collision2D col){



			if (Angrypower.sticky == true) {
				//Destroy (GameObject.Find("Character"));
			if(col.gameObject.name=="Heap (1)")
			{
			col.transform.parent = myBody.transform ;
			myBody.isKinematic=true;
				col.gameObject.isS
				m_useAutomaticMovement = false;
			}	}
		}

*/

	void reverseDirection(float inputX){
		if (inputX < 0)
			new_direction = 1;
		else if (inputX > 0)
			new_direction = -1;

		if (((int)direction.x != new_direction)) {
			direction.x = (float)new_direction;

			SpriteRenderer rend = GetComponent<SpriteRenderer> ();

			if (new_direction < 0)
				rend.sprite = playerReverse;
			else
				rend.sprite = playerFwd;
		}
	}

    public void OnJumpButtonClicked()
    {
        if (m_state == CharacterState.GROUND)
        {
            m_state = CharacterState.JUMPING;
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");
			}
			else{
            m_animator.Play("JUMP");
			}
				m_animator.SetBool("Airborn", true);
            //if (CrossPlatformInputManager.GetButton("JumpButton"))
            //myBody.AddForce(Vector2.up * jumpForce * 1000);
            myBody.velocity = new Vector2(myBody.velocity.x, 200);

            //isGround = false;
            //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce * 200);
        }
    }
}
