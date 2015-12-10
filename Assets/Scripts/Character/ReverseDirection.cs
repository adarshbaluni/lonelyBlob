using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReverseDirection : MonoBehaviour {
    public static bool isBesideWall = true;
    static bool canReverse = true;

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(canReverse)
            isBesideWall = true;
        //isBesideWall = (isBesideWall) ? false : true;
        //CharacterControl.MoveDirection = (CharacterControl.MoveDirection == -1) ? 1 : -1;
        StartCoroutine(crWaitForBlobReverse());
    }

    IEnumerator crWaitForBlobReverse()
    {
        canReverse = false;
        yield return new WaitForSeconds(0.2f);
        canReverse = true;
    }
}
