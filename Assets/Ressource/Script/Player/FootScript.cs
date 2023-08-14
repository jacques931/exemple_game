using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootScript : MonoBehaviour
{
    // Il saut quand il touche un surface 
    private bool isGround;

    public bool getIsGround()
    {
        return isGround;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if(transform.parent.tag=="Player")
        {
            isGround = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (transform.parent.tag=="Player" || col.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }
}
