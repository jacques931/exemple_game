using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    [SerializeField] private bool destroyInstanly=true;
    [SerializeField] private float time;
    [SerializeField] private float speed;

    private int damage;
    private bool isMonster;

    private void Start()
    {
        if(time==0 && speed==0 & GetComponent<Collider2D>()==null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().RecoveryLife(damage);
        }
        Destroy(gameObject,time);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void SetAttack(int _damage,bool _isMonster=false)
    {
        damage = _damage;
        isMonster = _isMonster;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ennemy") && !isMonster)
        {
            MonsterScript monster = col.GetComponent<MonsterScript>();
            monster.ApplyDamage(damage);
            if(destroyInstanly)
                Destroy(gameObject);
        }
        else if(col.gameObject.CompareTag("Player") && isMonster)
        {
            PlayerState playerState = col.GetComponent<PlayerState>();
            playerState.ApplyDamage(damage);
            Destroy(gameObject);
        }
        else if(col.gameObject.CompareTag("Spell"))
        {
            Destroy(gameObject);
        }
    }

}
