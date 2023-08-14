using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTeamManager : MonoBehaviour
{
    [SerializeField] private Transform monsterArea;

    private void OnDisable()
    {
        CanvasManager.instance.monsterList.gameObject.SetActive(false);
    }

    private void CheckIsFirst()
    {
        if(PlayerPrefs.GetInt("start")==0)
        {
            for(int i=0;i<monsterArea.childCount;i++)
            {
                PlayerPrefs.SetInt("monsterTeam" + i,-1);
            }
            CanvasManager.instance.dialogueManager.CreateDialogue(0);
            PlayerPrefs.SetFloat("backsound",0.8f);
            PlayerPrefs.SetFloat("dialogueSpeed",0.01f);
            PlayerPrefs.SetInt("start",1);
        }
    }
    
    public int HaveMonsterInTeamAndAlive()
    {
         for(int i=0;i<3;i++)
        {
            if(PlayerPrefs.GetInt("monsterTeam" + i)!=-1)
            {
                Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
                if(monsters[PlayerPrefs.GetInt("monsterTeam" + i)].currentLife>0)
                {
                    return PlayerPrefs.GetInt("monsterTeam" + i);
                }
            }
        }

        return -1;
    }

    public void SetTeam()
    {
        CheckIsFirst();
        Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
        for(int i=0;i<monsterArea.childCount;i++)
        {
            if(PlayerPrefs.GetInt("monsterTeam" + i)!=-1)
            {
                Monster monster = monsters[PlayerPrefs.GetInt("monsterTeam" + i)];
                int idSlot = i;
                monsterArea.GetChild(i).GetChild(0).GetComponent<MonsterTeamSlot>().UpdateSlot(monster,idSlot);
            }
            else
            {
                int idSlot = i;
                monsterArea.GetChild(i).GetChild(0).GetComponent<MonsterTeamSlot>().UpdateSlot(new Monster(),idSlot);
            }
            
        }
    }
}
