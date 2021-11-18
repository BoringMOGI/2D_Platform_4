using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInfoUI infoUI;       // �÷��̾� ���� UI.
    [SerializeField] Transform weaponPivot;     // ������ �߽���.
    [SerializeField] float weaponRadius;        // ������ ����.
    [SerializeField] LayerMask weaponMask;      // ���� ����ũ.

    private Movement2D movement;                // �̵� Ŭ����.
    private Stateable stat;                     // ����.
    private Animator anim;                      // �ִϸ�����.
    private Rigidbody2D rigid;

    private bool isLeft;
    private bool isAttack;                      // ���� ���ΰ�?
    private bool isStopControl;                 // ���� ����.

    private float comboResetTime = 0.0f;        // �޺� �ʱ�ȭ �ð�.
    private int attackCombo = 0;                // ���� �޺�.

    private void Start()
    {
        movement = GetComponent<Movement2D>();
        anim = GetComponent<Animator>();
        stat = GetComponent<Stateable>();
        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine(ResetCombo());
    }
    void Update()
    {
        if (isStopControl)
            return;

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


    public void OnDamaged(Transform attacker, float amount)
    {
        stat.hp = Mathf.Clamp(stat.hp - amount, 0, stat.MAX_HP);    // ü�� ���� �� �ּ�, �ִ�ġ ���̰��� ��ȯ.
        infoUI.SetHp(stat.hp, stat.MAX_HP);

        if(stat.IsAlive == false)
        {
            OnDead();
        }
        else
        {
            StartCoroutine(OnDamagedEffect(attacker));
        }
    }
    private IEnumerator OnDamagedEffect(Transform attacker)
    {
        // ���ư����� ���� ���.
        bool isForceLeft = transform.position.x < attacker.position.x;
        Vector2 vector = isForceLeft ? Vector2.left : Vector2.right;
        vector += Vector2.up;

        rigid.AddForce(vector * 4f, ForceMode2D.Impulse);           // �������� �� ����.
        isStopControl = true;                                       // ��Ʈ�ѷ� �ߴ�.

        while(true)
        {
            if (movement.IsGround && rigid.velocity.y <= 0)         // ���� �ӵ��� �����̸鼭 IsGround�� true�� ��� ����.
                break;

            yield return null;
        }

        isStopControl = false;                                      // ��Ʈ�ѷ� �ߴ� ����.
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
    private void OnDead()
    {

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
