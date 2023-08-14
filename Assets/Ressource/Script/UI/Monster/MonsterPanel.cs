using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MonsterPanel : MonoBehaviour
{
    [SerializeField] private Transform monsterSlotPanel;
    [SerializeField] private Text amountMonsterTxt;
    [SerializeField] private MonsterDatabase monsterDatabase;
    private MonsterCatchManager monsterCatchManager;
    private List<Monster> monsters = new List<Monster>();

    private int idMonsterCatch=-1;
    private int[] currentIdMonsterTeam = new int[3];
    private MonsterSlot monsterSlotScript;

    private void Awake()
    {
        RecoveryIdMonster();
        SetDataMonster();
        ApplyMonsterSlot();
        amountMonsterTxt.text = GetAmountOfMonster() + "/" + monsterSlotPanel.childCount;
    }

    private void Start()
    {
        CanvasManager.instance.monsterTeamManager.SetTeam();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        IconMonsterNormal();
        idMonsterCatch=-1;
        monsterSlotScript = null;
    }

    private void OnEnable()
    {
        SetCurrentData();
    }

    public void SaveMonster(Monster monster)
    {
        monsters[monster.id] = monster;
        monsterCatchManager.SaveMonsterList(monsters.ToArray());
    }

    public void SetCurrentData()
    {
        SetDataMonster();
        ApplyMonsterSlot();
        CanvasManager.instance.monsterTeamManager.SetTeam();
    }

    public void ResetIconMonster()
    {
        RecoveryIdMonster();
        IconMonsterNormal();
    }

    private bool MonsterIsInTeam(int compareId)
    {
        foreach (int id in currentIdMonsterTeam)
        {
            if(id!=-1 && compareId==id)
            {
                return true;
            }
        }

        return false;
    }

    private void RecoveryIdMonster()
    {
        for(int i=0;i<currentIdMonsterTeam.Length;i++)
        {
            currentIdMonsterTeam[i] = PlayerPrefs.GetInt("monsterTeam" + i)!=-1 ? PlayerPrefs.GetInt("monsterTeam" + i) : -1;
        }
    }

    public List<Monster> GetListMonster()
    {
        return monsters;
    }

    public bool CheckMonsterIsCaptured(int idMonster)
    {
        foreach(Monster monster in monsters)
        {
            if(monster.idMonster==idMonster)
            {
                return true;
            }
        }
        
        return false;
    }

    public void HealAllMonster()
    {
        foreach(Monster monster in monsters)
        {
            if(monster.idMonster!=0)
            {
                monster.currentLife = monster.maxLife;
            }
        }

        monsterCatchManager.SaveMonsterList(monsters.ToArray());
    }

    public void SetDataMonster()
    {
        monsterCatchManager = MonsterCatchManager.Instance;
        monsters = monsterCatchManager.LoadMonsterList().ToList();
        foreach(Monster monster in monsters)
        {
            monster.SetState(monsterDatabase.monster[monster.idMonster]);
        }
    }

    //Recupere les informations necessaire et met en couleur
    public void ClickIconMonster(GameObject monsterSlot)
    {
        monsterSlotScript = monsterSlot.GetComponent<MonsterSlot>();
        if(monsterSlotScript.getId()!=-1)
        {
            SoundManager.instance.Sound(0);
            if(MonsterIsInTeam(monsterSlotScript.getId()))
            {
                return;
            }
            idMonsterCatch = monsterSlotScript.getId();
            IconMonsterNormal();
            Color selectedColor = new Color(122f/255f, 181f/255f, 250f/255f, 1f);
            monsterSlot.GetComponent<Image>().color = selectedColor;
            monsterSlot.transform.GetChild(0).GetComponent<Image>().color = selectedColor;
        }
        
    }

    //Remet tous les icon des monstres a leur coleur initial et met en rouge pour le monstre actuel
    private void IconMonsterNormal()
    {
        foreach (Transform icon in monsterSlotPanel)
        {
            icon.GetComponent<Image>().color = Color.white;
            icon.transform.GetChild(0).GetComponent<Image>().color = Color.white;

            if(MonsterIsInTeam(icon.GetComponent<MonsterSlot>().getId()))
            {
                icon.GetComponent<Image>().color = Color.red;
            }
        }

    }

    //Actualise les slot a partir de la liste de monstre
    private void ApplyMonsterSlot()
    {
        for(int i=0;i<monsterSlotPanel.childCount;i++)
        {
            if(monsters.Count<=i)
            {
                monsterCatchManager.SaveMonsterList(monsters.ToArray());
                break;
            }
            monsters[i].id = i; // Permet d'avoir une suite de nombre d'affilé meme apres suppresion
            MonsterSlot slot = monsterSlotPanel.GetChild(i).GetComponent<MonsterSlot>();
            slot.InstanceIcon(monsters[i]);
        }
        monsterCatchManager.SaveMonsterList(monsters.ToArray());

    }

    //Capture un nouveau monstre
    public void CatchMonster(Monster monster)
    {
        int idMonster = GetLessMonsterSlot();

        if (idMonster == -1)
        {
            monster.id = monsters.Count;
            monsters.Add(monster);
            idMonster = monsters.Count - 1;
        }
        else
        {
            monster.id = idMonster;
            monsters[idMonster] = monster;
        }

        MonsterSlot monsterSlot = monsterSlotPanel.GetChild(idMonster).GetComponent<MonsterSlot>();
        monsterSlot.InstanceIcon(monster);
        amountMonsterTxt.text = GetAmountOfMonster() + "/" + monsterSlotPanel.childCount;
        monsterCatchManager.SaveMonsterList(monsters.ToArray());
        CanvasManager.instance.questManager.CheckQuestID(monster.id,1,TypeOfQuest.Capture);

    }

    //Renvoie a la position d'un monstre deja utiliser vide
    public int GetLessMonsterSlot()
    {
        for (int i = 0; i < monsterSlotPanel.childCount; i++)
        {
            if(monsters.Count<=i)
                return -1;
            if (monsters[i].playerPrefs == null)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetAmountOfMonster()
    {
        int number = 0;
        foreach(Monster monster in monsters)
        {
            if(monster.idMonster != 0)
                number++;
        }
        return number;
    }
    
    //Initialise la possibilité de supprimer un monster
    public void ClickDeleteMonster()
    {
        SoundManager.instance.Sound(0);
        if(PlayerPrefs.GetInt("idPlayer")==idMonsterCatch)
        {
            string texte = "You cannot delete your current monster";
            CanvasManager.instance.SystemMessage(texte);
            return;
        }
        if(idMonsterCatch!=-1)
        {
            CanvasManager.instance.confirmPanel.SetActive(true);
            string texte = "Are you sure you want to delete this monster ?";
            CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,DeleteMonster);
        }
        else
        {
            string message = "Please select a monster";
            CanvasManager.instance.SystemMessage(message);
        }
        
    }

    private void DeleteMonster()
    {
        if(idMonsterCatch!=-1)
        {
            SoundManager.instance.Sound(22);
            monsters[idMonsterCatch] = new Monster{id = idMonsterCatch};
            monsterSlotScript.InstanceIcon(monsters[idMonsterCatch]);
            IconMonsterNormal();
            idMonsterCatch=-1;
            monsterCatchManager.SaveMonsterList(monsters.ToArray());

            string message = "You deleted a monster";
            CanvasManager.instance.SystemMessage(message,0.8f);
            amountMonsterTxt.text = GetAmountOfMonster() + "/" + monsterSlotPanel.childCount;
            CanvasManager.instance.monsterList.ResetMonsterList();
        }
        
    }

    public void UpdateMonsterAttribut(Monster monster)
    {
        monsters[monster.id] = monster;
        monsterCatchManager.SaveMonsterList(monsters.ToArray());
    }

    //Permet de savoir si il a de la place pour capturer un nouveau monstre
    public bool CanCatch()
    {
        if(GetAmountOfMonster()>=monsterSlotPanel.childCount)
            return false;
        else
            return true;
    }

}
