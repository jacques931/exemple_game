using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList : MonoBehaviour
{
    [SerializeField] private Transform monsterListArea;
    [SerializeField] private GameObject SlotMonsterPrefs;

    private int idSlot;

    public void SetListOfMonster(bool active,Vector3 toolPosition,int idSlotMonster)
    {
        gameObject.SetActive(idSlot == idSlotMonster ? active : true);

        idSlot = idSlotMonster;
        float addAMount = 160;
        if(toolPosition.x + addAMount >1470)
        {
            addAMount = -addAMount;
        }
        transform.position = new Vector3(toolPosition.x + addAMount ,Mathf.Clamp(toolPosition.y+90, 170f, 730f),transform.position.z);

        SetMonsterInList(idSlotMonster);
    }

    public void ResetMonsterList()
    {
        SetMonsterInList(idSlot);
    }

    private void SetMonsterInList(int idSlotMonster)
    {
        DeleteAllMonsterList();
        List<Monster> monsters = CanvasManager.instance.monsterCatch.GetListMonster();

        //Initialise une option qui met aucun monstre
        GameObject resetSlot = Instantiate(SlotMonsterPrefs, monsterListArea);
        resetSlot.transform.GetChild(0).GetComponent<MonsterTeamSlot>().UpdateResetSlot(new Monster(), idSlotMonster);

        for (int i = 0; i < monsters.Count; i++)
        {
            if(monsters[i].idMonster!=0)
            {
                if (CanAddMonsterInList(monsters,0,i) && CanAddMonsterInList(monsters,1,i) && CanAddMonsterInList(monsters,2,i))
                {
                    GameObject monsterSlot = Instantiate(SlotMonsterPrefs, monsterListArea);
                    monsterSlot.transform.GetChild(0).GetComponent<MonsterTeamSlot>().UpdateSlot(monsters[i], idSlotMonster);
                }
            }
            
        }
    }

    private bool CanAddMonsterInList(List<Monster> monsters,int idSlotMonster,int currentIdlot)
    {
        if(PlayerPrefs.GetInt("monsterTeam" + idSlotMonster)!=-1)
        {
            return currentIdlot != monsters[PlayerPrefs.GetInt("monsterTeam" + idSlotMonster)].id;
        }
        return true;
    }

    private void DeleteAllMonsterList()
    {
        foreach(Transform child in monsterListArea)
        {
            Destroy(child.gameObject);
        }
    }
}
