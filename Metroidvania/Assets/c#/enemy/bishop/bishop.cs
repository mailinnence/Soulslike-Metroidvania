using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : enemy_move
{    
    // 회귀 기능 만들어야 함

    [Header("방향")]
    public int nextMove;
    public bool delay_turn; // 턴에 딜레이가 필요할때


    [Header("감지 상태")]
    public bool detection_player;
    public bool detection_attack;
    public bool idle_delay;


    [Header("공격")]
    public int damage; 
    public bool attacking;
    public bool get_parrying;


    [Header("공격 대상 플레이어")]
    public LayerMask attackableLayer1;
    public LayerMask attackableLayer2;
    private LayerMask combinedAttackableLayers;
    private LayerMask AttackableLayers;


    [Header("플레이어 감지")]
    public Transform playerDetection_left;  
    public Transform playerDetection_right;        
    public Vector2 playerDetection_left_;               
    public Vector2 playerDetection_right_;


    [Header("공격 범위")]
    public Transform attackLeft;   
    public Transform attackRight;      
    public Vector2 attackLeft_;         
    public Vector2 attackRight_;

   [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부


    void Start()
    {

        // 생존
        alive = true;

        // 데미지 초기화
        damage = 15;


        
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");

        combinedAttackableLayers = attackableLayer1;
        
        AttackableLayers = attackableLayer2;


    }




    void Update()
    {
        if(!inventory)
        {        
            if(alive)
            {
                if (!idle_delay)

                {
                    playerDetection();
                    turn_velocity_limit();
                    attackPlayer_detection();
                }

                
                if (detection_player && !detection_attack && !idle_delay)
                {
                    walking();
                }
                else if(detection_player || !detection_attack || idle_delay)
                {
                    anim.SetBool("walk" , false);
                }

                // 낭떨어지 확인
                if(spriteRenderer.flipX)
                {
                    nextMove = -1;
                }
                else
                {
                    nextMove = 1;
                }

            }
        }
    }


    void FixedUpdate()
    {
        if(!inventory)
        {       
            if(alive)
            {
        
            }

        
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }


    




    // 플레이어 탐지
    public void playerDetection()
    {
        // 플레이어 위치 판단 ----------------------------------------------------------------------------------------------------------------------------------------
        Collider2D[] objectsToHitLeft = Physics2D.OverlapBoxAll(playerDetection_left.position, playerDetection_left_, 0, combinedAttackableLayers);
        Collider2D[] objectsToHitRight = Physics2D.OverlapBoxAll(playerDetection_right.position, playerDetection_right_, 0, combinedAttackableLayers);

        // 좌측
        if(objectsToHitLeft.Length >= 1 && objectsToHitRight.Length == 0)  { detection_player = true;  }

        // 우측
        if(objectsToHitRight.Length >= 1 && objectsToHitLeft.Length == 0) { detection_player = true; }

        // 탐지가 되지 않을때
        if (objectsToHitLeft.Length == 0 && objectsToHitRight.Length == 0)
        {
            detection_player = false;
            detection_attack = false;
        }
    
        // 지형 체크 (낭떨어지) ----------------------------------------------------------------------------------------------------------------------------------------
        float frontVecDistance = 3f;
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f ,rigid.position.y );
        Debug.DrawRay(frontVec, Vector3.down * frontVecDistance, new Color(0, 9, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, frontVecDistance, platformAndObstacleMask);

        if (rayHit.collider == null) 
        { 
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    
        // 벽면 체크
        // float wallVecDistance = 1.5f;
        // Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;              // 레이 방향 결정
        // Vector2 wallVec = new Vector2(rigid.position.x , rigid.position.y);     // 시작 위치 설정
        // Debug.DrawRay(wallVec, rayDirection * wallVecDistance, new Color(0, 50, 0));             // 레이 그리기
        // RaycastHit2D wallRayHit = Physics2D.Raycast(wallVec, rayDirection, wallVecDistance, platformAndObstacleMask); // 충돌 검사


        // 턴 ----------------------------------------------------------------------------------------------------------------------------------------
        // 좌측
        if(objectsToHitLeft.Length >= 1 
        && objectsToHitRight.Length == 0 
        && !spriteRenderer.flipX 
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") 
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("turnLeft"))  
        { anim.SetTrigger("turnLeft"); }

        // 우측
        if(objectsToHitRight.Length >= 1
        && objectsToHitLeft.Length == 0 
        && spriteRenderer.flipX 
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") 
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("turnRight")) 
        { anim.SetTrigger("turnRight"); }
    
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







    // 걷기
    public void walking()
    {
        
        float moveSpeed = 3f; // 이동 속도 설정
        Vector2 newVelocity = rigid.velocity;


        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("turnRight") 
        || !anim.GetCurrentAnimatorStateInfo(0).IsName("turnLeft")
        || !anim.GetCurrentAnimatorStateInfo(0).IsName("idle")
        || !anim.GetCurrentAnimatorStateInfo(0).IsName("attack")
        || !anim.GetCurrentAnimatorStateInfo(0).IsName("get_hit") )
        {

            anim.SetBool("walk" , true);
            if (!spriteRenderer.flipX)
            {
                newVelocity.x = moveSpeed;
                
            }
            else
            {
                newVelocity.x = -moveSpeed;
            }

            rigid.velocity = newVelocity;
 
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("turnRight") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("turnLeft")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("idle")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("attack")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("get_hit"))
        {
            newVelocity = rigid.velocity;
            newVelocity.x = 0f;
            rigid.velocity = newVelocity;
        }
    }






    // 플레이어 공격 탐지
    public void attackPlayer_detection()
    {
        // 오른쪽
        if(!spriteRenderer.flipX )
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackRight.position, attackRight_, 0, combinedAttackableLayers);
            if (objectsToHit.Length >= 1)
            {
                detection_attack = true;
                idle_delay = true;
                Invoke("idle_delay_function", 2f);
                anim.SetTrigger("attack");
            }
            else 
            {
                detection_attack = false;
                anim.ResetTrigger("attack");
            }
        }   
        // 왼쪽
        else if(spriteRenderer.flipX )
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackLeft.position, attackLeft_, 0, combinedAttackableLayers);
           if (objectsToHit.Length >= 1)
            {
                detection_attack = true;
                idle_delay = true;
                Invoke("idle_delay_function", 2f);
                anim.SetTrigger("attack");
            }
            else 
            {
                anim.ResetTrigger("attack");
                detection_attack = false;
            }
        }   
    }




    public void idle_delay_function()
    {
        idle_delay = false;


    }





    public void attackPlayer_anim()
    {
        HashSet<GameObject> attackedObjects = new HashSet<GameObject>();

        // 오른쪽
        if (!spriteRenderer.flipX)
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackRight.position, attackRight_, 0, AttackableLayers);
            foreach (Collider2D collider in objectsToHit)
            {
                // 레이어 비교 코드
                if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
                {
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 10);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
                    // sound.hit_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage , 1);
                }
            }
        }
        // 왼쪽
        else if (spriteRenderer.flipX)
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackLeft.position, attackLeft_, 0, AttackableLayers);
            foreach (Collider2D collider in objectsToHit)
            {
                // 레이어 비교 코드
                if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
                {
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 10);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
                    // sound.hit_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage , 1);
                }
            }
        }
    }




    // 방향 바꾸기
    public void Turn()
    {
        // anim.SetBool("walk" , false);

        if (!spriteRenderer.flipX) { anim.SetTrigger("turnLeft"); spriteRenderer.flipX = true;}
        else { anim.SetTrigger("turnRight"); spriteRenderer.flipX = false; }

        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1; 
    }




    public void turn_velocity_limit()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("turnRight") 
        || anim.GetCurrentAnimatorStateInfo(0).IsName("turnLeft")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("idle")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        
        {
            Vector2 newVelocity = rigid.velocity;
            newVelocity.x = 0f;
            rigid.velocity = newVelocity;
        }
    }










    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetection_left.position , playerDetection_left_);
        Gizmos.DrawWireCube(playerDetection_right.position , playerDetection_right_);

        // 공격 범뮈
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackLeft.position , attackLeft_);
        Gizmos.DrawWireCube(attackRight.position , attackRight_);

    }


    public void Single_object_alive_anim()
    {
        alive = false;
    }

}
