using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManage : MonoBehaviour
{
    [SerializeField] private Transform allPositionMonster;
    [SerializeField] private int amountMonster;
    [SerializeField] private MonsterDatabase monsterDatabase;
    [SerializeField] private GameObject[] monsterPrefs;
    // Start is called before the first frame update
    void Start()
    {
        CreateMonsterInField();
        MonsterIA.list.Clear();
    }
    
    public GameObject[] getMonster()
    {
        return monsterPrefs;
    }

    private void CreateMonsterInField()
    {
        if(amountMonster==-1)
            amountMonster = 1;
        else
            amountMonster = Random.Range(amountMonster - 1, amountMonster + 3);
        if (amountMonster > allPositionMonster.childCount)
        {
            Debug.LogError("Il y a plus de monstres que de places disponibles : manque " + (amountMonster - allPositionMonster.childCount) + " place");
            return;
        }

        for (int i = 0; i < amountMonster; i++)
        {
            int position;
            int monsterId;

            do
            {
                position = Random.Range(0, allPositionMonster.childCount);
            }
            while (allPositionMonster.GetChild(position).childCount > 0);

            do
            {
                monsterId = Random.Range(0, monsterPrefs.Length);
            }
            while (monsterDatabase.monster[monsterPrefs[monsterId].GetComponent<MonsterScript>().GetMonster().idMonster].spawnChance < Random.Range(1, 100));

            GameObject monsterPrefab = monsterPrefs[monsterId];
            MonsterScript monsterScript = monsterPrefab.GetComponent<MonsterScript>();
            monsterScript.SetDataMonster(monsterDatabase.monster[monsterScript.GetMonster().idMonster]);

            Instantiate(monsterPrefab, allPositionMonster.GetChild(position).position, transform.rotation, allPositionMonster.GetChild(position));
        }
    }

}

