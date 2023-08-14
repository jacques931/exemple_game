using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovePlayer : PlayerMove
{
    private FootScript foot;
    private Rigidbody2D rb;

    private float normalSpeed;

    private void Start()
    {
        base.Start();
        foot = transform.GetChild(0).GetComponent<FootScript>();
        rb = GetComponent<Rigidbody2D>();
        normalSpeed = speed;
        SoundManager.instance.StopSound(16);
    }

    private void Update()
    {
        base.Update();
        if (foot.getIsGround() && Input.GetKeyDown(KeyCode.Space) && !cannotMove)
        {
            rb.velocity = new Vector2(0, jump);
        }
        if(animator.GetBool("Move"))
        {
            SoundManager.instance.Sound(17);
        }
        else
        {
            SoundManager.instance.StopSound(17);
        }
    }


}
