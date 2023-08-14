using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private Transform monsterManage;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private Transform playerStartPostion;

    public Vector3 GetNewPosition()
    {
        return playerStartPostion.position;
    }

    public (Monster[], Item[]) GetMonstersAndItemsInTerrain()
    {
        List<Monster> listMonster = new List<Monster>();
        List<Item> listItem = new List<Item>();

        GameObject[] monsterObject = monsterManage.GetComponent<MonsterManage>().getMonster();
        foreach(GameObject monster in monsterObject)
        {
            Monster monsterInScript = monster.GetComponent<MonsterScript>().GetMonster();
            listMonster.Add(monsterInScript);
            foreach(int idItem in monster.GetComponent<MonsterScript>().GetItem())
            {
                if(!ItemisInList(listItem,idItem))
                {
                    Item item = itemDatabase.item[idItem];
                    listItem.Add(item);
                }
                
            }
        }
        return (listMonster.ToArray(), listItem.ToArray());
    }

    private bool ItemisInList(List<Item> itemList,int newIdItem)
    {
        foreach(Item item in itemList)
        {
            if(item.id == newIdItem)
            {
                return true;
            }
        }

        return false;
    }
}
