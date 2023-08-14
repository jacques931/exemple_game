using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTeam : MonoBehaviour
{
    private bool haveChangePlayer;
    [SerializeField] private float waitTime;

    private void OnEnable()
    {
        SetMonsterInTeam();
    }

    public bool GetHaveChangePlayer()
    {
        return haveChangePlayer;
    }

    public void UpdateLifeMonster(Monster player)
    {
        for(int i=0;i<3;i++)
        {
            if(PlayerPrefs.GetInt("monsterTeam" + i)==player.id)
            {
                transform.GetChild(i).GetChild(0).GetComponent<SlotTeam>().UpdateSlot(player);
                break;
            }
        }
        CanvasManager.instance.monsterCatch.SetCurrentData();
    }

    public void RecoveryAllMonsterLife()
    {
        Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
        for(int i=0;i<3;i++)
        {
            if(PlayerPrefs.GetInt("monsterTeam" + i) !=-1)
            {
                Monster monster = monsters[PlayerPrefs.GetInt("monsterTeam" + i)];
                monster.currentLife = monster.maxLife;
                UpdateLifeMonster(monster);
            }  
        }
    
        MonsterCatchManager.Instance.SaveMonsterList(monsters);
        CanvasManager.instance.monsterCatch.SetCurrentData();
    }

    private void SetMonsterInTeam()
    {
        haveChangePlayer = false;
        Monster[] monsters = GetMonsterTeam();
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<SlotTeam>().UpdateSlot(monsters[i]);
            // Reset le temps d'attente pour le prochain monstre 
            transform.GetChild(i).GetChild(0).GetComponent<SlotTeam>().ApplyWaitTime(0,0);
        }
    }

    private Monster[] GetMonsterTeam()
    {
        List<Monster> newMonsters = new List<Monster>();
        Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
        for(int i=0;i<3;i++)
        {
            if(PlayerPrefs.GetInt("monsterTeam" + i) !=-1)
            {
                newMonsters.Add(monsters[PlayerPrefs.GetInt("monsterTeam" + i)]);
            }
            else
            {
                newMonsters.Add(new Monster());
            }
            
        }

        return newMonsters.ToArray();
    }

    public void ChangePlayer(int idMonster)
    {
        Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
        if(monsters[idMonster].currentLife>0)
        {
            StartCoroutine(WaitChangePlayer());
            PlayerPrefs.SetInt("idPlayer",idMonster);
            Vector3 currentPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CreateMonsterPlayer(currentPosition,true);
        }
        
    }

    public void UpdateIconMonster()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<SlotTeam>().UpdateIconMonster();
        }
    }

    public void ResetWaitTime()
    {
        haveChangePlayer = false;
        foreach(Transform child in transform)
        {
            child.GetChild(0).GetComponent<SlotTeam>().ApplyWaitTime(0,0);
        }
    }

    private IEnumerator WaitChangePlayer()
    {
        haveChangePlayer = true;

        // Comptage le temps restant proportionnellement entre 0 et 1
        float startTime = Time.time;
        float endTime = startTime + waitTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            float normalizedTime = (timeRemaining / waitTime);

            foreach(Transform child in transform)
            {
                child.GetChild(0).GetComponent<SlotTeam>().ApplyWaitTime(normalizedTime,timeRemaining);
            }

            yield return null;
        }

        haveChangePlayer = false;
    }
}
