using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MonsterSlot : MonoBehaviour
{
    private int idMonsterCatch=-1;
    private Monster monster;
    private Sprite iconNone;

    private void Awake()
    {
        iconNone = transform.GetChild(0).GetComponent<Image>().sprite;
    }

    public int getId()
    {
        if(monster!=null && monster.idMonster!=0)
        {
            return idMonsterCatch;
        }
        return -1;
    }

    public void InstanceIcon(Monster _monster)
    {
        monster = _monster;
        idMonsterCatch = monster.id;
        Image IconImg = transform.GetChild(0).GetComponent<Image>();
        IconImg.sprite = monster.playerIcon != null ? monster.playerIcon : iconNone;
        if(transform.childCount>1)
            AddNewSkill();
    }

    private void AddNewSkill()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        if(monster.idMonster!=0 && monster.input.Length>2 && !monster.input[2].canUse)
        {
            int monsterUnlockId = CanvasManager.instance.inventory.GetIdItemToUnlockSkill(monster.idMonster);

            if(monsterUnlockId!=-1)
            {
                if(CanvasManager.instance.inventory.CheckItemInInventory(monsterUnlockId)!=-1)
                {
                    transform.GetChild(1).gameObject.SetActive(true);
            
                }
            }
        }
    }

    public void ClickUnlockSkill()
    {
        SoundManager.instance.Sound(0);
        CanvasManager.instance.confirmPanel.SetActive(true);
        string texte = "Are you sure you want to use your item to unlock this monster's skill ?";
        CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,UnlockSkill);
    }

    private void UnlockSkill()
    {
        int monsterUnlockId = CanvasManager.instance.inventory.GetIdItemToUnlockSkill(monster.idMonster);
        monster.input[2].canUse = true;
        CanvasManager.instance.monsterCatch.SaveMonster(monster);
        CanvasManager.instance.inventory.RemoveItem(monsterUnlockId);
        CanvasManager.instance.monsterCatch.SetCurrentData();

        string message = "You have unlocked the last skill of the monster";
        CanvasManager.instance.SystemMessage(message);
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
