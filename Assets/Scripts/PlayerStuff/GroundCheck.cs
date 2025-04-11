using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool onGround = false;
    private int touchingGround = 0;

    void OnTriggerEnter2D(Collider2D collider)
    {
        onGround = true;
        touchingGround++;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        touchingGround--;
        if(touchingGround == 0)
        {
            onGround = false;
        }
    }

    public bool AmIOnGround()
    {
        return(onGround);
    }
}
