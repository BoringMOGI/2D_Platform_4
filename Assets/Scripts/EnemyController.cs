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
    [SerializeField] Item.ITEMTYPE[] dropTable;     // ����ϴ� �������� ����.
    [SerializeField] Transform dropPivot;           // ����ϴ� �������� ���� ��ġ.

    bool isStop;
    bool isLeft;

    public int hp;

    // ������Ƽ(Property)
    bool isAlive
    {
        // ���� ������ ��.
        get
        {
            return anim.GetBool("isAlive");
        }
        // ���� ������ ��.
        set
        {
            anim.SetBool("isAlive", value); // value : �ܺο��� ������ ��.
        }
    }

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    Coroutine damageEffect;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        // �ڽſ��Լ� �������� �˻� �� ����. (null�� ���� �ִ�.)
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // ���� isStop�� false�� �Լ��� �����϶�.
        if (isStop || !isAlive)
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

    public void OnDamaged(Transform attacker)
    {
        OnDamaged(attacker, 2.0f, 0.4f);
    }
    public void OnDamaged(Transform attacker, float force, float delay)
    {
        if (!isAlive)
            return;

        // Mathf.Clamp : �ּ�, �ִ� ������ ����.
        hp = Mathf.Clamp(hp - 1, 0, int.MaxValue);        

        if(hp <= 0)
        {
            isAlive = false;
            anim.SetTrigger("Dead");
        }
        else
        {
            anim.SetTrigger("Damage");

            if (damageEffect != null)                       // ������ ������ �ڷ�ƾ�� ���� ���̶��
                StopCoroutine(damageEffect);                // ������Ų��. (��? �ð� �� ������ ����ġ ���� ����� ���� �� �־)

            damageEffect = StartCoroutine(OnDamagedEffect(attacker, force, delay));       // �ڷ�ƾ ����.
        }
    }
    IEnumerator OnDamagedEffect(Transform attacker, float force, float delay)
    {
        isStop = true;

        bool isLeft = transform.position.x <= attacker.position.x;          // ���� �������� �������̳� ������ ���̳�.
        Vector2 vector = isLeft ? Vector2.left : Vector2.right;             // ���� ���ư����� ����.

        rigid.AddForce(vector * force, ForceMode2D.Impulse);                   // vector�������� 2��ŭ ���� ���Ѵ�.

        yield return new WaitForSeconds(delay);                              // 0.2�� ���.
        isStop = false;
    }

    private void OnDead()
    {
        // Instance : ���𰡸� �Ｎ�ؼ� �����.
        // ������? ��� ��ġ��? ȸ�� ����?
        //GameObject item = Instantiate(dropItem, dropPivot.position, Quaternion.identity);
        //Rigidbody2D itemRigid = item.GetComponent<Rigidbody2D>();
        //itemRigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);

        // ������ �Ŵ������� Ư�� �������� ��û�Ѵ�.
        ItemObject newItem = ItemManager.Instance.GetItemObject(dropTable.GetRandom());
        newItem.transform.position = dropPivot.position;

        // Destroy : ���𰡸� '����'�϶�.
        // gameObject : �� �ڽ� (�� ������Ʈ)
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
