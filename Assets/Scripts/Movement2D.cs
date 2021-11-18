using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Movement2D : MonoBehaviour
{
    [Header("Ground")]
    public Transform groundChecker;         // �� üũ ������.
    public LayerMask groundMask;            // �� üũ ����ũ.
    public float groundRadius;              // �� üũ ������.

    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;

    bool isGround;                     // ���� �� ���� �� �ִ��� ����.
    int jumpCount;                     // ���� ���� ���� Ƚ��.

    public bool IsGround => isGround;

    // ���� ���� ��(Ȥ�� ������Ʈ Ȱ��ȭ ��) ���� 1ȸ ����Ǵ� ����Ƽ �̺�Ʈ �Լ�.
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // �� �����Ӹ��� 1ȸ�� ����Ǵ� ����Ƽ �Լ�.
    void Update()
    {
        GroundCheck();
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    private void GroundCheck()
    {
        if (rigid.velocity.y > 0)       // ���� y�� ���� �ӵ��� 0���� Ŭ ��� (��� ���� ���) üũ���� �ʴ´�.
        {
            isGround = false;
            return;
        }

        // Ư�� ���� Ư�� �������� ũ�⸦ ���� �� �ȿ��� �浹 ����.
        Collider2D hit = Physics2D.OverlapCircle(groundChecker.position, groundRadius, groundMask);
        isGround = (hit != null);    // null�̸� �ƹ��͵� �浹���� �ʾҴ�.
        if (isGround)
        {
            jumpCount = 2;
        }
    }

    /// <summary>
    /// Ư�� ������Ʈ�� �����̴� �Լ�.
    /// </summary>
    /// <param name="inputX">����:-1, ������:+1</param>
    /// <param name="isFilpReverse">�ʱ� �̹����� ������ ���������� True.</param>
    public void Move(float inputX, bool isFilpReverse = false)
    {
        // Time.deltaTime : ���� �����Ӱ� ���� �����Ӱ��� �ð� ����.
        if (inputX != 0)
        {
            transform.Translate(Vector3.right * inputX * moveSpeed * Time.deltaTime);
            spriteRenderer.flipX = inputX < 0;
            if (isFilpReverse)
                spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        anim.SetBool("isWalk", inputX != 0);                 // animator�� �Լ� SetBool�� ȣ���ؼ� isWalk�� ����.
    }    
    public void Jump()
    {
        if(jumpCount > 0)
        {
            jumpCount -= 1;

            // rigid.velocity : ���� ��ü�� �ӷ�.
            // �׷��� x���� �ӷ��� �״�� �ΰ� y���� �ӷ��� 0.0���� �ʱ�ȭ.
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);

            // ���� �������� ���� ���� ������ ������ power��ŭ ���� �ö󰣴�.
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
        // GetKeyDown   : Ư�� Ű�� ���� �� ����.
        // GetKeyUp     : Ư�� Ű�� �����ٰ� ���� �� ����.
        // GetKey       : Ư�� Ű�� ������ �ִ� ���� ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charge = 0.0f;                      // ��ư�� ���� ��� charge�� ���� 0.0���� �ʱ�ȭ.
        }
        if (Input.GetKey(KeyCode.Space))
        {
            charge += Time.deltaTime;           // ��ư�� ������ ���� charge�� �ð� �� ����.            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            charge = Mathf.Clamp(charge, 0.3f, 1.0f);   // Mathf.Clamp(��, �ּҰ�, �ִ밪) : �ּ�, �ִ� ���� ������ ���� �� ��ȯ.
            rigid.AddForce(Vector2.up * jumpPower * charge, ForceMode2D.Impulse);       // Vector2.up�������� charge�� ���� ��ŭ ����.
        }
    }      
    */
}
