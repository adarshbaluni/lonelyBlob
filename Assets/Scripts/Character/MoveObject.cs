using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {
    Vector3 goalPos;
    float transitionTime = 2.5f;
    bool direction = false;
    bool slerp = false;
	// Use this for initialization
	void Start () {

      //  Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 tempCurrent = new Vector2(transform.position.x, transform.position.y);
        Vector2 tempGoal = new Vector2(goalPos.x, goalPos.y);
        Vector2 result = Vector2.Lerp(tempCurrent,tempGoal,Time.deltaTime*2);
        transform.position = new Vector3(result.x, result.y, transform.position.z);
	}

    public void updateDirection(bool directionIN)
    {
        if (direction != directionIN)
        {
            slerp = true;
            direction = directionIN;
        }
    }

	public void updatePosition(Vector3 newPos){
        //goalPos = newPos;
		transform.position = newPos;
	}

    public Vector3 getPostion()
    {
       // return goalPos;
        return transform.position;
    }

    public Vector3 getGoalPosition()
    {
        return goalPos;
    }

    public void updateGoalPosition(Vector3 newPos)
    {
        goalPos = newPos;
        //transform.position = newPos;
    }
}
