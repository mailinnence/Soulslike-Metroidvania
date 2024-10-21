using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 현재 코드는 로직이 꼬여서 다시 손봐야함


public class hang : playerStatManager
{
    private float Duration = 0.3f;


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
        hangoff();
        climb();


        if(anim.GetCurrentAnimatorStateInfo(0).IsName("climb") && spriteRenderer.flipX)
        {
            spriteRenderer.flipX=true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("hangup") && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX=false;
        }
    }


    public void hangon(Vector3 position , bool dircetion)
    {
        if (!hangAble)
        {
 
            // 중력과 속도 조절
            rigid.gravityScale = 0;
            rigid.velocity = new Vector2(0, 0); 

            // 위치 선정
            Vector2 offset = new Vector2(0f, 0f);
            if (dircetion) {offset = new Vector2(1f, -1.7f);}
            else {offset = new Vector2(0f, -1.7f); } 

            transform.position = position + (Vector3)offset;
            
            // 애니메이션
            anim.SetBool("hang", true);
        }
    }


    public void hangoff()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && anim.GetBool("hang"))
        {
            hangAble = true;
            rigid.gravityScale = 4f;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.x); 
            anim.SetBool("hang", false);
            StartCoroutine(hangAbleDelay());
        }
    }


    // 잡고 일어서기
    public void climb()
    {
        if (Input.GetKeyDown(KeyCode.S) && anim.GetBool("hang"))
        {
            spriteRenderer.enabled = false;
            anim.SetTrigger("hangup");
            StartCoroutine(climbDelay());
        }
    }




    public void jumpOff()
    {
        anim.SetBool("jump", false);
        anim.SetBool("jump2", false);
        jumpAble = true;
    }


    // 매달려 있다가 놓는 순간에는 hang 이 true 처리되는 것을 방지
    private IEnumerator hangAbleDelay()
    {
        yield return new WaitForSeconds(Duration);
        hangAble = false;
    }


    private IEnumerator climbDelay()
    {
        anim.SetBool("jump", true);
        anim.SetBool("jump2", false);

        yield return null;
        spriteRenderer.enabled = false;
        Vector2 climbOffset;
        if (spriteRenderer.flipX)
        {
            climbOffset = new Vector2(-1f, 2f);
        }
        else
        {
            climbOffset = new Vector2(1f, 2f);
        }

        transform.position += (Vector3)climbOffset;

        yield return new WaitForSeconds(0.01f);
        spriteRenderer.enabled = true;
        
        rigid.gravityScale = 4.7f; // Restore gravity scale
        anim.SetBool("hang", false); // Reset hang state
    }





    // 잡고 있다가 몬스터에게 맞았을때
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽이 왼쪽에 붙어 있을때는 플레이어가 왼쪽으로 바라봐야한다.
        if ((collision.gameObject.layer == LayerMask.NameToLayer("enemy")) && anim.GetBool("hang"))
        {
            StartCoroutine(hangon_attacked());
        }
    }

    private IEnumerator hangon_attacked()
    {
        hangAble = true;
        anim.SetBool("hang", false);
        rigid.gravityScale = 4f;
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.x); 
        yield return new WaitForSeconds(1f);
        hangAble = false;
    }

    

}
