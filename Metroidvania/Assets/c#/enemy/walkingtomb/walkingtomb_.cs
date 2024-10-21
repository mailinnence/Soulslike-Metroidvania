using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingtomb_ : enemy_move
{

    [Header("방향")]
    public int nextMove;
    public bool delay_turn; // 턴에 딜레이가 필요할때
    public bool attacking;

    [Header("공격")]
    int damage; 
    private bool hasDashed;


    [Header("공격 대상 플레이어")]
    public LayerMask attackableLayer1;
    public LayerMask attackableLayer2;
    public LayerMask attackableLayer3;
    private LayerMask combinedAttackableLayers;
    private LayerMask AttackableLayers;


    [Header("플레이어 감지")]
    public Transform playerDetection;      
    public Vector2 playerDetection_;             


    [Header("공격 범위")]
    public Transform attackLeft;      
    public Vector2 attackLeft_;      
    public Transform attackRight;      
    public Vector2 attackRight_;

    [Header("사운드 ")]
    public sound_walkingtomb sound;


    [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부

    
    void Start()
    {        
        // 생존
        alive = true;

        // 데미지 초기화
        damage = 15;

        // 방향 선택 (랜덤)
        nextMove = Random.Range(0, 2) * 2 - 1;

        // nextMove 0이 아니면 즉 정지가 아니고 1이면 1>>ture 를 처리함
        if (nextMove != 0) { spriteRenderer.flipX = nextMove == -1; }
        
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");

        combinedAttackableLayers = attackableLayer1 | attackableLayer2;
        
        AttackableLayers = attackableLayer1 | attackableLayer3;
    }

    
    void Update()
    {
        if(!inventory)
        {
            if (alive)
            { 
                // 감지 공격
                attackPlayer_detection();

                // 공격중에는 방향 전환 제한
                attacking_Limit();
            } // 지형 판정 
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
            if (alive)
            { 
                moveJudgment(); 
            } // 지형 판정 
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }
    



        // 지형 판정 및 이동 방향 결정
    public void moveJudgment()
    {
        // 이동
        rigid.velocity = new Vector2(nextMove , rigid.velocity.y);
        
        // 지형 체크 (낭떨어지)
        float frontVecDistance = 1f;
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f ,rigid.position.y );
        Debug.DrawRay(frontVec, Vector3.down * frontVecDistance, new Color(0, 9, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, frontVecDistance, platformAndObstacleMask);
        if (rayHit.collider == null) 
        { 
            Turn(); 
            delay_turn = true;
            StartCoroutine(playerDetection_turn_delay());
        }      
        else if (rayHit.collider != null && !delay_turn && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            playerDetection_turn();
        }
            

        // 벽면 체크
        float wallVecDistance = 1.2f;
        Vector3 rayDirection = spriteRenderer.flipX ? Vector3.left : Vector3.right;              // 레이 방향 결정
        Vector2 wallVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);     // 시작 위치 설정
        Debug.DrawRay(wallVec, rayDirection * wallVecDistance, new Color(0, 50, 0));             // 레이 그리기
        RaycastHit2D wallRayHit = Physics2D.Raycast(wallVec, rayDirection, wallVecDistance, platformAndObstacleMask); // 충돌 검사
        if (wallRayHit.collider)
        {
            Turn();
            delay_turn = true;
            StartCoroutine(playerDetection_turn_delay());
        }
        else if (wallRayHit.collider != null && !delay_turn && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            playerDetection_turn();
        }
    }


    public void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke(); 
        // Invoke("Think" , 5 );    
    }





    // 플레이어 탐지
    public void playerDetection_turn()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(playerDetection.position, playerDetection_, 0, combinedAttackableLayers);

        foreach (Collider2D obj in objectsToHit)
        {
            if (obj.transform.position.x > transform.position.x + 0.5f)
            {
                spriteRenderer.flipX = false;
                nextMove = 1;
            }
            else if (obj.transform.position.x <= transform.position.x - 0.5f) 
            {
                spriteRenderer.flipX = true;
                nextMove = -1;
            }
        }
    }


    // 낭떨어지 일경우 플레이어를 인식하였어도 다시 반대쪽으로 이동하게 만듬
    IEnumerator playerDetection_turn_delay()
    {
        yield return new WaitForSeconds(1f);
        delay_turn = false;
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
                anim.SetTrigger("attack");
            }
            else 
            {
                anim.ResetTrigger("attack");
            }
        }   
        // 왼쪽
        else if(spriteRenderer.flipX )
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackLeft.position, attackLeft_, 0, combinedAttackableLayers);
           if (objectsToHit.Length >= 1)
            {
                anim.SetTrigger("attack");
            }
            else 
            {
                anim.ResetTrigger("attack");
            }
        }   
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
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 6);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
     
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage ,1);
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
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 6);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
               
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage ,1);
                }
            }
        }
    }







    public void attacking_Limit()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


    
    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetection.position , playerDetection_);
        
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
