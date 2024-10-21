using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingtomb : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public float hp;
    public bool alive = true;

    
    // 레이어 처리 변수
    public int platformAndObstacleMask;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = 120;

        if (alive)
        {
            // 이동
            Think();
            Invoke("Think" , 5 );
            
        }
        
    
    }


    void Start()
    {
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
    }

    void FixedUpdate()
    {
        
        if (alive)
        {
            // 지형 판정
            moveJudgment();
        }
            
    }


    void Update()
    {
        Hp();
    }


    // 지형 판정 및 이동 방향 결정
    void moveJudgment()
    {

        // 플레이어 인식 레이캐스트
        float playerDistance = 2f;
        Vector3 playerDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;              // 레이 방향 결정
        Vector2 playerVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y +1f);     // 시작 위치 설정
        Debug.DrawRay(playerVec, playerDirection * playerDistance, Color.red);                        // 레이 그리기
        // RaycastHit2D playerRayHit = Physics2D.Raycast(playerVec, playerDirection, playerDistance, LayerMask.GetMask("player")); // 충돌 검사
        
        // if (playerRayHit.collider != null) 
        // {   
        //     // anim.SetTrigger("attack");
        // }   
        // else
        // {   
            // 속도 제어
            rigid.velocity = new Vector2(nextMove , rigid.velocity.y);
            

            // 지형 체크
            float frontVecDistance = 1f;
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f ,rigid.position.y );
            Debug.DrawRay(frontVec, Vector3.down * frontVecDistance, new Color(0, 9, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, frontVecDistance, platformAndObstacleMask);
            if (rayHit.collider == null) { Turn(); }      
            

            // 벽면 체크
            float wallVecDistance = 1.2f;
            Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;              // 레이 방향 결정
            Vector2 wallVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);     // 시작 위치 설정
            Debug.DrawRay(wallVec, rayDirection * wallVecDistance, new Color(0, 50, 0));             // 레이 그리기
            RaycastHit2D wallRayHit = Physics2D.Raycast(wallVec, rayDirection, wallVecDistance, platformAndObstacleMask); // 충돌 검사
            if (wallRayHit.collider) { Turn(); }
        
        // }
 
    }



    // 이동 재귀함수
    void Think()
    {
        
        nextMove = Random.Range(0, 2) * 2 - 1;

        // anim.SetInteger("walkSpeed" , nextMove);
        
        // nextMove 0이 아니면 즉 정지가 아니고 1이면 1>>ture 를 처리함
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;
        }

        // 재귀함수는 맨 아래에 적어주는 것이 좋다.
        float nextThinkTime = Random.Range(10f , 20f );
        Invoke("Think" , nextThinkTime );
        
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke(); 
        Invoke("Think" , 5 );    
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
        }
    }


    // 체력 관련 로직
    void Hp()
    {
        if (hp <= 0)
        {
            alive = false;
            hp =0;
            gameObject.layer = 15;
            anim.SetTrigger("death");
            anim.SetBool("walking" , false);

        }

    }


    void AnimationFinished()
    {
        Destroy(gameObject);
    }


    public void EnemyHit(float _damageDone)
    {
        hp -= _damageDone;
        // 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 빨간색으로 색상 변경
        renderer.color = new Color(1f, 0.12f, 0.06f); // Hex 코드 FF1F10에 해당하는 색상
        StartCoroutine(ResetColorAfterDelay(0.05f));
    }


    // 지정된 지연 후에 색상을 원래대로 되돌리는 코루틴 함수
    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 초기 색상으로 되돌리기
        renderer.color = Color.white;
    }



    public void color(Color newColor)
    {

    }




}


// y 축 힘 안 받게 필요에 따라서 지정해야 한다.