using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIA : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    protected Monster monster;
    protected Transform player;
    protected Animator anim;
    protected bool cannotMove;
    protected bool isDead;
    //Retour a la case depart
    protected Vector3 startPosition;
    private bool hasReachedStartPosition;
    protected bool isReturnStartPosition=true;
    // Limite les monstres a un certain nombre
    protected bool isRemoveInList;
    public static List<MonsterIA> list = new List<MonsterIA>();

    // Comportement seul 
    private float changeDirectionInterval = 0.5f;
    private bool isMovingRight = false;
    private bool lastMoveRight = false;
    private float timer = 0f;

    [SerializeField] private AttackCollider[] colliderAttack;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        monster = GetComponent<MonsterScript>().GetMonster();
        startPosition = transform.position;
    }

    protected void MoveToStartPosition()
    {
        if (!hasReachedStartPosition)
        {
            if (startPosition.x > transform.position.x)
            {
                Walk(1); // Déplacement vers la droite
            }
            else
            {
                Walk(-1); // Déplacement vers la gauche
            }
        }

        // Vérifier si le monstre a atteint sa destination
        float distanceToStart = Mathf.Abs(startPosition.x - transform.position.x);
        if (distanceToStart < 0.1f)
        {
            anim.SetBool("Move",false);
            hasReachedStartPosition = true;
            isReturnStartPosition = true;
        }
        else
        {
            hasReachedStartPosition = false;
        }
    }

    public IEnumerator SetMonsterInPlayerHunt()
    {
        list.Add(this);
        isRemoveInList = true;
        isReturnStartPosition = false;
        yield return new WaitForSeconds(12f);
        list.Remove(this);
        isRemoveInList = false;
    }

    public void DeadMonster()
    {
        cannotMove = true;
        isDead = true;
        if(list.Contains(this))
        {
            list.Remove(this);
        }
    }

    protected void MoveTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
            Walk(1); // Déplacement vers la droite
        else
            Walk(-1); // Déplacement vers la gauche
    }

    protected void Walk(int direction,float dividedSpeed = 1)
    {
        if (cannotMove) return;

        transform.Translate(monster.speed *dividedSpeed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Euler(0, direction == 1 ? 0 : 180, 0);
        RectTransform keyCodeTransform = canvas.GetComponent<RectTransform>();
        keyCodeTransform.rotation = Quaternion.Euler(keyCodeTransform.rotation.eulerAngles.x, transform.rotation.y, keyCodeTransform.rotation.eulerAngles.z);
        anim.SetBool("Move", true);
    }

    protected void Move()
    {
        // Déplacer l'objet dans la direction actuelle
        int direction = isMovingRight ? 1 : -1;
        Walk(direction, 0.5f);

        // Mettre à jour le minuteur et changer de direction si nécessaire
        timer += Time.deltaTime;
        if (timer >= changeDirectionInterval)
        {
            // Choix aléatoire pour déterminer la direction opposée à la dernière action
            bool preferLeft = Random.value < 0.75f; // 75% de chance de se déplacer à gauche
            isMovingRight = lastMoveRight ? !preferLeft : preferLeft;
            
            // Mettre à jour la dernière direction de déplacement
            lastMoveRight = isMovingRight;

            timer = 0f;
        }
    }

    protected void StopMoving()
    {
        cannotMove = true;
        anim.SetBool("Move",false);
    }

    // Attack
    protected void Attack(AttackInput attack,int numColliderAttack)
    {
        if(attack.attackAnimName!="")
            anim.SetTrigger(attack.attackAnimName);
            
        int damage = (int)(attack.damage * 0.75f);
        colliderAttack[numColliderAttack].SetAttackCollider(damage,true);
        
        StartCoroutine(ReloadAttack(attack));
        StartCoroutine(WaitToMove());
    }

    protected void ThrowAttack(AttackInput attack)
    {
        if(attack.attackAnimName!="")
            anim.SetTrigger(attack.attackAnimName);

        int damage = (int)(attack.damage * 0.75f);
        float addPos = (transform.rotation.y !=0 ? -0.25f : 0.25f);
        Vector3 position;
        if(attack.typeAttack == TypeOfSkill.Shield)
            position = transform.position + new Vector3(addPos*2.5f, 0, 1);
        else
            position = transform.position + new Vector3(addPos, 0, 1);
        GameObject attackObject = Instantiate(attack.attackPrefs,position,transform.rotation);
        attackObject.GetComponent<AttackObject>().SetAttack(damage,true);

        StartCoroutine(ReloadAttack(attack));
        StartCoroutine(WaitToMove());
    }

    protected IEnumerator ReloadAttack(AttackInput attack)
    {
        attack.isAttack = true;
        yield return new WaitForSeconds(attack.reload);
        attack.isAttack = false;
    }

    protected IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(0.3f);
        StopMoving();
        yield return new WaitForSeconds(1.5f);
        cannotMove = false;
    }
}
