using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [SerializeField] Transform eyePivot;
    [SerializeField] LayerMask groundMask;

    [Header("Combet")]
    [SerializeField] Transform combetPivot;         // 탐색 거리.
    [SerializeField] LayerMask combetMask;          // 전투 대상 마스크.
    [SerializeField] float combetRadius;            // 공격 범위.
    [SerializeField] float combetPower;             // 공격력.

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
            // eyePivot위치에서 왼,오르쪽으로 1의 길이만큼 레이를 발사.
            RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, isLeft ? Vector2.left : Vector2.right, 1.0f, groundMask);
            if (hit.collider != null)        // 만약 무언가 충돌했다면
            {
                isLeft = !isLeft;            // 방향을 반대로 돌린다.
            }

            // 낭떨어지 체크.
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
        // 탐지를 위해 Circle범위 만큼 체크.
        Collider2D collider = Physics2D.OverlapCircle(combetPivot.position, combetRadius, combetMask);
        if (collider != null)
        {
            anim.SetTrigger("Attack");                  // Attack 트리거 작동.
            movement.Move(0);                           // 이동 멈추기.
            isAttack = true;                            // 공격 상태.
            Invoke("OnResetAttack", 2.0f);              // n초가 흐른 뒤 초기화.
        }
    }
    private void OnAttack()
    {
        // 이벤트 함수, 애니메이션에서 특정 프레임에 호출.
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
