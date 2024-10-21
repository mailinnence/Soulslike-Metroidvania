using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallClimb : playerStatManager
{

    private float Duration = 0.3f;

    // 점프를 초기화 하여야 한다.
    public move move;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        climbOff();
        climbingJump();
    }

    public void climbStart(Vector3 position)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("jumpFrontAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("jumpFrontAttack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("jumpUpAttack"))
        {
        // 중력과 속도 조절
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(0, 0);

        gravity_anim_ = false;

        // 위치 선정
        Vector2 offset = new Vector2(0f, 0f);
        if (spriteRenderer.flipX) { offset = new Vector2(0.7f, 0); }
        else { offset = new Vector2(-0.7f, 0); }

        // 새로운 위치 계산 (position의 x값 + offset의 x값, 현재 위치의 y값)
        Vector3 newPosition = new Vector3(position.x + offset.x, transform.position.y, transform.position.z);

        // 위치 설정
        transform.position = newPosition;

        anim.SetBool("wallclimbing" , true);
        anim.SetBool("wallclimbing_jump" , false);
       
        }
    }

    // 칼 뽑고 내려가기
    public void climbOff()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && anim.GetBool("wallclimbing")  )
        {
            gravity_anim_ = false;

            // 점프 횟수 초기화
            move.jumpCount = 0;
            
            anim.SetBool("wallclimbing" , false);
            anim.SetBool("wallclimbing_jump" , false);
        }
    }



    public void animOff()
    {
        // 중력과 속도 조절
        rigid.gravityScale = 2.2f;
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.x); 
    }



    // 점프하기
    public void climbingJump()
    {
        if(Input.GetKeyDown(KeyCode.S) && anim.GetBool("wallclimbing")  )
        {
            gravity_anim_ = false;
            // 중력과 속도 조절
            rigid.gravityScale = 2.2f;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.x); 
            rigid.AddForce(Vector2.up * 22, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.right * 22, ForceMode2D.Impulse);
            anim.SetBool("wallclimbing_jump" , true);
            anim.SetBool("wallclimbing" , false);
            anim.SetBool("jump2" , true);
        }
    }



}










