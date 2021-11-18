using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Movement2D : MonoBehaviour
{
    [Header("Ground")]
    public Transform groundChecker;         // 땅 체크 기준점.
    public LayerMask groundMask;            // 땅 체크 마스크.
    public float groundRadius;              // 땅 체크 반지름.

    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;

    bool isGround;                     // 내가 땅 위에 서 있는지 여부.
    int jumpCount;                     // 연속 점프 가능 횟수.

    public bool IsGround => isGround;

    // 게임 실행 시(혹은 오브젝트 활성화 시) 최초 1회 실행되는 유니티 이벤트 함수.
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // 매 프레임마다 1회씩 실행되는 유니티 함수.
    void Update()
    {
        GroundCheck();
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    private void GroundCheck()
    {
        if (rigid.velocity.y > 0)       // 나의 y축 물리 속도가 0보다 클 경우 (상승 중일 경우) 체크하지 않는다.
        {
            isGround = false;
            return;
        }

        // 특정 지점 특정 반지름의 크기를 가진 원 안에서 충돌 감지.
        Collider2D hit = Physics2D.OverlapCircle(groundChecker.position, groundRadius, groundMask);
        isGround = (hit != null);    // null이면 아무것도 충돌하지 않았다.
        if (isGround)
        {
            jumpCount = 2;
        }
    }

    /// <summary>
    /// 특정 오브젝트를 움직이는 함수.
    /// </summary>
    /// <param name="inputX">왼쪽:-1, 오른쪽:+1</param>
    /// <param name="isFilpReverse">초기 이미지가 왼쪽을 보고있으면 True.</param>
    public void Move(float inputX, bool isFilpReverse = false)
    {
        // Time.deltaTime : 이전 프레임과 현재 프레임간의 시간 차이.
        if (inputX != 0)
        {
            transform.Translate(Vector3.right * inputX * moveSpeed * Time.deltaTime);
            spriteRenderer.flipX = inputX < 0;
            if (isFilpReverse)
                spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        anim.SetBool("isWalk", inputX != 0);                 // animator의 함수 SetBool을 호출해서 isWalk를 제어.
    }    
    public void Jump()
    {
        if(jumpCount > 0)
        {
            jumpCount -= 1;

            // rigid.velocity : 현재 물체의 속력.
            // 그래서 x축의 속력은 그대로 두고 y축의 속력을 0.0으로 초기화.
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);

            // 따라서 내려오는 힘이 없기 때문에 원래의 power만큼 위로 올라간다.
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundChecker != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
        }
    }

    /*
     * float charge;
    private void ChargeJump()
    {
        // GetKeyDown   : 특정 키를 누른 그 순간.
        // GetKeyUp     : 특정 키를 눌렀다가 때는 그 순간.
        // GetKey       : 특정 키를 누르고 있는 동안 계속
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charge = 0.0f;                      // 버튼을 누른 즉시 charge의 값이 0.0으로 초기화.
        }
        if (Input.GetKey(KeyCode.Space))
        {
            charge += Time.deltaTime;           // 버튼을 누르는 동안 charge에 시간 값 더함.            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            charge = Mathf.Clamp(charge, 0.3f, 1.0f);   // Mathf.Clamp(값, 최소값, 최대값) : 최소, 최대 사이 값으로 고정 후 반환.
            rigid.AddForce(Vector2.up * jumpPower * charge, ForceMode2D.Impulse);       // Vector2.up방향으로 charge에 비율 만큼 곱함.
        }
    }      
    */
}
