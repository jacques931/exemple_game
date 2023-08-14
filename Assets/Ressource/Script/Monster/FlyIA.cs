using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyIA : MonsterIA
{
    private FootScript footScript; 
    private Rigidbody2D rb;

    private Coroutine autoFlyCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(WaitToMoveFly());
        footScript = GetComponentInChildren<FootScript>();
        rb = GetComponent<Rigidbody2D>();
        ApplyAutoFly();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }
        else if (!player.GetComponent<PlayerMove>().GetStopMove() && !cannotMove)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            bool isInSearchRange = distanceToPlayer <= monster.searchPlayerDistance;

            if (isInSearchRange && !list.Contains(this))
            {
                if (list.Count < 4 && !isRemoveInList)
                {
                    if (isReturnStartPosition)
                        StartCoroutine(SetMonsterInPlayerHunt());
                }

                if (isReturnStartPosition)
                    Move();
                else
                    MoveToStartPosition();
                ApplyAutoFly();
            }
            else if (list.Contains(this))
            {
                SearchPlayer();
            }
            else
            {
                list.Remove(this);
                if (isReturnStartPosition)
                    Move();
                else
                    MoveToStartPosition();
                ApplyAutoFly();
            }
        }
        else if (!cannotMove)
        {
            if (isReturnStartPosition)
                Move();
            else
                MoveToStartPosition();
            ApplyAutoFly();
        }

    }

    private void SearchPlayer()
    {
        float playerColliderSizeX = player.GetComponent<Collider2D>().bounds.size.x;
        float distanceToPlayerX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceToPlayerY = Mathf.Abs(player.position.y - transform.position.y);
        Fly();

        if(!HaveCloseAttack())
        {
            // On essaie d'abord d'utiliser le sort 1
            if (monster.input.Length > 1 && monster.input[1].canUse && monster.input[1].attackDistance>0 && !monster.input[1].isAttack)
            {
                if (distanceToPlayerX <= monster.input[1].attackDistance && distanceToPlayerY<0.1f)
                {
                    ThrowAttack(monster.input[1]);
                }
            }
            // Sinon, on essaie le sort 0 et on considere que il a toujours la 1er attaque
            else if (!monster.input[0].isAttack && monster.input[0].attackDistance>0 && distanceToPlayerX <= monster.input[0].attackDistance && distanceToPlayerY<0.1f)
            {
                ThrowAttack(monster.input[0]);
            }
            // Si le joueur est trop éloigné, on se déplace vers lui
            else if (distanceToPlayerX - playerColliderSizeX*0.5f> 1f)
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            // On essaie d'abord d'utiliser le sort 1
            if (monster.input.Length > 1 && monster.input[1].canUse && !monster.input[1].isAttack && distanceToPlayerX <= monster.input[1].attackDistance)
            {
                if (monster.input[1].attackPrefs == null)
                    Attack(monster.input[1], 1);
                else if(distanceToPlayerY<0.1f)
                    ThrowAttack(monster.input[1]);
            }
            // Sinon, on essaie le sort 0 et on considere que il a toujours la 1er attaque
            else if (!monster.input[0].isAttack && distanceToPlayerX <= monster.input[0].attackDistance)
            {
                if (monster.input[0].attackPrefs == null)
                    Attack(monster.input[0], 0);
                else if(distanceToPlayerY<0.1f)
                    ThrowAttack(monster.input[0]);
            }
            // Si le joueur est trop éloigné, on se déplace vers lui
            else if (distanceToPlayerX - playerColliderSizeX*0.5f> 0.1f)
            {
                MoveTowardsPlayer();
            }
        }

    }

    private bool HaveCloseAttack()
    {
        foreach(AttackInput attack in monster.input)
        {
            if(attack.attackPrefs == null && attack.canUse)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator AutoFly()
    {
        while(true)
        {
            Transform terrain = GetComponentInParent<TerrainManager>().transform;
            float waitTime=0;
            if(transform.position.y>terrain.position.y)
            {
                waitTime = Random.Range(0.3f,0.5f);
            }
            else
            {
                waitTime = Random.Range(0.1f,0.4f);
            }
            
            yield return new WaitForSeconds(waitTime);
            if(!cannotMove)
            {
                rb.velocity = new Vector2(0, monster.jump);
                anim.SetBool("Move",true);
            }
        }
    }

    private void Fly()
    {
        if(player.position.y>transform.position.y)
        {
            rb.velocity = new Vector2(0, monster.jump);
            anim.SetBool("Move",true);
        }

        StopAutoFly();
    }

    private IEnumerator WaitToMoveFly()
    {
        yield return new WaitForSeconds(0.3f);
        StopMoving();
        yield return new WaitForSeconds(1.5f);
        cannotMove = false;
        transform.position = new Vector3(transform.position.x,transform.position.y+2,transform.position.z);
    }

    private void ApplyAutoFly()
    {
        if (autoFlyCoroutine == null)
        {
            autoFlyCoroutine = StartCoroutine(AutoFly());
        }
    }

    private void StopAutoFly()
    {
        if (autoFlyCoroutine != null)
        {
            StopCoroutine(autoFlyCoroutine);
            autoFlyCoroutine = null;
        }
    }

}
