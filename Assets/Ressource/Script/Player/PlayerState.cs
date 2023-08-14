using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState : PlayerAttack
{
    private bool isInvincible=true;
    private void Start()
    {
        playerState = MonsterCatchManager.Instance.LoadMonsterList()[PlayerPrefs.GetInt("idPlayer")];
        playerMove = GetComponent<PlayerMove>();
        SetXpCanvas();
        SetLifeCanvas();
        ResetAttack();
        StartCoroutine(RegenerationLife());
        StartCoroutine(StartPlayer());
    }

    private IEnumerator StartPlayer()
    {
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    private IEnumerator RegenerationLife()
    {
        while (true)
        {
            yield return new WaitForSeconds(35f);
            int recovery = (int)(playerState.maxLife * (playerState.recoveryLife + ItemManagerScene.instance.state[4]) / 100f);
            RecoveryLife(recovery);
        }
    }

    public void ApplyDamage(int damage)
    {
        if(!isInvincible)
        {
            SoundManager.instance.Sound(14);
            int finalDamage = (int)Mathf.Max(damage - (playerState.defense + ItemManagerScene.instance.state[1]), 0);
            playerState.currentLife -= finalDamage > 0 ? finalDamage : 1;
            SaveMonster();
            if(playerState.currentLife<1)
            {
                Dead();
            }
            CanvasManager.instance.monsterTeam.UpdateLifeMonster(playerState);
        }
        
    }

    public void ApplyDamageObject(int pourcent)
    {
        if (!isInvincible)
        {
            int randomValue = UnityEngine.Random.Range(0,101);
            if (randomValue >= 10)
            {
                SoundManager.instance.Sound(14);
                playerState.currentLife -= (int)(playerState.maxLife * pourcent / 100);
                SaveMonster();
                if (playerState.currentLife < 1)
                {
                    Dead();
                }
                CanvasManager.instance.monsterTeam.UpdateLifeMonster(playerState);
            }
        }
    }

    public bool RecoveryLife(int recovery)
    {
        if (playerState.currentLife == playerState.maxLife)
            return false;

        playerState.currentLife = Mathf.Clamp(playerState.currentLife + recovery, 0, playerState.maxLife);
        SaveMonster();
        CanvasManager.instance.monsterTeam.UpdateLifeMonster(playerState);
        return true;
    }

    private void Dead()
    {
        SoundManager.instance.Sound(13);
        playerState.currentLife = 0;
        GetComponent<Animator>().SetBool("Move", false);
        SaveMonster();

        //Dead Visuel
        playerMove.SetStopMove(true);
        GetComponent<Animator>().SetTrigger("Dead");
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.4f);
        GetComponent<Collider2D>().isTrigger = true;
        
        //Different possibilité de dead 
        int newIdMonster = CanvasManager.instance.monsterTeamManager.HaveMonsterInTeamAndAlive();
        if(newIdMonster!=-1)
        {
            StartCoroutine(ChangePlayer(newIdMonster));
        }
        else
        {
            TerrainObjectManager terrainManager = GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainObjectManager>();
            Action fonction = terrainManager.ReturnInCity;

            int idItemRessurection = 1;
            bool isInInventory = CanvasManager.instance.inventory.CheckItemInInventory(idItemRessurection) != -1;
            bool isInItemBar = CanvasManager.instance.itemBar.CheckItemInBar(idItemRessurection) != -1;

            if (isInInventory || isInItemBar)
            {
                string texte = "You are dead, want to resurrect in the dungeon";
                CanvasManager.instance.deadPanel.SetActive(true);

                Action ressurectionAction = () => PlayerRessurection(isInInventory);
                CanvasManager.instance.deadPanel.GetComponent<ConfirmPanelScript>().SetRessurectionPanel(texte, ressurectionAction, fonction);
            }
            else
            {
                string texte = "You died, you will rise again in the city";
                CanvasManager.instance.deadPanel.SetActive(true);
                CanvasManager.instance.deadPanel.GetComponent<ConfirmPanelScript>().SetDeadPanel(texte,fonction);
            }

            StartCoroutine(WaitToReturnCity(fonction));
            
        }
    }

    private IEnumerator ChangePlayer(int newIdMonster)
    {
        yield return new WaitForSeconds(1.2f);
        PlayerPrefs.SetInt("idPlayer",newIdMonster);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CreateMonsterPlayer(transform.position,true);
    }

    private IEnumerator WaitToReturnCity(Action fonction)
    {
        yield return new WaitForSeconds(15f);
        fonction.Invoke();
        CanvasManager.instance.deadPanel.SetActive(false);
    }

    private void PlayerRessurection(bool isInventory)
    {
        CanvasManager.instance.monsterTeam.RecoveryAllMonsterLife();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CreateMonsterPlayer(transform.position,true);
        int idItemRessurection = 1;
        if(isInventory)
            CanvasManager.instance.inventory.RemoveItem(idItemRessurection);
        else
            CanvasManager.instance.itemBar.RemoveItem(idItemRessurection);

        CanvasManager.instance.monsterTeam.ResetWaitTime();
    }

    public void SetXp(float xp)
    {
        if(!PlayerLevelMax())
        {
            playerState.xp += xp;
            float lessXp = playerState.maxXp - playerState.xp;
            if(lessXp<1)
            {
                LevelUp(lessXp);
            }
            SaveMonster();
        }
        
    }

    public bool PlayerLevelMax()
    {
        if(playerState.level==99)
        {
            return true;
        }
        return false;
    }

    public void LevelUp(float lessXp)
    {
        playerState.level +=1;
        // Amelioration des states
        playerState.maxLife += (int)(playerState.startLife*0.1f);
        playerState.defense *= 1.03f;
        if(playerState.defense==0 && playerState.level==8) // Permet au faible monstre d'avoir un defense
        {
            playerState.defense = 1;
        }
        playerState.maxXp *= 1.10f;

        foreach(AttackInput attack in playerState.input)
        {
            attack.damage *= 1.05f;
        }
        // Fin des ameliorations

        playerState.xp = (PlayerLevelMax()) ? 0 : -lessXp;
        playerState.currentLife = playerState.maxLife;
        if(playerState.level>=playerState.SecondSkillLevel && playerState.input.Length>1)
            playerState.input[1].canUse = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ApplySkillCanvas(playerState);

        CanvasManager.instance.monsterTeam.UpdateLifeMonster(playerState);
        SoundManager.instance.Sound(5);
        SaveMonster();
    }

    //Canvas Modification

    private void SaveMonster()
    {
        Monster[] monsters = CanvasManager.instance.monsterCatch.GetListMonster().ToArray();
        monsters[PlayerPrefs.GetInt("idPlayer")] = playerState;
        MonsterCatchManager.Instance.SaveMonsterList(monsters);
        CanvasManager.instance.monsterCatch.SetDataMonster();
        SetLifeCanvas();
        SetXpCanvas();
    }

    private void SetLifeCanvas()
    {
        CanvasManager.instance.SetPlayerInformation(playerState.currentLife,playerState.maxLife,playerState.level);
    }

    private void SetXpCanvas()
    {
        float xpPourcent = playerState.xp / playerState.maxXp;
        CanvasManager.instance.SetXpFilled(xpPourcent);
    }
}
