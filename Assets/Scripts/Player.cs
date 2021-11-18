using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Charactor
{
    int attackCombo
    {
        get
        {
            return anim.GetInteger("attackCombo");
        }
        set
        {
            anim.SetInteger("attackCombo", value);
        }
    }
    float comboResetTime = 0.0f;

    protected new void Start()
    {
        base.Start();           // ���� Ŭ������ Start�� ȣ��.

        StartCoroutine(ResetCombo());
    }


    protected override void Movement()
    {
        inputX = 0;

        if(!isStopControl)
           inputX = Input.GetAxisRaw("Horizontal");                
    }
    protected override void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();
    }
    protected override void Attack()
    {
        if (!isAttack && attackCombo >= 0 && attackCombo < 3 && Input.GetKeyDown(KeyCode.Z))
        {
            isAttack = true;
            anim.SetTrigger("attack");
            attackCombo += 1;
            comboResetTime = 0.0f;
            Invoke("OnEndAttack", 0.4f);
        }
    }
    private void OnAttackable()         // ���� ���� ó��.
    {
        // ���̸� ���.
    }
    private void OnEndAttack()          // ���� �����ϵ��� ����.
    {
        isAttack = false;
    }
    private IEnumerator ResetCombo()
    {
        const float RESET_TIME = 1.0f;

        while (true)
        {
            if ((comboResetTime += Time.deltaTime) >= RESET_TIME)
            {
                isAttack = false;
                attackCombo = 0;
            }
            yield return null;
        }
    }
}
