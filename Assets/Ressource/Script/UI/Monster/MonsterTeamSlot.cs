using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MonsterTeamSlot : MonoBehaviour
{
    [SerializeField] private Image monsterIcon;
    [SerializeField] private Text monsterInformation;

    private Monster monster;
    private int slotPosition;
    private Sprite startIcon;

    public void UpdateSlot(Monster _monster,int _slotPosition)
    {
        if(startIcon==null)
            startIcon = monsterIcon.sprite;

        monster = _monster;
        slotPosition = _slotPosition;
        monsterIcon.sprite = (monster.idMonster != 0) ? monster.playerIcon : startIcon;
        monsterInformation.text = (monster.idMonster != 0) ? $"Level: {monster.level}\nLife: {monster.currentLife}/{monster.maxLife}" : "None";
    }

    public void UpdateResetSlot(Monster _monster,int _slotPosition)
    {
        monster = _monster;
        monster.id = -1;
        slotPosition = _slotPosition;
    }

    public void ClickMonster()
    {
        SoundManager.instance.Sound(0);
        MonsterList monsterList = CanvasManager.instance.monsterList;
        monsterList.SetListOfMonster(!monsterList.gameObject.active,transform.position,slotPosition);
    }

    public void ClickChangeMonster()
    {
        SoundManager.instance.Sound(0);
        if(monster.id!=-1 || CheckNotLastMonsterTeam())
        {
            PlayerPrefs.SetInt("monsterTeam" + slotPosition,monster.id);
            CanvasManager.instance.monsterTeamManager.SetTeam();
            CanvasManager.instance.monsterList.gameObject.SetActive(false);
            CanvasManager.instance.monsterCatch.ResetIconMonster();
        }
        
    }

    private bool CheckNotLastMonsterTeam()
    {
        int notMonster = 0;
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("monsterTeam" + i) == -1)
            {
                notMonster++;
            }
        }
        
        return notMonster != 2;
    }

    public void MouseEnter()
    {
        if(monster !=null && monster.idMonster!=0)
        {   
            string lastSkill = monster.input.Length > 2 ? (monster.input[2].canUse ? "Last Skill is Unlock\n" : "Last Skill is Lock\n") : "";
            string jumpOrFly = monster.canFly ? "Fly : " : "Jump : ";
            string texteMonster = "Level : " + monster.level + '\n' + 
                    "Life : " + monster.currentLife + " / " + monster.maxLife + '\n' + 
                    "Attack : " + monster.input[0].damage + '\n' + 
                    "Defense : " + monster.defense + '\n' + 
                    "Regneration : " + Math.Round(monster.recoveryLife, 2) + "% / 35sec" + '\n' + 
                    "Speed : " + monster.speed + '\n' + 
                    jumpOrFly + monster.jump + '\n' + 
                    lastSkill +
                    "Xp : " + (int)monster.xp + " / " + (int)monster.maxXp;

            GameObject monsterPanel = transform.parent.gameObject;
            CanvasManager.instance.tooltip.SetTooltip(transform.position,monsterPanel,monster.playerIcon,monster.name,monster.rarity.ToString(),texteMonster);
        }
    
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }
}
