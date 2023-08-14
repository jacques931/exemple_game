using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCanvas : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillFilled;
    [SerializeField] private Text skillKey;
    [SerializeField] private GameObject skillCadena;

    private AttackInput currentAttack;

    
    public void SetSkill(AttackInput attack)
    {
        currentAttack = attack;
        skillIcon.sprite = attack.iconAttack;
        skillKey.text = attack.key.ToString();
        FilledTimeSkill(0);
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

    public void CanUseSkill(bool canUse=false)
    {
        skillKey.transform.parent.gameObject.SetActive(canUse);
        skillCadena.SetActive(!canUse);
    }

    public void FilledTimeSkill(float timeRemaining)
    {
        skillFilled.fillAmount = timeRemaining;
    }

    public void MouseEnter()
    {
        if (currentAttack != null)
        {
            string texteSkill = GetTextSkill(currentAttack.typeAttack,(int)currentAttack.damage) + '\n' +'\n' +
                                    "Initial reload time is " + currentAttack.reload + " seconds";

            GameObject skillParent = transform.parent.gameObject;

            CanvasManager.instance.tooltip.SetTooltip(transform.position, skillParent, currentAttack.iconAttack, currentAttack.attackName, currentAttack.typeAttack.ToString(), texteSkill);
        }
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

    private string GetTextSkill(TypeOfSkill typeSkill,int value)
    {
        switch (typeSkill)
        {
            case TypeOfSkill.Melee_Attack:
            case TypeOfSkill.Ranged_Attack:
                return "Initial attack inflicts " + value + " points of damage";
            case TypeOfSkill.Area_Attack:
                return "Initial attack inflicts " + value + " points of damage in the zone";
            case TypeOfSkill.Heal:
                return "Recover " + value + " points of life";
            case TypeOfSkill.Shield:
                return "Create an immobile wall";

            default:
                return "";

        }
    }
}
