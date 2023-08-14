using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTeam : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private Image monsterIcon;
    [SerializeField] private Image fillLife;
    [SerializeField] private Text nameMonsterTxt;

    //Chargement d'un nouvea monstre
    [SerializeField] private Image waitMonsterImg;

    private Monster monster;
    private MonsterTeam monsterTeam;

    private void Start()
    {
        monsterTeam = transform.parent.parent.GetComponent<MonsterTeam>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(key))
        {
            ChangePlayerSlot();
        }
    }

    public void ChangePlayerSlot()
    {
        if(PlayerPrefs.GetInt("idPlayer")!=monster.id && !monsterTeam.GetHaveChangePlayer())
            monsterTeam.ChangePlayer(monster.id);
    }

    public void ApplyWaitTime(float pourcent,float remainingTime)
    {
        Text waitMonsterTxt = waitMonsterImg.transform.GetChild(0).GetComponent<Text>();
        if(!waitMonsterTxt.gameObject.active)
            waitMonsterTxt.gameObject.SetActive(true);

        waitMonsterImg.fillAmount = pourcent;
        waitMonsterTxt.text = remainingTime.ToString("F2");

        if(remainingTime<0.1f)
        {
            waitMonsterTxt.gameObject.SetActive(false);
        }
    }

    public void UpdateSlot(Monster _monster)
    {
        monster = _monster;
        if(monster.idMonster !=0)
        {
            transform.parent.gameObject.SetActive(true);
            monsterIcon.sprite = monster.playerIcon;
            nameMonsterTxt.text = "Lv." + monster.level + " " + monster.name;
            SetFillLife();
            monsterIcon.color = (monster.currentLife < 1) ? Color.gray : Color.white;
            UpdateIconMonster();
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
        
    }

    public void UpdateIconMonster()
    {
        if(monster.id == PlayerPrefs.GetInt("idPlayer"))
            monsterIcon.transform.parent.GetComponent<Image>().color  = new Color(134f/255f,153f/255,1f,1f);
        else
            monsterIcon.transform.parent.GetComponent<Image>().color  = Color.white;
    }

    public void SetFillLife()
    {
        fillLife.fillAmount = (float)monster.currentLife / (float)monster.maxLife;
        if(fillLife.fillAmount>0.25f)
            fillLife.color = Color.green;
        else
            fillLife.color = Color.red;
    }
}
