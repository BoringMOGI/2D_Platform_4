using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] Transform eyePivot;    
    [SerializeField] Movement2D movement;
    [SerializeField] LayerMask groundMask;

    [Header("Item")]
    [SerializeField] Item.ITEMTYPE[] dropTable;     // 드랍하는 아이템의 종류.
    [SerializeField] Transform dropPivot;           // 드랍하는 아이템의 생성 위치.

    bool isStop;
    bool isLeft;

    public int hp;

    // 프로퍼티(Property)
    bool isAlive
    {
        // 값을 참조할 때.
        get
        {
            return anim.GetBool("isAlive");
        }
        // 값을 대입할 때.
        set
        {
            anim.SetBool("isAlive", value); // value : 외부에서 대입한 값.
        }
    }

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    Coroutine damageEffect;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        // 자신에게서 랜더러를 검색 후 대입. (null일 수도 있다.)
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // 만약 isStop이 false면 함수를 종료하라.
        if (isStop || !isAlive)
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

    public void OnDamaged(Transform attacker)
    {
        OnDamaged(attacker, 2.0f, 0.4f);
    }
    public void OnDamaged(Transform attacker, float force, float delay)
    {
        if (!isAlive)
            return;

        // Mathf.Clamp : 최소, 최대 값으로 고정.
        hp = Mathf.Clamp(hp - 1, 0, int.MaxValue);        

        if(hp <= 0)
        {
            isAlive = false;
            anim.SetTrigger("Dead");
        }
        else
        {
            anim.SetTrigger("Damage");

            if (damageEffect != null)                       // 기존에 동일한 코루틴이 실행 중이라면
                StopCoroutine(damageEffect);                // 중지시킨다. (왜? 시간 차 때문에 예상치 못한 결과가 나올 수 있어서)

            damageEffect = StartCoroutine(OnDamagedEffect(attacker, force, delay));       // 코루틴 실행.
        }
    }
    IEnumerator OnDamagedEffect(Transform attacker, float force, float delay)
    {
        isStop = true;

        bool isLeft = transform.position.x <= attacker.position.x;          // 내가 공격자의 왼쪽편이냐 오른쪽 편이냐.
        Vector2 vector = isLeft ? Vector2.left : Vector2.right;             // 내가 날아가야할 방향.

        rigid.AddForce(vector * force, ForceMode2D.Impulse);                   // vector방향으로 2만큼 힘을 가한다.

        yield return new WaitForSeconds(delay);                              // 0.2초 대기.
        isStop = false;
    }

    private void OnDead()
    {
        // Instance : 무언가를 즉석해서 만든다.
        // 무엇을? 어느 위치에? 회전 값은?
        //GameObject item = Instantiate(dropItem, dropPivot.position, Quaternion.identity);
        //Rigidbody2D itemRigid = item.GetComponent<Rigidbody2D>();
        //itemRigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);

        // 아이템 매니저에게 특정 아이템을 요청한다.
        ItemObject newItem = ItemManager.Instance.GetItemObject(dropTable.GetRandom());
        newItem.transform.position = dropPivot.position;

        // Destroy : 무언가를 '삭제'하라.
        // gameObject : 나 자신 (내 오브젝트)
        Destroy(gameObject);
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
