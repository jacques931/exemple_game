using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueDatabase dialogueDatabase;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text dialogueTxt;
    [SerializeField] private GameObject nextImg;

    private Dialogue currentDialogue;
    private int idScene;
    private bool playerCanMove;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)&& nextImg.activeSelf)
        {
            SoundManager.instance.Sound(0);
            idScene++;
            if(idScene<currentDialogue.scenes.Length)
                CreateScene(idScene);
            else
            {
                ChangePlayerMove(true);
                gameObject.SetActive(false);
            }
                
        }
    }

    public void QuitDialogue()
    {
        SoundManager.instance.Sound(0);
        ChangePlayerMove(false);
        SoundManager.instance.StopSound(4);
        gameObject.SetActive(false);
    }

    public void CreateDialogue(int idDialogue)
    {
        gameObject.SetActive(true);
        currentDialogue = dialogueDatabase.dialogue[idDialogue];
        idScene = 0 ;
        CreateScene(0);
        playerCanMove = currentDialogue.playerStop;
        ChangePlayerMove(playerCanMove);
    }

    private void ChangePlayerMove(bool value)
    {
        PlayerMove player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMove>();
        if(player!=null)
            player.SetStopMove(value);
    }

    private void CreateScene(int sceneId)
    {
        Scene scene = currentDialogue.scenes[sceneId];
        nameTxt.text = scene.name;
        StartCoroutine(ReadText(scene.dialogueTxt));
    }

    private IEnumerator ReadText(string[] text)
    {
        dialogueTxt.text = "";
        nextImg.SetActive(false);
        SoundManager.instance.Sound(4);
        for(int i=0;i<text.Length;i++)
        {
            foreach (char letter in text[i])
            {
                dialogueTxt.text += letter;
                yield return new WaitForSeconds(PlayerPrefs.GetFloat("dialogueSpeed"));
            }

            if(i<text.Length-1)
                dialogueTxt.text += '\n';
        }
        SoundManager.instance.StopSound(4);
        nextImg.SetActive(true);
    }
}
