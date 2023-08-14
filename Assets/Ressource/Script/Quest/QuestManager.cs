using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestDatabase questDatabase;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private MonsterDatabase monsterDatabase;
    [SerializeField] private TerrainDatabase terrainDatabase;
    [SerializeField] private Text questTitle;
    [SerializeField] private GameObject dialogueButton;

    [SerializeField] private Transform questContent;
    [SerializeField] private GameObject questPrefs;

    [SerializeField] private Transform rewardContent;
    [SerializeField] private GameObject rewardPrefs;
    [SerializeField] private GameObject rewardMonsterPrefs;
    [SerializeField] private GameObject rewardGoldPrefs;
    // 0 : dialogue / 1 : kill / 2 : capture / 3 : donjonEnter / 4 : donjonFinish / 5 : item
    [SerializeField] private Sprite[] iconQuest;

    private Quest currentQuest;

    private void Start()
    {
        RemoveQuestInfo();
        RemoveRewardInfo();
        CreateQuest();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UdpateQuestInfo(); // Flemme de le mettre partout
    }

    private void CreateQuest()
    {
        currentQuest = questDatabase.quest[PlayerPrefs.GetInt("QuestId")];
        questTitle.text = currentQuest.nameQuest;
        CreateQuestInformation(currentQuest.questType);
        CreateRewardInformation(currentQuest.rewards);
        if(currentQuest.idDialogue!=-1)
            dialogueButton.SetActive(true);
        else
            dialogueButton.SetActive(false);
    }

    private void CreateQuestInformation(QuestType[] questType)
    {
        for(int i=0;i<questType.Length;i++)
        {
            GameObject questObject = Instantiate(questPrefs,questContent);
            questObject.GetComponent<QuestPrefs>().UpdateQuest(GetIconQuest(questType[i].typeOfQuest),GetQuestInfo(questType[i],i),questType[i],i);
        }
    }

    private void CreateRewardInformation(Reward[] rewards)
    {
        if(currentQuest.money>0)
        {
            GameObject rewardGoldObject = Instantiate(rewardGoldPrefs,rewardContent);
            rewardGoldObject.transform.GetChild(1).GetComponent<Text>().text = currentQuest.money + " Gold";
        }
        
        for(int i=0;i<rewards.Length;i++)
        {
            if(!rewards[i].isMonster)
            {
                GameObject rewardObject = Instantiate(rewardPrefs,rewardContent);
                Item rewardItem = itemDatabase.item[rewards[i].id];
                string rewardText = rewards[i].amount + " " + rewardItem.name;
                rewardObject.GetComponent<QuestPrefs>().UpdateReward(rewardItem,rewardText);
            }
            else
            {
                GameObject rewardObject = Instantiate(rewardMonsterPrefs,rewardContent);
                Monster rewardMonster = monsterDatabase.monster[rewards[i].id];
                string rewardText = rewardMonster.name;
                rewardObject.GetComponent<QuestPrefs>().UpdateRewardMonster(rewardMonster,rewardText);
            }
            
        }
    }

    private void UdpateQuestInfo()
    {
        for(int i=0;i<questContent.childCount;i++)
        {
            QuestType questType = currentQuest.questType[i];
            string questTxt = GetQuestInfo(questType,i);
            questContent.GetChild(i).GetComponent<QuestPrefs>().UpdateQuest(GetIconQuest(questType.typeOfQuest),questTxt,questType,i);
        }
    }

    public List<string> GetQuestInfoTxt()
    {
        List<string> texte = new List<string>();

        for(int i=0;i<currentQuest.questType.Length;i++)
        {
            QuestType questType = currentQuest.questType[i];
            texte.Add(GetQuestInfo(questType,i));
        }

        return texte;
    }

    private string GetQuestInfo(QuestType quest,int idQuest)
    {
        switch (quest.typeOfQuest)
        {
            case TypeOfQuest.Kill:
                return "Kill " + monsterDatabase.monster[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Dialogue:
                return "Talk to " + quest.name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Capture:
                return "Capture a " + monsterDatabase.monster[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Enter_Dungeon:
                return "Enter the dungeon " + terrainDatabase.terrain[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Finish_Dungeon:
                return "Finish the dungeon " + terrainDatabase.terrain[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Buy_Item:
                return "Buy " + itemDatabase.item[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Craft:
                return "Craft " + itemDatabase.item[quest.idObject].name + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            case TypeOfQuest.Item:
                string nameItem = itemDatabase.item[quest.idObject].name;
                int itemInInventory = CanvasManager.instance.inventory.GetAllItemNumber(quest.idObject);
                PlayerPrefs.SetInt("Quest" + idQuest,itemInInventory);
                return "Give " + nameItem + " : " + PlayerPrefs.GetInt("Quest" + idQuest) + " / " + quest.amount;
            default:
                return "";
        }
    }

    private void RemoveQuestSave()
    {
        for(int i=0;i<4;i++)
        {
            PlayerPrefs.SetInt("Quest" + i,0);
        }
    }

    public void CreateDialogue()
    {
        if(currentQuest.idDialogue!=-1)
        {
            CanvasManager.instance.dialogueManager.CreateDialogue(currentQuest.idDialogue);
            gameObject.SetActive(false);
        }
            
    }

    //Cette fonction verifier si une des quete a etait fait par nom
    public void CheckQuestDialogue(string name,TypeOfQuest type)
    {
        if(type == TypeOfQuest.Dialogue && !CanEndDialogue())
        {
            return;
        }
        for (int i = 0; i < questContent.childCount; i++)
        {
            QuestType quest = currentQuest.questType[i];
            
            if (type == quest.typeOfQuest && quest.name == name && quest.amount > PlayerPrefs.GetInt("Quest" + i))
            {
                int questValue = PlayerPrefs.GetInt("Quest" + i) + 1;
                PlayerPrefs.SetInt("Quest" + i, questValue);
                FinishQuest();
                CanvasManager.instance.questFrame.ApplyText();
                break;
            }
        }
    }

    //Cette fonction verifier si une des quete a etait fait par id
    public void CheckQuestID(int itemId,int amount,TypeOfQuest type)
    {
        for (int i = 0; i < questContent.childCount; i++)
        {
            QuestType quest = currentQuest.questType[i];
            
            if (type == quest.typeOfQuest && quest.idObject == itemId && quest.amount > PlayerPrefs.GetInt("Quest" + i))
            {
                int questValue = PlayerPrefs.GetInt("Quest" + i) + amount > quest.amount ? quest.amount : (PlayerPrefs.GetInt("Quest" + i) + amount);
                PlayerPrefs.SetInt("Quest" + i, questValue);
                FinishQuest();
                CanvasManager.instance.questFrame.ApplyText();
                break;
            }
        }
    }

    private bool CheckFinishQuest()
    {
        for(int i=0;i<questContent.childCount;i++)
        {
            if(PlayerPrefs.GetInt("Quest" + i)<currentQuest.questType[i].amount)
            {
                return false;
            }
        }

        return true;
    }

    private void FinishQuest()
    {
        if(CheckFinishQuest())
        {
            string message = "You have finished the quest";
            CanvasManager.instance.SystemMessage(message);
            SoundManager.instance.Sound(3);

            RecoveryReward();
            RemoveItemtoReward();
            int idQuest = PlayerPrefs.GetInt("QuestId") + 1;
            PlayerPrefs.SetInt("QuestId",idQuest);
            RemoveQuestInfo();
            RemoveRewardInfo();
            RemoveQuestSave();
            CreateQuest();
            CreateDialogue();
        }
    }

    private void RecoveryReward()
    {
        int newMoney = PlayerPrefs.GetInt("money") + currentQuest.money;
        PlayerPrefs.SetInt("money",newMoney);
        CanvasManager.instance.inventory.SetGoldCanvas();
        //Crea un nouveau donjon
        if(PlayerPrefs.GetInt("idDungeon")<currentQuest.newIdDungeon)
        {
            PlayerPrefs.SetInt("idDungeon",currentQuest.newIdDungeon);
        }

        foreach(Reward reward in currentQuest.rewards)
        {
            if(!reward.isMonster)
            {
                CanvasManager.instance.inventory.AddItemInInventory(reward.id,reward.amount);
            }
            else
            {
                if(CanvasManager.instance.monsterCatch.CanCatch())
                {
                    Monster rewardMonster = monsterDatabase.monster[reward.id];
                    rewardMonster.input[0].canUse = true;
                    rewardMonster.startLife = rewardMonster.maxLife;
                    CanvasManager.instance.monsterCatch.CatchMonster(rewardMonster);
                    if(CanvasManager.instance.monsterCatch.GetAmountOfMonster()==1)
                    {
                        PlayerPrefs.SetInt("monsterTeam0",0);
                        CanvasManager.instance.monsterTeamManager.SetTeam();
                        CanvasManager.instance.monsterCatch.ResetIconMonster();
                    }
                }
                else
                {
                    string texte = "You have too many monsters";
                    CanvasManager.instance.SystemMessage(texte,1);
                }
                
            }
            
        }
    }

    private void RemoveItemtoReward()
    {
        foreach(QuestType quest in currentQuest.questType)
        {
            if(quest.typeOfQuest == TypeOfQuest.Item)
            {
                CanvasManager.instance.inventory.RemoveItem(quest.idObject,quest.amount);
            }
        }
    }

    private void RemoveQuestInfo()
    {
        foreach(Transform child in questContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void RemoveRewardInfo()
    {
        foreach(Transform child in rewardContent)
        {
            Destroy(child.gameObject);
        }
    }

    private Sprite GetIconQuest(TypeOfQuest type)
    {
        int id = (int)type;
        return iconQuest[id];
    }

    public bool CanEndDialogue()
    {
        int questDontFinish = 0;

        for (int i = 0; i < questContent.childCount; i++)
        {
            if (PlayerPrefs.GetInt("Quest" + i) < currentQuest.questType[i].amount && TypeOfQuest.Dialogue != currentQuest.questType[i].typeOfQuest)
            {
                questDontFinish++;
            }
        }

        return questDontFinish == 0;
    }



}
