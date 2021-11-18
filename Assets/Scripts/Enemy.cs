using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [SerializeField] Transform eyePivot;
    [SerializeField] LayerMask groundMask;

    [Header("Combet")]
    [SerializeField] Transform combetPivot;         // Ž�� �Ÿ�.
    [SerializeField] LayerMask combetMask;          // ���� ��� ����ũ.
    [SerializeField] float combetRadius;            // ���� ����.
    [SerializeField] float combetPower;             // ���ݷ�.

    bool isLeft = false;

    protected new void Start()
    {
        base.Start();
        isEnemy = true;
    }

    protected override void Movement()
    {
        inputX = 0;

        if (!isStopControl && !isAttack)
        {
            // eyePivot��ġ���� ��,���������� 1�� ���̸�ŭ ���̸� �߻�.
            RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, isLeft ? Vector2.left : Vector2.right, 1.0f, groundMask);
            if (hit.collider != null)        // ���� ���� �浹�ߴٸ�
            {
                isLeft = !isLeft;            // ������ �ݴ�� ������.
            }

            // �������� üũ.
            hit = Physics2D.Raycast(eyePivot.position + (isLeft ? Vector3.left : Vector3.right) * 1.0f, Vector2.down, 2.0f, groundMask);
            if (hit.collider == null)
            {
                isLeft = !isLeft;
            }

            inputX = isLeft ? -1 : 1;
        }
    }
    protected override void Attack()
    {
        // Ž���� ���� Circle���� ��ŭ üũ.
        Collider2D collider = Physics2D.OverlapCircle(combetPivot.position, combetRadius, combetMask);
        if (collider != null)
        {
            anim.SetTrigger("Attack");                  // Attack Ʈ���� �۵�.
            movement.Move(0);                           // �̵� ���߱�.
            isAttack = true;                            // ���� ����.
            Invoke("OnResetAttack", 2.0f);              // n�ʰ� �帥 �� �ʱ�ȭ.
        }
    }
    private void OnAttack()
    {
        // �̺�Ʈ �Լ�, �ִϸ��̼ǿ��� Ư�� �����ӿ� ȣ��.
        Collider2D collider = Physics2D.OverlapCircle(combetPivot.position, combetRadius, combetMask);
        if (collider != null)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            player.OnDamaged(transform, combetPower);
        }
    }
    private void OnResetAttack()
    {
        isAttack = false;
    }


}
