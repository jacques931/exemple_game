using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterScript : MonoBehaviour
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private Text nameMonsterTxt;
    [SerializeField] private GameObject iconStar;
    [SerializeField] protected Monster monster;
    [Header("Item")]
    [SerializeField] private int nbLoots;
    [SerializeField] private GameObject[] items;
    [Header("Boss")]
    [SerializeField] private GameObject finalGate;
    [SerializeField] private bool isBoss;

    private void Start()
    {
        SetMonster();
        UpdateLifeBar();
    }

    public void SetDataMonster(Monster newMonster)
    {
        monster = newMonster;
    }

    private void SetMonster()
    {
        int level = monster.level;
        monster.startLife = monster.maxLife;
        if(!isBoss)
            monster.level = Random.Range(level,monster.level+3);

        int addLevel = monster.level - level;
        // Permet d'adpater le niveau du monstre // Changer plus tard
        for(int i=0;i<addLevel;i++)
        {
            monster.maxLife += (int)(monster.startLife*0.1f);
            monster.defense *= 1.03f;
            if(monster.defense==0 && monster.level==8) // Permet au faible monstre d'avoir un defense
            {
                monster.defense = 1;
            }
            monster.maxXp *= 1.10f;
            monster.xp *= 1.05f;
            foreach(AttackInput attack in monster.input)
            {
                attack.damage *= 1.05f;
            }
        }

        if(monster.input.Length > 1 && monster.level>=monster.SecondSkillLevel)
            monster.input[1].canUse = true;

        monster.input[0].canUse = true;
        monster.currentLife = monster.maxLife;

        //Canvas
        nameMonsterTxt.text = "Lv."+monster.level +" "+ monster.name;
        if(monster.playerPrefs!=null)
        {
            iconStar.SetActive(true);
        }

    }

    public int[] GetItem()
    {
        List<int> listItem = new List<int>();
        foreach(GameObject item in items)
        {
            int idItem = item.GetComponent<ItemDrop>().GetIdItem();
            listItem.Add(idItem);
        }

        return listItem.ToArray();
    }

    public Monster GetMonster()
    {
        return monster;
    }

    private void UpdateLifeBar()
    {
        float fillAmount = (float)monster.currentLife / monster.maxLife;
        lifeBar.fillAmount = fillAmount;

        if(lifeBar.fillAmount>0.25f)
        lifeBar.color = Color.green;
        else
        lifeBar.color = Color.red;
    }

    public void ApplyDamage(int damage)
    {
        SoundManager.instance.Sound(12);
        int finalDamage = (int)Mathf.Max(damage - monster.defense, 0);
        monster.currentLife -= finalDamage > 0 ? finalDamage : 1;
        UpdateLifeBar();
        if(monster.currentLife<1)
        {
            Dead();
        }
        else
        {
            StartCoroutine(GetComponent<MonsterIA>().SetMonsterInPlayerHunt());
        }
    }

    private void Dead()
    {
        SoundManager.instance.Sound(11);
        GetComponent<Animator>().SetTrigger("Dead");
        SetItemInScene();
        SetMoneyToPlayer();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().SetXp(monster.getXpMonster);
        GetComponent<MonsterIA>().DeadMonster();
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.4f);
        GetComponent<Collider2D>().isTrigger = true;
        CanvasManager.instance.questManager.CheckQuestID(monster.idMonster,1,TypeOfQuest.Kill);
        //Boss
        if(isBoss)
        {
            finalGate.SetActive(true);
            SoundManager.instance.Sound(15);
        }
        Destroy(gameObject,1f);
    }

    private void SetMoneyToPlayer()
    {
        int ancienMoney = PlayerPrefs.GetInt("money");
        float moneyRangeMin = monster.money * 0.8f;
        float moneyRangeMax = monster.money * 1.2f;
        float randomMoney = Random.Range(moneyRangeMin, moneyRangeMax) *  (1 + ItemManagerScene.instance.state[5]/100);

        int currentMoney = ancienMoney + Mathf.RoundToInt(randomMoney);
        PlayerPrefs.SetInt("money", currentMoney);
        CanvasManager.instance.inventory.SetGoldCanvas();
    }

    private void SetItemInScene()
    {
        float addPositionX = -0.4f;
        for (int i = 0; i < nbLoots; i++)
        {
            int idItemRandom = Random.Range(0, items.Length);
            int randomNumbre = Random.Range(0, 100);
            int idItem = items[idItemRandom].GetComponent<ItemDrop>().GetIdItem();
            ItemDatabase itemDatabase = CanvasManager.instance.inventory.GetItemDatabase();
            if (randomNumbre <= itemDatabase.item[idItem].dropChance)
            {
                GameObject itemObject = items[idItemRandom];
                addPositionX += 0.15f;
                Vector3 spawnPosition = new Vector3(transform.position.x + addPositionX, transform.position.y, transform.position.z);
                Instantiate(itemObject, spawnPosition, transform.rotation);
            }
        }
    }

    private void CatchMonsterInList()
    {
        SoundManager.instance.Sound(18);
        CanvasManager.instance.monsterCatch.CatchMonster(monster);
        string texte = "You captured a " + monster.name;
        CanvasManager.instance.SystemMessage(texte,1);
        
        Destroy(gameObject); // Faire des effet plus tard 
    }

    public void CatchMonster(Pokeball pokeball)
    {
        if(monster.playerPrefs!=null)
        {
            if(CanvasManager.instance.monsterCatch.CanCatch())
            {
                System.Random random = new System.Random();
                float randomValue = (float)random.NextDouble();
                
                float healthPercentage = (float)monster.currentLife / (float)monster.maxLife;
                float captureProbability = (pokeball.catchChance/200f) + (monster.catchChance / 100f);
                if (healthPercentage > 0.4f)
                {
                    float reduction = Mathf.Clamp((1.5f - healthPercentage),0.5f,1);
                    captureProbability = (pokeball.catchChance/200f) + (monster.catchChance / 100f)*reduction;
                }

                if (randomValue <= captureProbability)
                {
                    CatchMonsterInList();
                }
                else
                {
                    string texte = "You failed to capture the monster";
                    CanvasManager.instance.SystemMessage(texte,1);
                }
            }
            else
            {
                string texte = "You have too many monsters";
                CanvasManager.instance.SystemMessage(texte,1);
            }
            
        }
        else
        {
            string texte = "You can't capture this monster";
            CanvasManager.instance.SystemMessage(texte,1);
        }
        
    }

}
