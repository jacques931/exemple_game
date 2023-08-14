using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected Monster playerState;
    protected PlayerMove playerMove;
    [SerializeField] private AttackCollider[] attackCollider;

    private void Update()
    {
        if(!GetComponent<PlayerMove>().GetStopMove())
        {
            for(int i=0;i<playerState.input.Length;i++)
            {
                if(Input.GetKeyDown(playerState.input[i].key) && !playerState.input[i].isAttack && playerState.input[i].canUse)
                {
                    Attack(playerState.input[i],i);
                }
            }
        }
        
    }

    protected void ResetAttack()
    {
        for(int i=0;i<playerState.input.Length;i++)
        {
            playerState.input[i].isAttack = false;
        }
    }

    private void Attack(AttackInput attackInput,int idAttack)
    {
        SoundManager.instance.SetSoundAttack(attackInput.attackSound);
        int damage = (int)(attackInput.damage  * (1 + ItemManagerScene.instance.state[3]/100));
        if(attackInput.attackPrefs !=null)
            ThrowAttack(attackInput,damage);
        else
            attackCollider[idAttack].SetAttackCollider(damage);

        StartCoroutine(AttackReload(attackInput,idAttack));
    }

    private void ThrowAttack(AttackInput attackInput,int damage)
    {
        float addPos = (transform.rotation.y !=0 ? (-0.25f - attackInput.addPosAttack) : (0.25f + attackInput.addPosAttack));
        Vector3 position = transform.position + new Vector3(addPos, attackInput.addPosAttackY, 1);
        GameObject attackObject = Instantiate(attackInput.attackPrefs,position,transform.rotation);
        attackObject.GetComponent<AttackObject>().SetAttack(damage);
    }

    private IEnumerator AttackReload(AttackInput attackInput,int idAttack)
    {
        if (attackInput.attackAnimName != "")
            playerMove.SetAnimationBool(attackInput.attackAnimName);
        attackInput.isAttack = true;

        float totalTime = attackInput.reload * (1 - ItemManagerScene.instance.state[6]/100);
        // Comptage le temps restant proportionnellement entre 0 et 1
        float startTime = Time.time;
        float endTime = startTime + totalTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            float normalizedTime = (timeRemaining / totalTime);

            CanvasManager.instance.WaitTimeSkill(idAttack,normalizedTime);

            yield return null; // Attendre jusqu'à la prochaine frame
        }

        attackInput.isAttack = false;
    }
}
