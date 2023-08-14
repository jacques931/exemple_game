using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    [SerializeField] private GameObject systemMessagePanel;
    [Header("Canvas Object")]
    public GameObject confirmPanel;
    public GameObject deadPanel;
    public GameObject buffInformationPanel;
    public Inventory inventory;
    public ItemBar itemBar;
    public MonsterPanel monsterCatch;
    public MonsterTeamManager monsterTeamManager;
    public MonsterList monsterList;
    public Tooltip tooltip;
    public PokeballManager pokeballManager;
    public MonsterTeam monsterTeam;
    public BuyShop buyShop;
    public DialogueManager dialogueManager;
    public QuestManager questManager;
    public QuestFrame questFrame;
    [SerializeField] private GameObject terrainPanel;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject craft;
    [Header("Player Canvas")]
    [SerializeField] private Image iconPlayer;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthTxt;
    [SerializeField] private Image xpBar;
    [SerializeField] private Text levelTxt;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private Text currentRoomTxt;


    private Coroutine systemMessageCoroutine;

    private void Awake()
    {
        if(instance !=null)
        {
            return;
        }
        instance = this;
    }

    public void SetRoomTxt(int currentRoom,int maxRoom)
    {
        currentRoomTxt.text = "Room " + currentRoom + " / " + maxRoom;
    }

    public void ClickSound()
    {
        SoundManager.instance.Sound(0);
    }

    //Renvoie a un panel par son nom
    public GameObject GetPanelByName(string namePanel)
    {
        switch (namePanel)
        {
            case "Terrain":
                return terrainPanel;
            case "Monster":
                return monsterTeamManager.gameObject;
            case "Heal":
                return monsterCatch.gameObject;
            case "Shop":
                return shop;
            case "Craft":
                return craft;
            default:
                Debug.LogError("Panel not found: " + namePanel);
                return null;
        }
    }

    public void ChangeIconPlayer(Sprite sprite)
    {
        iconPlayer.sprite = sprite;
    }

    public void QuitGame()
    {
        SoundManager.instance.Sound(0);
        Application.Quit();
    }

    public void WaitTimeSkill(int idSkill,float timeRemaining)
    {
        skillPanel.transform.GetChild(idSkill).GetComponent<SkillCanvas>().FilledTimeSkill(timeRemaining);
    }

    public void SetPlayerInformation(int life,int maxLife,int level)
    {
        float lifePourcent = (float)life /(float)maxLife;
        healthBar.fillAmount = lifePourcent;
        healthTxt.text = life + " / " + maxLife;

        if(healthBar.fillAmount>0.25f)
        healthBar.color = Color.green;
        else
        healthBar.color = Color.red;
        levelTxt.text = "Level " + level;
    }

    public void SetXpFilled(float xpPourcent)
    {
        xpBar.fillAmount = xpPourcent;
    }

    public void ChangeStatut(GameObject canvasObject)
    {
        canvasObject.SetActive(!canvasObject.active);
        SoundManager.instance.Sound(0);
    }

    public void SystemMessage(string texte, float time = 3)
    {
        if (systemMessageCoroutine != null)
        {
            StopCoroutine(systemMessageCoroutine);
        }

        systemMessagePanel.transform.GetChild(0).GetComponent<Text>().text = texte;
        systemMessageCoroutine = StartCoroutine(SystemMessageActive(time));
    }

    private IEnumerator SystemMessageActive(float time)
    {
        systemMessagePanel.SetActive(true);
        yield return new WaitForSeconds(time);
        systemMessagePanel.SetActive(false);
        systemMessageCoroutine = null; // Remettre la référence à la coroutine à null après son exécution.
    }

    
}
