using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterTerrain : MonoBehaviour
{
    [SerializeField] private MonsterDatabase monsterDatabase;
    [SerializeField] private Image monsterIcon;
    [SerializeField] private Text monsterStatut;
    private int idMonster;
    
    public void SetInformation(int _idMonster)
    {
        idMonster = _idMonster;
        Monster monster = monsterDatabase.monster[idMonster];
        monsterIcon.sprite = monster.playerIcon;
        if (monster.playerPrefs != null)
        {
            monsterStatut.text = CanvasManager.instance.monsterCatch.CheckMonsterIsCaptured(idMonster) ? "Captured" : "No Captured";
        }
        else
        {
            monsterStatut.text = "Can't Captured";
        }
        

    }

    public void MouseEnter()
    {
        Monster monster = monsterDatabase.monster[idMonster];
        if(monster !=null)
        {   
            string jumpOrFly = monster.canFly ? "Fly : " : "Jump : ";
            string texteMonster = "Level : " + monster.level + " ~ " + (monster.level + 3)+ '\n' + 
                    "Life : " + monster.maxLife + " ~ " + (int)(monster.maxLife + monster.maxLife*0.1f*3) + '\n' + 
                    "Attack : " + monster.input[0].damage + " ~ " + (int)(monster.input[0].damage * Mathf.Pow(1 + 0.05f, 3)) + '\n' + 
                    "Defense : " + monster.defense + " ~ " + (int)(monster.defense * Mathf.Pow(1 + 0.03f, 3)) + '\n' + 
                    "Speed : " + monster.speed + '\n' + 
                    jumpOrFly + monster.jump + '\n' + 
                    "Spawn chance : " + monster.spawnChance + "%" + '\n' +
                    "Catch chance : " + monster.catchChance + "%";

            GameObject monsterPanel = transform.parent.parent.parent.parent.parent.gameObject;
            CanvasManager.instance.tooltip.SetTooltip(monsterIcon.transform.position,monsterPanel,monster.playerIcon,monster.name,monster.rarity.ToString(),texteMonster);
        }
    
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

}
