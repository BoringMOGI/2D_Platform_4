using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform eyePivot;
    public LayerMask groundMask;
    public Movement2D movement;
    public bool isStop;
    public bool isLeft;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        // 자신에게서 랜더러를 검색 후 대입. (null일 수도 있다.)
    }
    void Update()
    {
        // 만약 isStop이 false면 함수를 종료하라.
        if (isStop)
            return;

        movement.Move(isLeft ? -1 : 1, true);

        // eyePivot위치에서 왼,오르쪽으로 1의 길이만큼 레이를 발사.
        RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, isLeft ? Vector2.left : Vector2.right, 1.0f, groundMask);
        if(hit.collider != null)        // 만약 무언가 충돌했다면
        {
            isLeft = !isLeft;           // 방향을 반대로 돌린다.
        }

        // 낭떨어지 체크.
        hit = Physics2D.Raycast(eyePivot.position + (isLeft ? Vector3.left : Vector3.right) * 1.0f, Vector2.down, 2.0f, groundMask);
        if(hit.collider == null)
        {
            isLeft = !isLeft;
        }
    }

    public void OnDamaged()
    {
        Debug.Log("공격을 맞았다.");
        StartCoroutine(DamageEffect());     // 코루틴.
    }
    IEnumerator DamageEffect()
    {
        for(int i = 0; i<3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.05f);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (eyePivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(eyePivot.position, (isLeft ? Vector2.left : Vector2.right) * 1.0f);
            Gizmos.DrawRay(eyePivot.position + (isLeft ? Vector3.left : Vector3.right) * 1.0f, Vector2.down * 2.0f);
        }
    }
}
