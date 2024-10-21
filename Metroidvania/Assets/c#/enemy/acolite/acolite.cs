using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acolite : enemy_move
{


    [Header("방향")]
    public int nextMove;
    public bool delay_turn; // 턴에 딜레이가 필요할때


    [Header("감지 상태")]
    public bool detection_player;
    public bool detection_attack;


    [Header("공격")]
    public int damage; 
    public bool attacking;
    public bool get_parrying;


    [Header("공격 대상 플레이어")]
    public LayerMask attackableLayer1;
    public LayerMask attackableLayer2;
    public LayerMask attackableLayer3;
    public LayerMask attackableLayer4;
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
    // public enemy_move enemy_move;


    void Start()
    {

        // 생존
        alive = true;

        // 데미지 초기화
        damage = 25;


        
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");

        combinedAttackableLayers = attackableLayer1 | attackableLayer2 | attackableLayer3 | attackableLayer4;
        
        AttackableLayers = attackableLayer1 | attackableLayer3;
    }


    void Update()
    {        
        if(!inventory)
        {
            nextMove_init();
            moveJudgment();


            if(!alive && anim.GetCurrentAnimatorStateInfo(0).IsName("get_parrying"))
            {
                anim.SetTrigger("death");
            }

            if(alive && anim.GetCurrentAnimatorStateInfo(0).IsName("get_parrying"))
            {
                anim.SetTrigger("parry_death");
            }


            if (alive)
            { 
                if (!detection_player && !get_parrying && !anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) 
                {
                    walking();
                }
                if (attacking && !get_parrying)
                {
                    attack_front();
                }
                if (get_parrying)
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                }
                get_parrying_setting();

            } 
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }

        }

    }

    void FixedUpdate()
    {
        if(!inventory)
        {
            if (alive && !detection_attack)
            { 
                playerDetection();
            } 
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }


    // 방향 초기화
    public void nextMove_init()
    {
        if (!spriteRenderer.flipX) { nextMove = 1; }
        else { nextMove = -1; }
    }



    // 플레이어 탐지
    public void playerDetection()
    {
        Collider2D[] objectsToHitLeft = Physics2D.OverlapBoxAll(playerDetection_left.position, playerDetection_left_, 0, combinedAttackableLayers);

        Collider2D[] objectsToHitRight = Physics2D.OverlapBoxAll(playerDetection_right.position, playerDetection_right_, 0, combinedAttackableLayers);

        // 좌측
        if(objectsToHitLeft.Length >= 1 && objectsToHitRight.Length == 0)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) spriteRenderer.flipX = false;
            detection_player = true;
            detection_attack = true;
            anim.SetTrigger("attack");
        }
 

        // 우측
        if(objectsToHitRight.Length >= 1 && objectsToHitLeft.Length == 0) 
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) spriteRenderer.flipX = true;
            detection_player = true;
            detection_attack = true;
            anim.SetTrigger("attack");
        }



        // 탐지가 되지 않을때
        if (objectsToHitLeft.Length == 0 && objectsToHitRight.Length == 0)
        {
            detection_player = false;
            detection_attack = false;
            anim.ResetTrigger("attack");
        }
    }



    // 걷기
    public void walking()
    {

        float moveSpeed = 1f; // 이동 속도 설정
        Vector2 newVelocity = rigid.velocity;

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




    public void moveJudgment()
    {

        // 지형 체크 (낭떨어지)
        float frontVecDistance = 1f;
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f ,rigid.position.y );
        Debug.DrawRay(frontVec, Vector3.down * frontVecDistance, new Color(0, 9, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, frontVecDistance, platformAndObstacleMask);
        if (rayHit.collider == null && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) 
        { 
            Turn(); 
        }  
        else if (!rayHit.collider && anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        else
        {
             rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }



       // 벽면 체크
        float wallVecDistance = 1f;
        Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;              // 레이 방향 결정
        Vector2 wallVec = new Vector2(rigid.position.x , rigid.position.y);     // 시작 위치 설정
        Debug.DrawRay(wallVec, rayDirection * wallVecDistance, new Color(0, 50, 0));             // 레이 그리기
        RaycastHit2D wallRayHit = Physics2D.Raycast(wallVec, rayDirection, wallVecDistance, platformAndObstacleMask); // 충돌 검사
        if (wallRayHit.collider && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Turn();
        }
    }


    // 방향 바꾸기
    public void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1; 
    }



    // 전진 공격 -------------------------------------------------------------------------
    public void attack_front()
    {
        float moveSpeed = 19f; // 이동 속도 설정
        Vector2 newVelocity = rigid.velocity;

        if (!spriteRenderer.flipX)
        {
            newVelocity.x = moveSpeed;
            newVelocity.y = 0f;
            
        }
        else
        {
            newVelocity.x = -moveSpeed;
            newVelocity.y = 0f;
        }

        rigid.velocity = newVelocity;
    }


    public void attacking_on_anim()
    {
        attacking = true;
    }


    public void attacking_off_anim()
    {
        attacking = false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }
    // ---------------------------------------------------------------------------------------



    // 공격 딜레이 
    public void detection_attack_off_anim()
    {
        detection_attack = false;
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
                // 패링 당했을떄
                if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
                {
                    get_parrying_on();
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "counter");
                }

                // 공격을 맞았을때
                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject) && !get_parrying)
                {
                    // sound.hit_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
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
                    get_parrying_on();
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "counter");
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject) && !get_parrying)
                {
                    // sound.hit_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
                }
            }
        }
    }




    // 패링 당했을때
    public void get_parrying_on()
    {
        anim.SetTrigger("get_parrying");
    }


    // 패링 움직임 제한
    public void get_parrying_setting()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("get_parrying") || anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            attacking = false;
            get_parrying= true;
        }
        else 
        {
            get_parrying = false;
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

}
