using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainPanel : MonoBehaviour
{
    [SerializeField] private TerrainDatabase terrainDatabase;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform TerrainContent;
    [SerializeField] private Transform MonsterContent;
    [SerializeField] private Transform ItemContent;
    [SerializeField] private GameObject ItemPanelPrefs;
    [SerializeField] private GameObject MonsterPanelPrefs;
    [SerializeField] private GameObject RoomTxtPrefs;
    [SerializeField] private Text terrainName;
    [SerializeField] private Image terrainImage;
    [SerializeField] private TerrainObjectManager TerrainObjectContent;
    [SerializeField] private GameObject waitPanel;
    private int idTerrainSelect;
    private Color normalColor;

    private void Start()
    {
        SetTerrainSlot();
        SelectTerrain(1);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnDisable()
    {
        SelectTerrain(1);
    }

    public void SetTerrainSlot()
    {
        for(int i=1;i<terrainDatabase.terrain.Length;i++)
        {
            int terrainIndex = i;
            normalColor = TerrainContent.GetChild(i-1).GetComponent<Image>().color;
            TerrainContent.GetChild(i-1).GetChild(0).GetComponent<Text>().text = terrainDatabase.terrain[terrainIndex].name;
            TerrainContent.GetChild(i-1).GetComponent<Button>().onClick.AddListener(() => SelectTerrain(terrainIndex));
        }
    }

    private void SelectTerrain(int idTerrain)
    {
        SoundManager.instance.Sound(0);
        if(PlayerPrefs.GetInt("idDungeon")>=idTerrain-1)
        {
            idTerrainSelect = idTerrain;
            SelectButtonNormal();
            TerrainContent.GetChild(idTerrainSelect-1).GetComponent<Image>().color = new Color(122f/255f, 181f/255f, 250f/255f, 1f);
            SetInformationTerrain();
        }
        
    }

    private void SelectButtonNormal()
    {
        for(int i=0;i<TerrainContent.childCount;i++)
        {
            if(PlayerPrefs.GetInt("idDungeon")>=i)
            {
                TerrainContent.GetChild(i).GetComponent<Image>().color = normalColor;
            }
            else
            {
                TerrainContent.GetChild(i).GetComponent<Image>().color = new Color(255f/255f, 132f/255f, 132f/255f, 1f);
            }
            
        }
    }

    private void ResetChild(Transform panelRemove)
    {
        foreach(Transform child in panelRemove)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetInformationTerrain()
    {
        Terrain currentTerrain = terrainDatabase.terrain[idTerrainSelect];
        ResetChild(MonsterContent);
        ResetChild(ItemContent);

        terrainName.text = currentTerrain.name;
        terrainImage.sprite = currentTerrain.sprite;

        for (int i = 0; i < currentTerrain.terrainPrefs.transform.childCount; i++)
        {
            TerrainManager terrainManager = currentTerrain.terrainPrefs.transform.GetChild(i).GetComponent<TerrainManager>();
            (Monster[] listMonster, Item[] listItem) = terrainManager.GetMonstersAndItemsInTerrain();

            GameObject roomMonsterContent = Instantiate(RoomTxtPrefs, MonsterContent);
            roomMonsterContent.transform.GetChild(0).GetComponent<Text>().text = "Room " + (i+1);

            GameObject roomItemContent = Instantiate(RoomTxtPrefs, ItemContent);
            roomItemContent.transform.GetChild(0).GetComponent<Text>().text = "Room " + (i+1);

            foreach (Monster monster in listMonster)
            {
                GameObject monsterObject = Instantiate(MonsterPanelPrefs, MonsterContent);
                monsterObject.GetComponent<MonsterTerrain>().SetInformation(monster.idMonster);
            }
            foreach (Item item in listItem)
            {
                GameObject itemObject = Instantiate(ItemPanelPrefs, ItemContent);
                itemObject.GetComponent<ItemTerrain>().SetInformation(item.id);
            }
        }

        MonsterContent.parent.GetComponent<ContentToZero>().ReturnZero();
        ItemContent.parent.GetComponent<ContentToZero>().ReturnZero();
    }


    public void ClickGenerateTerrain()
    {
        SoundManager.instance.Sound(0);
        if(CanvasManager.instance.monsterTeamManager.HaveMonsterInTeamAndAlive() != -1)
        {
            SoundManager.instance.Sound(9);
            TerrainObjectContent.CreateTerrain(idTerrainSelect);
            Vector3 newPositionPlayer = terrainDatabase.terrain[idTerrainSelect].terrainPrefs.transform.GetChild(0).GetComponent<TerrainManager>().GetNewPosition();
            newPositionPlayer.z = -1f;
            if(idTerrainSelect!=0)
            {
                gameManager.CreateMonsterPlayer(newPositionPlayer);
            }
                
            else
            {
                gameManager.DeleteAncienPlayer();
                gameManager.CreatePlayerInCity(newPositionPlayer);
            }

            CanvasManager.instance.questManager.CheckQuestID(idTerrainSelect,1,TypeOfQuest.Enter_Dungeon);
                
            waitPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            string message = "You must have a monster in your team with life points";
            CanvasManager.instance.SystemMessage(message);
        }
        
    }


    
}
