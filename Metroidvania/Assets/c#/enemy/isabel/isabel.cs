using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isabel : enemy_move
{
    // 회귀 기능 만들어야 함
    // 낭떨어지 방지 기능 만들어야 함




    [Header("감지 상태")]
    public bool start;
    public bool detection_player;
    public bool detection_attack;


    [Header("방향")]
    public bool attacking;
    public Vector3 currentPosition;

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

    [Header("사운드 ")]
    public sound_isabel sound;


    [Header("체력 생존 여부 ")]
    public bool alive; // 생존 여부



    // Start is called before the first frame update
    void Start()
    {

        anim.SetTrigger("start");
        // 생존
        alive = true;

        // 시작 위치
        currentPosition = transform.position;

        // 데미지 초기화
        damage = 30;

        // // 방향 선택 (랜덤)
        // nextMove = Random.Range(0, 2) * 2 - 1;

        // // nextMove 0이 아니면 즉 정지가 아니고 1이면 1>>ture 를 처리함
        // if (nextMove != 0) { spriteRenderer.flipX = nextMove == -1; }
        
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");

        combinedAttackableLayers = attackableLayer1 | attackableLayer2 | attackableLayer3 | attackableLayer4;
        
        AttackableLayers = attackableLayer1 | attackableLayer3;

    }


    void Update()
    {
        if(!inventory && start)
        { 
            if (alive)
            { 
                playerDetection();
                attackPlayer_detection();
                if(detection_player && !detection_attack && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))    // 공격중에는 이동 금지
                { 
                    walking(); 
                    anim.SetBool("walking" , true);
                    anim.SetBool("backwalking" , false);    
                }   
                else 
                {
                    anim.SetBool("walking" , false);
                    anim.SetBool("backwalking" , false);    
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
        if(!inventory && start)
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




    // 이동
    public void walking()
    {
        if (detection_player )
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
        else
        {

            rigid.velocity = new Vector2(0, rigid.velocity.y); // 감지되지 않으면 멈춤
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
        detection_attack = true;
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("attack");
        detection_attack = false;
        yield return new WaitForSeconds(0.5f);
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
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 12);
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
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 12);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
                    // sound.hit_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
                }
            }
        }
    }



    
    // Trigger 감지 후 호출할 함수입니다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체의 태그가 "Player"인 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("start");
        }
    }


    public void start_see()
    {
        spriteRenderer.enabled = true;
    }


    public void start_function_action()
    {
        start = true;   
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





