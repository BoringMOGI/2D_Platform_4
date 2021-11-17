using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Item item;

    // 유니티 이벤트 함수 : 오브젝트가 켜질 때 마다 호출.
    private void OnEnable()
    {
        
    }

    public void Setup(Item item)
    {
        this.item = item;                                   // 매게변수로 아이템 데이터를 받는다.

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = item.itemSprite;            // 랜더러로 데이터에 있는 스프라이트를 그린다.

        // 아이템 생성 시 위 방향으로 3의 힘만큼 뛴다.
        rigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    }
}
