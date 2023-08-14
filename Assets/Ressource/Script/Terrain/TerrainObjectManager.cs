using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObjectManager : MonoBehaviour
{
    [SerializeField] private TerrainDatabase terrainDatabase;
    public GameObject waitPanel;

    private bool canReturnCity;
    private bool isEndGate;
    private bool newIdDungeon;
    private int currentTerrainId;

    private void Awake()
    {
        CreateTerrain(0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && canReturnCity)
        {
            ReturnInCity();
        }
    }

    public void SetCanReturnCity(bool canIt,bool _isEndGate,bool _nextDungeon)
    {
        canReturnCity = canIt;
        isEndGate = _isEndGate;
        newIdDungeon = _nextDungeon;
    }

    public void ReturnInCity()
    {
        if(isEndGate)
        {
            CanvasManager.instance.questManager.CheckQuestID(currentTerrainId,1,TypeOfQuest.Finish_Dungeon);
        }
        if(newIdDungeon)
        {
            if(PlayerPrefs.GetInt("idDungeon")<currentTerrainId)
            {
                PlayerPrefs.SetInt("idDungeon",currentTerrainId);
            }
            
        }
        SoundManager.instance.Sound(9);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        CreateTerrain(0);
        waitPanel.SetActive(true);
    }

    public void CreateTerrain(int idTerrainSelect)
    {
        DestroyTerrain();
        currentTerrainId = idTerrainSelect;
        GameObject newTerrain = terrainDatabase.terrain[idTerrainSelect].terrainPrefs;
        GameObject currentTerrain = Instantiate(newTerrain,newTerrain.transform.position,new Quaternion(0,0,0,0),transform); // modifier la postion plus tard 
        if(idTerrainSelect==0)
        {
            Vector3 newPosition = currentTerrain.GetComponent<TerrainManager>().GetNewPosition();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CreatePlayerInCity(newPosition);
        }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ChangeCameraY(currentTerrain.transform.GetChild(0).position.y);
    }

    private void DestroyTerrain()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
