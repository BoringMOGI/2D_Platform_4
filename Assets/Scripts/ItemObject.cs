using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Item item;

    // ����Ƽ �̺�Ʈ �Լ� : ������Ʈ�� ���� �� ���� ȣ��.
    private void OnEnable()
    {
        
    }

    public void Setup(Item item)
    {
        this.item = item;                                   // �ŰԺ����� ������ �����͸� �޴´�.

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = item.itemSprite;            // �������� �����Ϳ� �ִ� ��������Ʈ�� �׸���.

        // ������ ���� �� �� �������� 3�� ����ŭ �ڴ�.
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
