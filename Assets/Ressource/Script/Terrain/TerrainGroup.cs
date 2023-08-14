using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainGroup : MonoBehaviour
{
    private bool canChangeRoom;
    private int positionTerrain;

    public void SetCanChangeRoom(bool changeRoom,int _positionTerrain)
    {
        canChangeRoom = changeRoom;
        positionTerrain = _positionTerrain;
    }

    private void Start()
    {
        CanvasManager.instance.SetRoomTxt(positionTerrain+1,transform.childCount);
    }

    private void Update()
    {
        if(canChangeRoom)
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                ChangeRoom();
            }
        }
    }

    private void ChangeRoom()
    {
        SoundManager.instance.Sound(9);
        //Remove and apply Terrain
        transform.GetChild(positionTerrain-1).gameObject.SetActive(false);
        transform.GetChild(positionTerrain).gameObject.SetActive(true);
        CanvasManager.instance.SetRoomTxt(positionTerrain+1,transform.childCount);

        //Changement de position du player
        Vector3 newPosition = transform.GetChild(positionTerrain).GetComponent<TerrainManager>().GetNewPosition();
        GameObject.FindGameObjectWithTag("Player").transform.position = newPosition;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ChangeCameraY(transform.GetChild(positionTerrain).position.y);

        transform.GetComponentInParent<TerrainObjectManager>().waitPanel.SetActive(true);
        
    }
}
