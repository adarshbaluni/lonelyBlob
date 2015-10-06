using UnityEngine;
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

    public bool m_canDraw; //only activated when we are on lonely blob mode
    public float m_lifetime; //time for the blocks to dissapear
    public List<MapBlobBlock> m_blocks; //required to keep track of map blocks to destroy them
    Vector3 m_prevBlockPos; //needed for computing the next box orientation
    bool m_isFirstBlock = false; //first block of a gesture
    bool m_mousePressed = false;
    static float m_distanceToWorld = 4.5f; //Distance from camera to world plane where character is moving. Note: we should make this more generic to avoid bugs when camera zoom in and out

	// Use this for initialization
	void Start () {
        m_blockRadiusSqr = gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.GetComponent<BoxCollider2D>().size.x *0.7f;
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
        if (Input.GetMouseButtonDown(0))
        {
            m_mousePressed = true;
            m_isFirstBlock = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_mousePressed = false;
            m_isFirstBlock = false;
        }

        UpdateAndRemoveTimeoutPlatforms();
	}

    IEnumerator crDrawPlatforms()
    {
        Vector3 blockOrient;
        while (true)
        {
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
                     blockOrient = newBlockPos - m_prevBlockPos;
                }

                
                blockRot = Quaternion.FromToRotation(Vector3.right, blockOrient);

                if (!IsColliding(ref m_prevBlockPos, ref newBlockPos) || true)
                {
                    GameObject block = (GameObject)Instantiate(m_BlobBlockPrefab, newBlockPos, blockRot);

                    m_blocks.Add(new MapBlobBlock(block, m_lifetime));
                    m_prevBlockPos = newBlockPos;
                }
            }

            yield return null;
        }
    }

    void UpdateAndRemoveTimeoutPlatforms()
    {
        //Update Block Lifetimes
        foreach(var block in m_blocks)
        {
            block.m_lifetime -= Time.deltaTime;
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


    public static Vector3 MouseScreenToWorld(Vector3 mousePosition)
    {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
         return ray.origin + (ray.direction * m_distanceToWorld);  
         //Debug.Log( "World point " + coordinates[i]); 
    }
}
