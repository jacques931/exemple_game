using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovelayer : PlayerMove
{
    private Rigidbody2D rb;

    private float normalSpeed;

    private void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        normalSpeed = speed;
        SoundManager.instance.StopSound(17);
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.Space) && !cannotMove)
        {
            rb.velocity = new Vector2(0, jump);
            animator.SetBool("Move",true);
        }
        if(animator.GetBool("Move"))
        {
            SoundManager.instance.Sound(16);
        }
        else
        {
            SoundManager.instance.StopSound(16);
        }
    }
}
