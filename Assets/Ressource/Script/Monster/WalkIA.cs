using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkIA : MonsterIA
{
    private FootScript footScript; 
    private Rigidbody2D rb;

    private void Start()
    {
        base.Start();
        StartCoroutine(WaitToMove());
        footScript = transform.GetChild(0).GetComponent<FootScript>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(RandomJump());
    }

    private void Update()
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
                //Peut avoir au maximun 4 monstre qui attaque le joueur automatiquement 
                if (list.Count < 4 && !isRemoveInList)
                {
                    if (isReturnStartPosition)
                        StartCoroutine(SetMonsterInPlayerHunt());
                }

                if (isReturnStartPosition)
                    Move();
                else
                    MoveToStartPosition();
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
            }
        }
        else if (!cannotMove)
        {
            if (isReturnStartPosition)
                Move();
            else
                MoveToStartPosition();
        }
    }

    private void SearchPlayer()
    {
        float playerColliderSizeX = player.GetComponent<Collider2D>().bounds.size.x;
        float monsterColliderSizeX = GetComponent<Collider2D>().bounds.size.x;
        float distanceToPlayerX = Mathf.Abs(player.position.x - transform.position.x);

        //print(monsterColliderSizeX);

        // On essaie d'abord d'utiliser le sort 1
        if (monster.input.Length > 1 && monster.input[1].canUse && monster.input[1].attackDistance>0 && !monster.input[1].isAttack && distanceToPlayerX <= monster.input[1].attackDistance)
        {
            if (monster.input[1].attackPrefs == null)
                Attack(monster.input[1], 1);
            else
                ThrowAttack(monster.input[1]);
        }
        // Sinon, on essaie le sort 0 et on considere que il a toujours la 1er attaque
        else if (!monster.input[0].isAttack && distanceToPlayerX <= monster.input[0].attackDistance && monster.input[0].attackDistance>0)
        {
            if (monster.input[0].attackPrefs == null)
                Attack(monster.input[0], 0);
            else
                ThrowAttack(monster.input[0]);
        }
        // Si le joueur est trop éloigné, on se déplace vers lui
        else if (distanceToPlayerX - playerColliderSizeX*0.5f> monsterColliderSizeX*0.4f)
        {
            MoveTowardsPlayer();
            if(footScript.getIsGround())
            {
                Jump();
            }
        }
        else if(footScript.getIsGround())
        {
            Jump();
        }
    }

    private void Jump()
    {
        if(!cannotMove)
        {
            float distanceToPlayerY = Mathf.Abs(player.position.y - transform.position.y);
            if(distanceToPlayerY>0.1f)
            {
                rb.velocity = new Vector2(0, monster.jump);
            }
        }
    }

    private IEnumerator RandomJump()
    {
        while(true)
        {
            float waitTime = Random.Range(2f,5f);
            yield return new WaitForSeconds(waitTime);
            if(!cannotMove)
            {
                rb.velocity = new Vector2(0, monster.jump * 0.7f);
            }
        }
    }

}
