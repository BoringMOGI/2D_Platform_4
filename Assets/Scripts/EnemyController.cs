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
        spriteRenderer = GetComponent<SpriteRenderer>();        // �ڽſ��Լ� �������� �˻� �� ����. (null�� ���� �ִ�.)
    }
    void Update()
    {
        // ���� isStop�� false�� �Լ��� �����϶�.
        if (isStop)
            return;

        movement.Move(isLeft ? -1 : 1, true);

        // eyePivot��ġ���� ��,���������� 1�� ���̸�ŭ ���̸� �߻�.
        RaycastHit2D hit = Physics2D.Raycast(eyePivot.position, isLeft ? Vector2.left : Vector2.right, 1.0f, groundMask);
        if(hit.collider != null)        // ���� ���� �浹�ߴٸ�
        {
            isLeft = !isLeft;           // ������ �ݴ�� ������.
        }

        // �������� üũ.
        hit = Physics2D.Raycast(eyePivot.position + (isLeft ? Vector3.left : Vector3.right) * 1.0f, Vector2.down, 2.0f, groundMask);
        if(hit.collider == null)
        {
            isLeft = !isLeft;
        }
    }

    public void OnDamaged()
    {
        Debug.Log("������ �¾Ҵ�.");
        StartCoroutine(DamageEffect());     // �ڷ�ƾ.
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
