using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManagerScene : MonoBehaviour
{
    [SerializeField] private Transform buffPanel;
    [SerializeField] private GameObject buffPrefs;
    [SerializeField] private Sprite[] buffSprite;
    public static ItemManagerScene instance;

    //Buff 0 : captureSpeed / 1 : defense / 2 : speed / 3 : attack / 4 : recoveryLife / 5 : money / 6 : skillSpeed
    public float[] state = new float[7];
    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private List<GameObject> activeBuffObject = new List<GameObject>();

    private void Awake()
    {
        if(instance!=null)
        {
            return;
        }
        instance = this;

        for(int i=0;i<state.Length;i++)
        {
            activeCoroutines.Add(null);
            activeBuffObject.Add(null);
        }
    }

    public bool ApplyItemEffect(ItemEffect itemEffect)
    {
        switch (itemEffect.effect)
        {
            case Effect.Heal:
                return Heal(itemEffect.valueEffect);
            case Effect.Level_Up:
                return LevelUp(itemEffect.valueEffect);
            //Buff
            case Effect.Capture_Speed:
                return AddState(0,itemEffect);
            case Effect.Shield:
                return AddState(1,itemEffect);
            case Effect.Speed:
                return AddState(2,itemEffect);
            case Effect.Attack:
                return AddState(3,itemEffect);
            case Effect.Recovery_Life:
                return AddState(4,itemEffect);
            case Effect.Get_Money:
                return AddState(5,itemEffect);
            case Effect.Skill_Speed:
                return AddState(6,itemEffect);
            
            default:
                return false;
        }
    }

    private bool AddState(int position,ItemEffect itemEffect)
    {
        if (state[position] < itemEffect.valueEffect)
        {
            state[position] = itemEffect.valueEffect;
            // Vérifier si une coroutine pour cette position est déjà en cours d'exécution.
            if (activeCoroutines[position] != null)
            {
                // Arrêter l'ancienne coroutine pour cette position.
                StopCoroutine(activeCoroutines[position]);
                activeBuffObject[position].SetActive(false);
            }

            // Stocker la nouvelle coroutine active pour cette position.
            activeCoroutines[position] = StartCoroutine(WaitRemoveState(position, itemEffect));
            return true;
        }

        return false;
    }

    private IEnumerator WaitRemoveState(int position, ItemEffect itemEffect)
    {
        activeBuffObject[position] = Instantiate(buffPrefs,buffPanel);
        activeBuffObject[position].GetComponent<BuffScript>().SetBuff(itemEffect,buffSprite[position]);

        // Comptage le temps restant proportionnellement entre 0 et 1
        float startTime = Time.time;
        float endTime = startTime + itemEffect.timeEffect;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            float normalizedTime = (timeRemaining / itemEffect.timeEffect);

            activeBuffObject[position].GetComponent<BuffScript>().SetFilled(normalizedTime);

            yield return null;
        }
        
        state[position] = 0;
        activeBuffObject[position].SetActive(false);
        CanvasManager.instance.buffInformationPanel.SetActive(false);
        activeCoroutines[position] = null;
    }

    private bool Heal(float value)
    {
        PlayerState player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        return player.RecoveryLife((int)value);
    }

    private bool LevelUp(float levelUp)
    {
        PlayerState player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        for (int i = 0; i < levelUp; i++)
        {
            if (player.PlayerLevelMax())
            {
                return i > 0;
            }
            player.LevelUp(0);
        }
        return true;
    }

}
