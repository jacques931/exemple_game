using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Comma))
        {
            string texte = "Are you sure you want to delete all your data ?";
            CanvasManager.instance.confirmPanel.gameObject.SetActive(true);
            CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,ResetGameData);
        }

        if(Input.GetKey(KeyCode.F3) && Input.GetKey(KeyCode.F10) && Input.GetKey(KeyCode.F5))
        {
            string texte = "Do you want to reset your positions ?";
            CanvasManager.instance.confirmPanel.gameObject.SetActive(true);
            CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,ResetPosition);
        }
    }

    private void ResetPosition()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>());
        PlayerPosition.SavePosition("playerPosition",Vector3.zero);
        SceneManager.LoadScene(0);
    }

    private void ResetGameData()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>());
        PlayerPrefs.DeleteAll();
        MonsterCatchManager.Instance.SaveMonsterList(new Monster[0]);
        ItemManager.Instance.SaveItemList(new Item[0],"itemBar.json");
        ItemManager.Instance.SaveItemList(new Item[0]);
        SceneManager.LoadScene(0);
    }
}
