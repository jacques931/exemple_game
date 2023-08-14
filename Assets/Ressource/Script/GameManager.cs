using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    private Monster[] listMonster;
    [SerializeField] CanvasManager canvas;
    [SerializeField] GameObject playerPanel;
    [SerializeField] GameObject skillPanel;
    [SerializeField] GameObject teamPanel;
    private MonsterCatchManager monsterCatchManager;

    [SerializeField] GameObject playerObject;
    [SerializeField] Sprite playerSprite;
    [SerializeField] MonsterDatabase monsterDatabase;

    private void Awake()
    {
        monsterCatchManager = MonsterCatchManager.Instance;
    }

    public void CreatePlayerInCity(Vector3 newPosition)
    {
        if(!PlayerPosition.isFirstPosition("playerPosition"))
            newPosition = PlayerPosition.LoadPosition("playerPosition");
        GameObject player = Instantiate(playerObject,newPosition,transform.rotation);
        canvas.ChangeIconPlayer(playerSprite);
        CanvasManager.instance.pokeballManager.WaitNextPokeball(0);
        playerPanel.SetActive(false);
        skillPanel.SetActive(false);
        teamPanel.SetActive(false);
    }

    public GameObject CreateMonsterPlayer(Vector3 newPosition,bool inBattle=false)
    {
        if(!inBattle)
        {
            PlayerPrefs.SetInt("idPlayer",CanvasManager.instance.monsterTeamManager.HaveMonsterInTeamAndAlive());
        }
        else
        {
            CanvasManager.instance.monsterTeam.UpdateIconMonster();
        }

        //Enleve la possibilité de l'arret des recharge pokeball 
        CanvasManager.instance.pokeballManager.WaitNextPokeball(0);
        //Recuperation de la liste des monstre capturer
        listMonster = monsterCatchManager.LoadMonsterList();
        Monster monster = listMonster[PlayerPrefs.GetInt("idPlayer")];
        // Recuperation des image et prefable
        monster.SetState(monsterDatabase.monster[monster.idMonster]);
        canvas.ChangeIconPlayer(listMonster[PlayerPrefs.GetInt("idPlayer")].playerIcon);

        GameObject player = Instantiate(monster.playerPrefs,newPosition,transform.rotation);
        PlayerMove playerScript = player.GetComponent<PlayerMove>();
        playerScript.instantiatePlayer(monster.speed,monster.jump); // On va surement mettre le le script monster directement
        DeleteAncienPlayer();
        playerPanel.SetActive(true);
        skillPanel.SetActive(true);
        teamPanel.SetActive(true);
        ApplySkillCanvas(monster);
        return player;
    }

    public void DeleteAncienPlayer()
    {
        GameObject ancienPlayer = GameObject.FindGameObjectWithTag("Player").gameObject;
        Destroy(ancienPlayer);
    }

    public void ApplySkillCanvas(Monster player)
    {
        for (int i = 0; i < skillPanel.transform.childCount; i++)
        {
            SkillCanvas child = skillPanel.transform.GetChild(i).GetComponent<SkillCanvas>();
            child.gameObject.SetActive(false);
            
            if (i < player.input.Length)
            {
                child.SetSkill(player.input[i]);
                child.CanUseSkill(player.input[i].canUse);
                child.gameObject.SetActive(true);
            }
        }
    }

}
