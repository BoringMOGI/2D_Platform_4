using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    // 이동
    // 점프
    protected Movement2D movement;
    protected Animator anim;
    protected Rigidbody2D rigid;

    protected float inputX = 0;
    protected bool isEnemy = false;
    protected bool isStopControl = false;
    protected bool isAttack = false;

    protected void Start()
    {
        movement = GetComponent<Movement2D>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        OnUpdate();

        movement.Move(inputX, isEnemy);             // 이동 (or 정지)
        anim.SetBool("isWalk", inputX != 0);        // 애니메이터 파라미터 갱신.
    }
    protected void OnUpdate()
    {
        if (isStopControl)
            return;

        Movement();
        Jump();
        Attack();
    }

    protected virtual void Movement()
    {
    }
    protected virtual void Jump()
    {
    }
    protected virtual void Attack()
    {
    }
    private void OnAttackEvent()
    {

    }
}
