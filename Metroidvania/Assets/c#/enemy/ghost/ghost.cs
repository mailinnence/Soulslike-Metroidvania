using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : enemy_move
{

    [Header("방향")]
    public int nextMove;


    [Header("감지 상태")]
    public bool detection_player;
    public bool detection_attack;
    Vector2 position;           // 플레이어 위치


    [Header("공격")]
    public GameObject magic_ball;


    [Header("공격 대상 플레이어")]
    public LayerMask attackableLayer1;



    [Header("플레이어 감지")]
    public Transform playerDetection;          
    public Vector2 playerDetection_;


    [Header("공격 범위")]
    public Transform attack;          
    public Vector2 attack_;


    [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부



    [Header("페이드 인 앤 아웃 ")]
    public float fadeFloat;

    [Header("사운드 ")]
    public AudioClip BELLGHOST_APPEARING;  

    void Start()
    {
        alive = true;


    }


    void Update()
    {
        if(!inventory)
        {
            if(alive)
            {  
                playerDetection_function();
                attackPlayer_detection();

                spriteRenderer.color = new Color(1, 1, 1, fadeFloat); 

                if(detection_player)
                {
                    FadeIn();
                }
                else if(!detection_player)
                {
                    FadeOut();
                }

                if (detection_player && detection_attack) attackTrigger();

                // 이동은 플레이어 감지 , 공격 비감지 , 공격 애니메이션 x 일때만 가능
                if (detection_player && !detection_attack && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) walk();
                

                // 공격 중 이동 및 축 이동 방지
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("get_hit") || !detection_player)
                {
                    rigid.velocity = new Vector2(0f, 0f);
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
                }

                // 공격이 아닐때 이동 가능
                else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                {
                    rigid.constraints =  RigidbodyConstraints2D.FreezeRotation; 
                    rigid.velocity = new Vector2(rigid.velocity.x , rigid.velocity.y);
                }

                // 턴
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                {
                    // 왼쪽으로
                    if(!spriteRenderer.flipX && transform.position.x - position.x > 2.5f)
                    {
                        Turn();
                    }
                    // 오른쪽으로
                    else if(spriteRenderer.flipX && position.x - transform.position.x > 2.5f)
                    {
                        Turn();
                    }
                }
            }
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, 0);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }



   // 플레이어 탐지
    public void playerDetection_function()
    {
        // 플레이어 위치 판단 ----------------------------------------------------------------------------------------------------------------------------------------
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(playerDetection.position, playerDetection_, 0, attackableLayer1);
  
        // 좌측
        if(objectsToHit.Length >= 1)  
        { 
            detection_player = true;  
            position = objectsToHit[0].transform.position;
        }
        else if(objectsToHit.Length == 0)
        {
            detection_player = false;  
        }
    }




    // 방향 바꾸기
    public void Turn()
    {
        // anim.SetBool("walk" , false);

        if (!spriteRenderer.flipX) { anim.SetTrigger("turnLeft"); spriteRenderer.flipX = true;}
        else { anim.SetTrigger("turnRight"); spriteRenderer.flipX = false; }

    }


    // 방향틀기
    public void turnLeft()
    {
        spriteRenderer.flipX = true;
    }



    public void turnRight()
    {
        spriteRenderer.flipX = false;
    }





    // 이동
    public void walk()
    {
        float maxSpeed = 3f;
        float followDistance = 2.5f;
        float forceAmount = 0.5f;

        Vector2 direction = position - (Vector2)transform.position;
        float distance = direction.magnitude;

        if (distance > followDistance)
        {
            direction.Normalize();
            rigid.AddForce(direction * forceAmount, ForceMode2D.Impulse);
        }

        // 최고 속도 제한
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, maxSpeed);

        // 관성 잡기
        if ((spriteRenderer.flipX && rigid.velocity.x > 0) || (!spriteRenderer.flipX && rigid.velocity.x < 0))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        }

        if ((transform.position.y - position.y > followDistance && rigid.velocity.y > 0) ||
            (transform.position.y - position.y < -followDistance && rigid.velocity.y < 0))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        }
    }









    // 플레이어 공격 탐지
    public void attackPlayer_detection()
    {
  
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attack.position, attack_, 0, attackableLayer1);
        if (objectsToHit.Length >= 1)
        {
            detection_attack = true;
        }
        else
        {
            detection_attack = false;
        }
    }


    // 공격 트리거
    public void attackTrigger()
    {
        anim.SetTrigger("attack");
    }




    // 공격 
    public void attackPlayer_anim()
    {
        if (!spriteRenderer.flipX) 
        { 
            Vector3 offset = new Vector3(1f, 0.1f, 0f); 
            Instantiate(magic_ball, transform.position + offset, transform.rotation);
        }
        else 
        { 
            Vector3 offset = new Vector3(-1f, 0.1f, 0f); 
            Instantiate(magic_ball, transform.position + offset, transform.rotation);
        }


    

    }





    // 페이드 아웃
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    // 페이드 인
    public void FadeIn()
    {
        SoundManager.Instance.PlaySound(BELLGHOST_APPEARING);
        StartCoroutine(FadeInRoutine());
    }

    // 페이드 아웃 루틴
    private IEnumerator FadeOutRoutine()
    {
        while (fadeFloat > 0f)
        {
            fadeFloat -= 0.05f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
        fadeFloat = 0f; // fadeFloat이 정확히 0이 되도록 설정합니다.
    }

    // 페이드 인 루틴
    private IEnumerator FadeInRoutine()
    {
        while (fadeFloat < 1f)
        {
            fadeFloat += 0.05f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
        fadeFloat = 1f; // fadeFloat이 정확히 1이 되도록 설정합니다.
    }
    



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetection.position , playerDetection_);
   

        // 공격 범뮈
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attack.position , attack_);

    }

    public void Single_object_alive_anim()
    {
        alive = false;
    }






}





    // // 이동
    // public void walk()
    // {
    //     float maxspeedX = 3f;
    //     // 최고 속도
    //     if(rigid.velocity.x >= maxspeedX)
    //     {
    //         rigid.velocity = new Vector2(maxspeedX, rigid.velocity.y);
    //     }
    //     else if(rigid.velocity.x <= -1 * maxspeedX)
    //     {
    //         rigid.velocity = new Vector2(-1 * maxspeedX , rigid.velocity.y);
    //     }
        
    //     if(rigid.velocity.y >= maxspeedX)
    //     {
    //         rigid.velocity = new Vector2(rigid.velocity.x, maxspeedX);
    //     }
    //     else if(rigid.velocity.y <= -1 * maxspeedX)
    //     {
    //         rigid.velocity = new Vector2(rigid.velocity.x , -1 * maxspeedX );
    //     }


    //     // 관성 잡기
    //     // 우측
    //     if(spriteRenderer.flipX && rigid.velocity.x >0)
    //     {
    //         rigid.velocity = new Vector2(0f , rigid.velocity.y);
    //     }
    //     // 좌측
    //     else if(!spriteRenderer.flipX && rigid.velocity.x < 0)
    //     {
    //         rigid.velocity = new Vector2(0f , rigid.velocity.y);
    //     }
    //     // 위에서 아래로 
    //     if (transform.position.y - position.y > 2.5f && rigid.velocity.y > 0)
    //     {
    //         rigid.velocity = new Vector2(rigid.velocity.x , 0f);
    //     }
    //     // 아래에서 위로 
    //     if (transform.position.y - position.y < 2.5f && rigid.velocity.y < 0)
    //     {
    //         rigid.velocity = new Vector2(rigid.velocity.x , 0f);
    //     }


    //     // 왼쪽으로
    //     if(transform.position.x - position.x > 2.5f)
    //     {
    //         rigid.AddForce(new Vector2(-1 * 0.5f,0) , ForceMode2D.Impulse);
    //     }
    //     // 오른쪽으로
    //     else if(position.x - transform.position.x > 2.5f)
    //     {
    //         rigid.AddForce(new Vector2(1 * 0.5f,0) , ForceMode2D.Impulse);
    //     }
    
    
    //     // 위쪽으로
    //     if(transform.position.y - position.y > 2.5f)
    //     {
    //         rigid.AddForce(new Vector2(0 , -1 * 0.5f) , ForceMode2D.Impulse);
    //     }
    //     // 아래쪽으로
    //     else if(transform.position.y - position.y < 2.5f)
    //     {
    //         rigid.AddForce(new Vector2(0 , 1 * 0.5f) , ForceMode2D.Impulse);
    //     }
    
    // }
