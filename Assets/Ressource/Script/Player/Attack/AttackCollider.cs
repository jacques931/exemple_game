using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private int damage;
    private bool isMonster;

    public void SetAttackCollider(int _damage,bool _isMonster=false)
    {
        damage = _damage;
        isMonster = _isMonster;
        gameObject.SetActive(true);
        StartCoroutine(DeactiveObject());
    }

    private IEnumerator DeactiveObject()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ennemy") && !isMonster)
        {
            col.GetComponent<MonsterScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
        else if(col.gameObject.CompareTag("Player") && isMonster)
        {
            col.GetComponent<PlayerState>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
