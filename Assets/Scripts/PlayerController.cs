using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInfoUI infoUI;       // 플레이어 상태 UI.
    [SerializeField] Transform weaponPivot;     // 무기의 중심점.
    [SerializeField] float weaponRadius;        // 무기의 범위.
    [SerializeField] LayerMask weaponMask;      // 공격 마스크.

    private Movement2D movement;                // 이동 클래스.
    private Stateable stat;                     // 상태.
    private Animator anim;                      // 애니메이터.
    private Rigidbody2D rigid;

    private bool isLeft;
    private bool isAttack;                      // 공격 중인가?
    private bool isStopControl;                 // 조작 중지.

    private float comboResetTime = 0.0f;        // 콤보 초기화 시간.
    private int attackCombo = 0;                // 공격 콤보.

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
        stat.hp = Mathf.Clamp(stat.hp - amount, 0, stat.MAX_HP);    // 체력 감소 후 최소, 최대치 사이값을 반환.
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
        // 날아가야할 방향 계산.
        bool isForceLeft = transform.position.x < attacker.position.x;
        Vector2 vector = isForceLeft ? Vector2.left : Vector2.right;
        vector += Vector2.up;

        rigid.AddForce(vector * 4f, ForceMode2D.Impulse);           // 물리적인 힘 적용.
        isStopControl = true;                                       // 컨트롤러 중단.

        while(true)
        {
            if (movement.IsGround && rigid.velocity.y <= 0)         // 수직 속도가 음수이면서 IsGround가 true일 경우 종료.
                break;

            yield return null;
        }

        isStopControl = false;                                      // 컨트롤러 중단 해제.
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
    private void OnDead()
    {

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
