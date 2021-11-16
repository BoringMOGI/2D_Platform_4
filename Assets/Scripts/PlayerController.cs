using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Movement2D movement;
    public Transform weaponPivot;       // ������ �߽���.
    public float weaponRadius;          // ������ ����.
    public float weaponOffset;          // ������ �Ÿ�.
    public LayerMask weaponMask;

    Animator anim;
    bool isLeft;
    bool isAttack;                      // ���� ���ΰ�?

    float comboResetTime = 0.0f;        //  
    int attackCombo = 0;                // ���� �޺�.

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ResetCombo());
    }
    void Update()
    {
        Movement();
        Attackable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(string.Compare(collision.gameObject.tag, "Item") == 0)
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");           // ��,���� Ű���� �Է� (��:-1, X:0, ��:+1)

        if (inputX != 0)                                         // Ű�Է��� �ϰ� ���� ��.
        {
            // transform.position : ���� ��ǥ , transform.localPositon : ���� ��ǥ

            isLeft = inputX < 0;                                 // isLeft�� ����.
            Vector3 pivot = weaponPivot.localPosition;           // ���� ��ǥ�� �����Ѵ�. 
            pivot.x = Mathf.Abs(pivot.x) * (isLeft ? -1f : 1f);  // �ش� ��ǥ�� x���� �����Ѵ�. (Mathf.Abs(float) : ���밪)
            weaponPivot.localPosition = pivot;                   // ������ ��ǥ�� weaponPivot�� ����.
        }

        movement.Move(inputX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }
    }
    private void Attackable()
    {
        if (attackCombo >= 0 && attackCombo < 3 && Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("Attack");
            attackCombo += 1;
            comboResetTime = 0.0f;
        }

        anim.SetInteger("AttackCombo", attackCombo);
    }

    IEnumerator ResetCombo()
    {
        const float RESET_TIME = 1.0f;        

        while(true)
        {
            if((comboResetTime += Time.deltaTime) >= RESET_TIME)
            {
                attackCombo = 0;
            }
            yield return null;
        }
    }

    private void OnAttackable()
    {
        // ���� ���̸� ���� Ÿ���� ����.
        Collider2D target = Physics2D.OverlapCircle(weaponPivot.position, weaponRadius, weaponMask);

        // �ش� Ÿ���� �����Ѵٸ�
        if (target != null)
        {
            // Ÿ�ٿ��Լ� EnemyController�� �˻�.
            EnemyController enemy = target.GetComponent<EnemyController>();

            // �����Ѵٸ�...
            if (enemy != null)
            {
                enemy.OnDamaged(transform);             // �ǰ� ���� Enemy�� OnDamaged�Լ� ȣ��. ���� transform�� �ŰԺ����� ����.
            }
        }
    }
    private void OnEndAttack()
    {
        isAttack = false;
    }

    // private void OnDrawGizmos()
    private void OnDrawGizmosSelected()
    {
        if (weaponPivot != null)
        {
            // ������ ���� �׸���.
            Gizmos.DrawWireSphere(weaponPivot.position, weaponRadius);
        }
    }
}
