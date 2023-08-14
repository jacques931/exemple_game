using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffScript : MonoBehaviour
{
    private ItemEffect itemEffect;

    public void SetBuff(ItemEffect effect,Sprite buffSprite)
    {
        itemEffect = effect;
        GetComponent<Image>().sprite = buffSprite;
    }

    public void SetFilled(float value)
    {
        transform.GetChild(0).GetComponent<Image>().fillAmount = value;
    }

    public void MouseEnter()
    {
        string textEffect = "";
        switch (itemEffect.effect)
        {
            case Effect.Speed:
                textEffect = "Increase speed of " + itemEffect.valueEffect;
                break;
            case Effect.Shield:
                textEffect = "Increase defense of " + itemEffect.valueEffect;
                break;
            case Effect.Attack:
                textEffect = "Increase attack of " + itemEffect.valueEffect + "%";
                break;
            case Effect.Recovery_Life:
                textEffect = "Increase life of " + itemEffect.valueEffect + "%";;
                break;
            case Effect.Get_Money:
                textEffect = "Increases the money you earn by " + itemEffect.valueEffect + "%";;
                break;
            case Effect.Skill_Speed:
                textEffect += "Reduce skill cooldown by " + itemEffect.valueEffect + " %";;
                break;
            case Effect.Capture_Speed:
                textEffect += "Reduce capture cooldown by " + itemEffect.valueEffect + " %";;
                break;
            default:
                break;
        }

        CanvasManager.instance.buffInformationPanel.SetActive(true);
        CanvasManager.instance.buffInformationPanel.transform.position = new Vector3(transform.position.x,transform.position.y-35,transform.position.z);
        CanvasManager.instance.buffInformationPanel.transform.GetChild(0).GetComponent<Text>().text = textEffect;
    }

    public void MouseExit()
    {
        CanvasManager.instance.buffInformationPanel.SetActive(false);
    }
}
