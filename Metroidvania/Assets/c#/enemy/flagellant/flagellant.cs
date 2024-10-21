using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagellant : enemy_move
{


    [Header("감지 상태")]
    public bool detection_player;
    public bool detection_attack;


    [Header("방향")]
    public float moveSpeed;
    public bool ramdom;    
    public Vector3 currentPosition;
    public int nextMove;
    public bool delay_turn; // 턴에 딜레이가 필요할때

    [Header("공격")]
    int damage; 
    private bool hasDashed;


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


    [Header("패링 여부")]
    public bool get_parrying;


    [Header("사운드 ")]
    public enemy_sound enemy_sound;


    [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부



    // Start is called before the first frame update
    void Start()
    {
        // 생존
        alive = true;
        nextMove = 1;

        // 시작 위치
        currentPosition = transform.position;

        // 데미지 초기화
        damage = 10;

        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
        combinedAttackableLayers = attackableLayer1 | attackableLayer2 | attackableLayer3 | attackableLayer4 ;
        AttackableLayers = attackableLayer1 | attackableLayer3;

    }


    void Update()
    {
        if(!inventory)
        {            
            get_parrying_setting();

            if (alive)
            { 
                playerDetection();
                // 플레이어 감지전
                if(!detection_player && !detection_attack && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("get_hit") && !get_parrying) 
                {   
                    anim.SetBool("running" , false);   

                    // 감지전 이동 로직
                    before_detection();             // 이동을 정한다.
                    if (!ramdom) walk_or_stop();    // 속도를 정해주고
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk")) { moveSpeed = 2f; }
                    else if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) { moveSpeed = 0f; }
                }
                else if(detection_player && !get_parrying && !anim.GetCurrentAnimatorStateInfo(0).IsName("get_hit"))
                {
                    moveSpeed = 5f;
                    if(!detection_attack)after_detection();
                    attackPlayer_detection();
                    anim.SetBool("running" , true);  
                    anim.SetBool("walking" , false);
                }
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
        if (alive)
            { 
            
            } 
            else if(!alive)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }



    public void walk_or_stop()
    {
        ramdom = true;                      // 5초마다 정지 행동을 바꾸기 위한 변수
        moveSpeed = Random.Range(0, 2);
        if (moveSpeed == 1) 
        { 
            anim.SetBool("walking" , true);
        }
        else if (moveSpeed == 0) 
        { 
            anim.SetBool("walking" , false);
        }
        StartCoroutine(Delay());
    }

    // Coroutine to handle the attack delay
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
        ramdom = false;
    }



    // 플레이어 탐지
    public void playerDetection()
    {
        Collider2D[] objectsToHitLeft = Physics2D.OverlapBoxAll(playerDetection_left.position, playerDetection_left_, 0, combinedAttackableLayers);

        Collider2D[] objectsToHitRight = Physics2D.OverlapBoxAll(playerDetection_right.position, playerDetection_right_, 0, combinedAttackableLayers);


        if(objectsToHitLeft.Length >= 1 && objectsToHitRight.Length == 0 && !detection_attack)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) spriteRenderer.flipX = true;
            detection_player = true;
        }

        if(objectsToHitRight.Length >= 1 && objectsToHitLeft.Length == 0 && !detection_attack) 
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack")) spriteRenderer.flipX = false;
            detection_player = true;
        }

        if (objectsToHitLeft.Length == 0 && objectsToHitRight.Length == 0)
        {
            detection_player = false;
        }
    }



    // 플레이어를 감지하지 못했을때 걷거나 멈추거나 랜덤성을 줘야 한다.
    public void before_detection()
    {
        // 이동
        Vector2 newVelocity = rigid.velocity;

        if (!spriteRenderer.flipX) { newVelocity.x = moveSpeed; }
        else { newVelocity.x = -moveSpeed; }

        rigid.velocity = newVelocity;


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



    // 플레이어를 감지하였을때 
    // 낭떨어지에서 떨어지지만 않으면 된다.
    public void after_detection()
    {
        // 이동
        Vector2 newVelocity = rigid.velocity;

        if (!spriteRenderer.flipX) { newVelocity.x = moveSpeed; }
        else { newVelocity.x = -moveSpeed; }

        rigid.velocity = newVelocity;


        // 지형 체크 (낭떨어지)
        float frontVecDistance = 1f;
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f ,rigid.position.y );
        Debug.DrawRay(frontVec, Vector3.down * frontVecDistance, new Color(0, 9, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, frontVecDistance, platformAndObstacleMask);
        if (!rayHit.collider )
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }



    // 플레이어 공격 탐지
    public void attackPlayer_detection()
    {
        // 공격중에는 방향 턴 이동 금지
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        else
        {
             rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // 오른쪽
        if(!spriteRenderer.flipX )
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackRight.position, attackRight_, 0, combinedAttackableLayers);
            if (objectsToHit.Length >= 1)
            {

                StartCoroutine(AttackAfterDelay());
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

                StartCoroutine(AttackAfterDelay());
            }
            else 
            {
                detection_attack = false;
                anim.ResetTrigger("attack");
            }
        }   
    }


    // Coroutine to handle the attack delay
    IEnumerator AttackAfterDelay()
    {
        anim.SetBool("running" , false);  
        anim.SetBool("walking" , false);

        detection_attack = true;
        yield return new WaitForSeconds(0.000001f);
        anim.SetTrigger("attack");
        detection_attack = false;
        yield return new WaitForSeconds(0.000001f);
    }





    // 공격 
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
                    get_parrying_on();
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "counter");
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
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

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
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
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("get_parrying"))
        {
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

    public void Single_object_alive_anim()
    {
        alive = false;
    }



}
