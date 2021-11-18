using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    // �̵�
    // ����
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

        movement.Move(inputX, isEnemy);             // �̵� (or ����)
        anim.SetBool("isWalk", inputX != 0);        // �ִϸ����� �Ķ���� ����.
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
