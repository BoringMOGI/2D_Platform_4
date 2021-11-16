using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Movement2D movement;
    public Transform weaponPivot;       // 무기의 중심점.
    public float weaponRadius;          // 무기의 범위.
    public float weaponOffset;          // 무기의 거리.
    public LayerMask weaponMask;

    Animator anim;
    bool isLeft;
    bool isAttack;                      // 공격 중인가?

    float comboResetTime = 0.0f;        //  
    int attackCombo = 0;                // 공격 콤보.

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
        float inputX = Input.GetAxisRaw("Horizontal");           // 좌,우측 키보드 입력 (좌:-1, X:0, 우:+1)

        if (inputX != 0)                                         // 키입력을 하고 있을 때.
        {
            // transform.position : 월드 좌표 , transform.localPositon : 로컬 좌표

            isLeft = inputX < 0;                                 // isLeft를 갱신.
            Vector3 pivot = weaponPivot.localPosition;           // 로컬 좌표를 대입한다. 
            pivot.x = Mathf.Abs(pivot.x) * (isLeft ? -1f : 1f);  // 해당 좌표의 x값을 갱신한다. (Mathf.Abs(float) : 절대값)
            weaponPivot.localPosition = pivot;                   // 수정된 좌표를 weaponPivot에 대입.
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
        // 원형 레이를 쏴서 타겟을 감지.
        Collider2D target = Physics2D.OverlapCircle(weaponPivot.position, weaponRadius, weaponMask);

        // 해당 타겟이 존재한다면
        if (target != null)
        {
            // 타겟에게서 EnemyController를 검색.
            EnemyController enemy = target.GetComponent<EnemyController>();

            // 존재한다면...
            if (enemy != null)
            {
                enemy.OnDamaged(transform);             // 피격 당한 Enemy의 OnDamaged함수 호출. 나의 transform을 매게변수로 전달.
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
            // 무기의 범위 그리기.
            Gizmos.DrawWireSphere(weaponPivot.position, weaponRadius);
        }
    }
}
