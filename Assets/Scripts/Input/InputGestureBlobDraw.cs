using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapBlobBlock
{
    public GameObject m_block;
    public float m_lifetime = 5;

    public MapBlobBlock(GameObject block, float lifetime)
    {
        m_block = block;
        m_lifetime = lifetime;
    }
}

public class InputGestureBlobDraw : MonoBehaviour 
{
	public GameObject m_BlobBlockPrefab;
    float m_blockRadiusSqr;
	public static bool platformCreate=false;
	public bool Draw=true; // for testing on PC set it to true , else set it to false for continuing on mobile
    bool m_canDraw; //only activated when we are on lonely blob mode
    public float m_lifetime; //time for the blocks to dissapear
    public List<MapBlobBlock> m_blocks; //required to keep track of map blocks to destroy them
    Vector3 m_prevBlockPos; //needed for computing the next box orientation
    bool m_isFirstBlock = false; //first block of a gesture
    bool m_mousePressed = false;
    static float m_distanceToWorld = 4.5f; //Distance from camera to world plane where character is moving. Note: we should make this more generic to avoid bugs when camera zoom in and out
	
	// Use this for initialization
	void Start () {
        //m_blockRadiusSqr = gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.GetComponent<BoxCollider2D>().size.x *0.7f;
        m_blockRadiusSqr = GetComponent<CircleCollider2D>().radius * GetComponent<CircleCollider2D>().radius;
        m_blocks = new List<MapBlobBlock>();
        StartCoroutine(crDrawPlatforms());
	}

    bool IsColliding(ref Vector3 existingBlock, ref Vector3 newBlock)
    {
        if ((existingBlock - newBlock).sqrMagnitude < m_blockRadiusSqr)
            return true;
        return false;
    }

	// Update is called once per frame
	void Update () {

		if(UIManager.isPaused || Win.isWon || DamagePlayer.isLost){
			
			return;
    	}
        if (Input.GetMouseButtonDown(0))// && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            m_mousePressed = true;
            m_isFirstBlock = true;
            platformCreate = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_mousePressed = false;
            m_isFirstBlock = false;
            platformCreate = false;
        }
        print("Mouse pressed: " + m_mousePressed);
     
		
        UpdateAndRemoveTimeoutPlatforms();
	}

    IEnumerator crDrawPlatforms()
    {
		if(Draw==false)
		{
		m_canDraw=CharacterControl.m_canDraw; // mobile testing
		}
		else if(Draw==true)
		{
		m_canDraw=true; // PC testing
		}
		
		
        Vector3 blockOrient;
        while (true)
        {
            if (UIManager.isPaused || Win.isWon || DamagePlayer.isLost)
            {
                yield return null;
                continue;
            }

            if (m_canDraw && m_mousePressed)
            {
                Vector3 newBlockPos = MouseScreenToWorld(Input.mousePosition);

                Quaternion blockRot;
                if (m_isFirstBlock)
                {
                    //first block of gesture has default orientation
                    m_prevBlockPos = newBlockPos;
                    m_isFirstBlock = false;
                    blockOrient = Vector3.right;
                }
                else
                {
                    DrawBlocksBetweenPos(ref m_prevBlockPos, ref newBlockPos);
                    blockOrient = newBlockPos - m_prevBlockPos;
                }

                
                blockRot = Quaternion.FromToRotation(Vector3.right, blockOrient);

                if (!IsColliding(ref m_prevBlockPos, ref newBlockPos) || true)
                {
                    CreateBlock(ref newBlockPos, blockRot);
                    m_prevBlockPos = newBlockPos;
                }
            }

            yield return null;
        }
    }

    void CreateBlock(ref Vector3 blockPos,Quaternion blockRot)
    {
		if(LifeBar.OutOfLife)
			return;
        GameObject block = (GameObject)Instantiate(m_BlobBlockPrefab, blockPos, blockRot);
        m_blocks.Add(new MapBlobBlock(block, m_lifetime));
    }

    void DrawBlocksBetweenPos(ref Vector3 prevPos,ref Vector3 newPos)
    {
        float overlapFactor = 0.5f; //radius factor that circles will overlap on the interpolation
        float blockWidth = 2.0f*GetComponent<CircleCollider2D>().radius * overlapFactor;
        Vector3 distVec = newPos - prevPos;
        float availableDist = distVec.magnitude - blockWidth;
        int blockQty = (int)(availableDist / blockWidth); //how many blocks can we fit in available space
        float blockWidthFactor = blockWidth / distVec.magnitude;
        Vector3 newBlockPos;

        for(int i=1;i < blockQty+2;++i)
        {
            newBlockPos = prevPos + distVec * i * blockWidthFactor;
            CreateBlock(ref newBlockPos,Quaternion.FromToRotation(Vector3.right, distVec));

        }
    }

	//-----------------------------------------------------------------------



	void UpdateAndRemoveTimeoutPlatforms()
	{
		
		//Update Block Lifetimes
		foreach(var block in m_blocks)
		{
			block.m_lifetime -= Time.deltaTime;
			
			Color newColor = block.m_block.GetComponent<Renderer>().material.color;
			newColor.a -= Time.deltaTime/3.5f;
			block.m_block.GetComponent<Renderer>().material.color = newColor;
			
		}
		
		//Store indices of expired blocks to remove them
		List<int> removeIndices = new List<int>();
		int index = 0;
		foreach (var block in m_blocks)
		{
			
			
			if (block.m_lifetime < 0)
			{
				//Block has expired so mark for destruction
				removeIndices.Add(index);
			}
			++index;
		}
		
		foreach(var iB in removeIndices)
		{
			Destroy(m_blocks[iB].m_block);
			m_blocks.RemoveAt(iB);
		}
	}
//--------------------------------------------------------------------------











	/*

    void UpdateAndRemoveTimeoutPlatforms()
    {

        //Update Block Lifetimes
        foreach(var block in m_blocks)
        {
            block.m_lifetime -= Time.deltaTime;

			Color newColor = block.m_block.GetComponent<Renderer>().material.color;
			newColor.a -= Time.deltaTime/4.50f;
			block.m_block.GetComponent<Renderer>().material.color = newColor;

        }

        //Store indices of expired blocks to remove them
        List<int> removeIndices = new List<int>();
        int index = 0;
        foreach (var block in m_blocks)
        {


            if (block.m_lifetime < 0)
            {
                //Block has expired so mark for destruction
                removeIndices.Add(index);
            }
            ++index;
        }

        foreach(var iB in removeIndices)
        {
			Destroy(m_blocks[iB].m_block);
            m_blocks.RemoveAt(iB);
        }
    }


	*/

    public static Vector3 MouseScreenToWorld(Vector3 mousePosition)
    {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
         return ray.origin + (ray.direction * m_distanceToWorld);  
         //Debug.Log( "World point " + coordinates[i]); 
    }
}
