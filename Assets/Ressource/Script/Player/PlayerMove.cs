using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    protected float speed=2;
    protected float jump=3;
    protected bool cannotMove;
    protected bool isClimb;
    protected Animator animator;

    private bool canTeleport;
    private bool breakTeleport;
    private Transform teleportPosition;
    [SerializeField] private Transform canvasPlayer;

    protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetStopMove(bool _cannotMove)
    {
        cannotMove = _cannotMove;
        if(cannotMove)
            GetComponent<Animator>().SetBool("Move",false);

    }

    public bool GetStopMove()
    {
        return cannotMove;
    }

    public void SetAnimationBool(string name)
    {
        animator.SetTrigger(name);
    }

    protected void Update()
    {
        Move(Input.GetKey(KeyCode.RightArrow), Input.GetKey(KeyCode.LeftArrow));
        if(Input.GetKeyDown(KeyCode.T) && canTeleport && !breakTeleport)
        {
            StartCoroutine(TeleportBreak());
        }
        
    }

    private IEnumerator TeleportBreak()
    {
        Vector3 teleportPos = new Vector3(teleportPosition.position.x,teleportPosition.position.y,transform.position.z);
        transform.position = teleportPos;
        breakTeleport = true;
        yield return new WaitForSeconds(0.8f);
        breakTeleport = false;
    }

    public void ChangeTextIcon(string texte, bool active)
    {
        RectTransform keyCodeTransform = canvasPlayer.GetChild(0).GetComponent<RectTransform>();
        keyCodeTransform.rotation = Quaternion.Euler(keyCodeTransform.rotation.eulerAngles.x, transform.rotation.y, keyCodeTransform.rotation.eulerAngles.z);

        GameObject keyCodeObject = canvasPlayer.GetChild(0).gameObject;
        keyCodeObject.SetActive(active);

        Text keyCodeText = keyCodeObject.transform.GetChild(0).GetComponent<Text>();
        keyCodeText.text = texte;
    }


    public void SetcanTeleport(bool _canTeleport,Transform nextTeleport=null)
    {
        canTeleport = _canTeleport;
        teleportPosition = nextTeleport;
        ChangeTextIcon("T",canTeleport);
    }

    private void Move(bool moveRight, bool moveLeft)
    {
        if (cannotMove) return;

        if (moveRight)
        {
            transform.Translate((speed + ItemManagerScene.instance.state[2]) * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.identity;
        }
        else if (moveLeft)
        {
            transform.Translate((speed + ItemManagerScene.instance.state[2]) * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        animator.SetBool("Move", moveRight || moveLeft);
    }

    public void instantiatePlayer(float _speed,float _jump)
    {
        jump = _jump;
        speed = _speed;
    }
}
