using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;

    private void Update()
    {
        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = new Vector3(player.position.x,transform.position.y,transform.position.z);
    }

    public void ChangeCameraY(float posY)
    {
        transform.position = new Vector3(transform.position.x,posY,transform.position.z);
    }
}
