using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class attack2 : playerStatManager 
{

    [Header("캐릭터 공격 범위 오브젝트")]
    public Transform sideAttackTransformRight;      // 스탠딩(우) 공격 
    public Transform sideAttackTransformLeft;       // 스탠딩(좌) 공격 
    public Transform sideAttackTransformRightBig;   // 스탠딩(우) 마무리 공격 
    public Transform sideAttackTransformLeftBig;    // 스탠딩(좌) 마무리  공격  
    public Transform UpAttackTransform;             // 스탠딩 상단 공격

    public Transform DownAttackTransformRight;      // 하단 우측 공격
    public Transform DownAttackTransformLeft;       // 하단 좌측 공격

    public Transform stabbingTransform;             // 찌르기  공격

    public Transform PlungueDrill_lv1;         // 점프 하단 내려찍기 공격
    public Transform PlungueDrill_lv2;         // 점프 하단 내려찍기 공격


    [Header("캐릭터 공격 벡터")]
    public Vector2 sideAttackAreaRight;             // 스탠딩(우) 공격 범위 
    public Vector2 sideAttackAreaLeft;              // 스탠딩(좌) 공격 범위
    public Vector2 sideAttackAreaBigRight;          // 스탠딩(우) 마무리 공격 범위
    public Vector2 sideAttackAreaBigLeft;           // 스탠딩(좌) 마무리 공격 범위 
    public Vector2 UpAttackArea;                    // 상단 공격 범위

    public Vector2 DownAttackAreaRight;             // 숙여서 공격 (우)
    public Vector2 DownAttackAreaLeft;              // 숙여서 공격 (좌)

    public Vector2 stabbingTransform_;             // 찌르기  공격

    public Vector2 PlungueDrill_lv1_;         // 점프 하단 내려찍기 공격
    public Vector2 PlungueDrill_lv2_;         // 점프 하단 내려찍기 공격

    public LayerMask attackableLayer; 
    private float timeBetweenAttack , timeSinceAttack;  // 초기화 하지 않으면 0 이다.
    private float damage;                           // 기본 공격 데미지


    [Header("캐릭터 공격 이펙트")]
    public GameObject sideAttackEffect;
    public GameObject sideAttackEffect2;
    public GameObject sideAttackEffect3;
    public GameObject UpAttackEffect;
    public GameObject DownAttackEffect;
    public GameObject jumpAttackEffect1;                      
    public GameObject jumpAttackEffect2;                      
    public GameObject PlungueDrill_effect_lv1;
    public GameObject PlungueDrill_effect_lv2;
    public GameObject PlungueDrill_saw_effect_left;
    public GameObject PlungueDrill_saw_effect_right;
    public GameObject charging_effect;

    [Header("콜라이더 크기 조정")]
    public collider collider;


    [Header("공격 콤보 관련 변수")]
    public int standingCombo;           // 콤모 변수
    public int jumpStandingCombo;           // 콤모 변수
    public float timer;                 // 콤모 초기화 타이머 변수
    public float jumpCombotimer;
    private int downHitSpeed;           // 숙여서 공격하는 공격 속도가 너무 빠름 제어하는 변수

    [Header("공격 피격시 진동 관련 변수")]
    public Camera cameraScript;
    public Camera_Cm Camera_Cm;
    public CameraShake CameraShake;




    [Header("효과음 변수")]
    public effectSound effectSound;

    [Header("피격 효과")]
    public attackEffect attackEffect;
    public get_hit get_hit;


    [HideInInspector]
    private Transform playerTransform;      // 이펙트 위치 변수


    // 레이어 처리 변수
    private int platformAndObstacleMask;

    // 이벤트 오브젝트
    private bool event_F;



    [Header("찌르기 관련 변수")]
    public float slideDuration = 0.7f;     // Duration of the sliding animation
    public float slideSpeed = 30f;
    public move move;

    [Header("잔상 효과")]
    public AfterImage ghost;  




    void Start()
    {
        // 자식 오브젝트 생성
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  
        
        // 타임머 변수
        float timer = 0.0f;
        float jumpCombotimer = 0.0f;


        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
        
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cameraScript = FindObjectOfType<Camera>();
        Camera_Cm = FindObjectOfType<Camera_Cm>();
        CameraShake = FindObjectOfType<CameraShake>();
    }


    
    void Update()
    {
        // 스탠딩 공격 -------------------------------------------------------------------------------
        // 콤보 초기화 
        attackingOff_standing();                 
        attackingOff_jump();

        // 애니메이션 중에는 이동 금지!
        // attackingAnim();

        // 공격 중에 경사로 이동 방지
        attackingLimit();

        // 내려찍기
        jump_vertical_attack_anim_stop();
        


    

    }




    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransformRight.position , sideAttackAreaRight);
        Gizmos.DrawWireCube(sideAttackTransformLeft.position , sideAttackAreaLeft);
        Gizmos.DrawWireCube(UpAttackTransform.position , UpAttackArea);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(PlungueDrill_lv1.position , PlungueDrill_lv1_);
        Gizmos.DrawWireCube(PlungueDrill_lv2.position , PlungueDrill_lv2_);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(DownAttackTransformRight.position , DownAttackAreaRight);
        Gizmos.DrawWireCube(DownAttackTransformLeft.position , DownAttackAreaLeft);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(sideAttackTransformRightBig.position , sideAttackAreaBigRight);
        Gizmos.DrawWireCube(sideAttackTransformLeftBig.position , sideAttackAreaBigLeft);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(stabbingTransform.position , stabbingTransform_);


    }




    // 공격 함수
    public void playerAttackAction()
    {

        // 위에 벽이 있어서 자동으로 숙여야 할떄
        RaycastHit2D autoDownHit = Physics2D.Raycast(rigid.position , Vector3.up, 1.8f ,LayerMask.GetMask("platform"));
        Debug.DrawRay(rigid.position, Vector3.up * 1.8f, new Color(0, 1, 0));



        if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") 
        && !anim.GetBool("jump2") && !isSliding && !acting )
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack >= timeBetweenAttack)
            {
                timeSinceAttack = 0;
            
                // 스탠딩 공격
                if (!Input.GetKey(KeyCode.UpArrow))  
                {   
                    anim.SetTrigger("attacking");
                }

           
                // up 공격
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    anim.SetTrigger("Upattacking");
                }



            }
        }



        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") 
        && !anim.GetBool("jump2") && !isSliding && !acting )
        {

            // 스탠딩 공격
            if (!Input.GetKey(KeyCode.UpArrow))  
            {   
                gameObject.layer = 10;
                anim.SetBool("charging_shot" , false);
                anim.SetTrigger("charging");
            }
        }


        else if ((Input.GetKeyUp(KeyCode.A) || !Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") 
        && !anim.GetBool("jump2") && !isSliding && !acting && anim.GetCurrentAnimatorStateInfo(0).IsName("charging_start"))
        {

            // 스탠딩 공격
            if (!Input.GetKey(KeyCode.UpArrow))  
            {   
                anim.Play("idle", 0, 0.01f);
            }
        }


        else if ((Input.GetKeyUp(KeyCode.A) || !Input.GetKey(KeyCode.A)) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") 
        && !anim.GetBool("jump2") && !isSliding && !acting && anim.GetCurrentAnimatorStateInfo(0).IsName("charging"))
        {

            // 스탠딩 공격
            if (!Input.GetKey(KeyCode.UpArrow) && !hurt)  // 데미지를 받았을떄는 차징이 되어서는 안 된다!
            {   
                anim.SetBool("charging_shot", true);
            }
        }




        // 고개 숙이기
        else if (Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !anim.GetBool("jump2") && !isSliding && !acting)
        {
            gameObject.layer = 10;
            // 콜라이더 변경
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            collider.Down_Coilder_Size();
            anim.SetBool("crouchDown" , true);
            anim.SetBool("crouchUp" , false);

            // 하단 공격 >> downHitSpeed 가 0 일때만 가능하도록 하고 공겨시 1로 두었다가 다시 시간 차를 두고 0으로 돌리어 속도를 조절한다. 
            if (Input.GetKeyDown(KeyCode.A) && downHitSpeed == 0)
            {

                downHitSpeed = 1;
                Invoke("downHitSpeed_init" , 0.25f); 
                anim.SetTrigger("crouchDownAttack");
            }
        }



        // 자동 숙이기 상태 하단 공격
        else if (autoDownHit.collider != null && !anim.GetBool("jump") && !anim.GetBool("jump2") && !isSliding && !acting)
        {
            // 콜라이더 변경
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            collider.Down_Coilder_Size();
            anim.SetBool("crouchDown" , true);
            anim.SetBool("crouchUp" , false);
        
            // 하단 공격 >> downHitSpeed 가 0 일때만 가능하도록 하고 공겨시 1로 두었다가 다시 시간 차를 두고 0으로 돌리어 속도를 조절한다. 
            if (Input.GetKeyDown(KeyCode.A) && downHitSpeed == 0 )
            {
                downHitSpeed = 1;
                Invoke("downHitSpeed_init" , 0.4f); 
                anim.SetTrigger("crouchDownAttack");
            }
        }

        

        



        // 고개 들어올리기
        else if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !anim.GetBool("jump2") && !isSliding && !acting)
        {
            collider.Idle_Coilder_Size();
            anim.SetBool("crouchDown" , false);
            anim.SetBool("crouchUp" , true);
        }





        // 점프 공격
        else if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && (anim.GetBool("jump") || anim.GetBool("jump2")) && !acting)
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack >= timeBetweenAttack)
            {
                timeSinceAttack = 0;
    
                // 정면 공격
                if (!Input.GetKey(KeyCode.UpArrow)) { anim.SetTrigger("jumpFrontAttack"); }
        
                // up 공격
                else if (Input.GetKey(KeyCode.UpArrow)) { anim.SetTrigger("jumpUpAttack"); }

            }
        }

        // 점프 내려찍기
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.DownArrow) && (anim.GetBool("jump") || anim.GetBool("jump2")) && !acting)
        {
            // 중력 변경 변수
            jump_verticalattack = true;

            // 잔상효과
            jump_ghost = true;
 
            // 레이어 변경
            gameObject.layer = 30;

            // 잔상효과
            ghost.ghostDelay = 0.03f;
            ghost.makeGhost = true;


            anim.SetTrigger("jump_vertical_attack");
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2( 0f , 0f);
        }




        // 찌르기
        else if (Input.GetKeyDown(KeyCode.A) && isSliding && !anim.GetBool("jump") && !anim.GetBool("jump2") && !stabbing_)
        {
            StartCoroutine(stabbing());
         
           // effectSound.AMANECIDA_SWORD_ATTACK_function();
       
        }

    }



    // 내려찍는 중 x축 이동 방지
    public void jump_vertical_attack_anim_limit()
    {
        jump_verticalattack = false;    // 중력을 다른 함수와 분리하기 위한 변수
        rigid.gravityScale = 4.7f;
        rigid.velocity = new Vector2( 0f, -100f);
    }



    // 내려찍기 하다가 바닥에 닿으면 애니메이션이 바뀌어야 한다.
    public void jump_vertical_attack_anim_stop()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down, 2f , platformAndObstacleMask);
        Debug.DrawRay(rigid.position, Vector3.down *  2f, new Color(0, 1, 0));
        if (rayHit.collider != null && anim.GetCurrentAnimatorStateInfo(0).IsName("penitent_verticalattack_falling")) 
        {  
            anim.SetTrigger("jump_vertical_finish");
            // cameraScript.ShakeCamera();
            // Camera_Cm.ShakeCamera();
            CameraShake.TriggerShake(7f, 5.5f, 0.16f);

            if (jump_high == 0)
            {
                // cameraScript.ShakeCamera();
                damage = 25f;
                // 내려찍기
                SpriteRenderer slidingSpriteRenderer = PlungueDrill_effect_lv1.GetComponent<SpriteRenderer>();
                slidingSpriteRenderer.flipX = spriteRenderer.flipX;


                Vector3 offset = new Vector3(0f, 2.2f, 0f); 
                Vector3 instantiatePosition = new Vector3(transform.position.x, rayHit.point.y, transform.position.z) + offset;
                Instantiate(PlungueDrill_effect_lv1, instantiatePosition, transform.rotation);

                Hit(PlungueDrill_lv1 , PlungueDrill_lv1_ , "jump_vertical_attack");  
                Hit_trap(PlungueDrill_lv1 , PlungueDrill_lv1_ , "jump_vertical_attack");  
            }

            // 높은 곳에서 공격 시 더 넓은 범위의 공격과 더 넓은 이펙트 발생
            else if (jump_high == 1)
            {
                // cameraScript.ShakeCamer_lv2();
                damage = 40f;
               // 내려찍기
                SpriteRenderer slidingSpriteRenderer = PlungueDrill_effect_lv2.GetComponent<SpriteRenderer>();
                slidingSpriteRenderer.flipX = spriteRenderer.flipX;

                Vector3 offset = new Vector3(0f, 2.4f, 0f); 
                Vector3 instantiatePosition = new Vector3(transform.position.x, rayHit.point.y, transform.position.z) + offset;
                Instantiate(PlungueDrill_effect_lv2, instantiatePosition, transform.rotation);

                Vector3 offsetLeft = new Vector3(2f, -0.2f, 0f); 
                Instantiate(PlungueDrill_saw_effect_left, instantiatePosition + offsetLeft , transform.rotation);

                Vector3 offsetRight = new Vector3(-2f, -0.2f, 0f); 
                Instantiate(PlungueDrill_saw_effect_right, instantiatePosition + offsetRight , transform.rotation);

                Hit(PlungueDrill_lv2 , PlungueDrill_lv2_ , "jump_vertical_attack");  
                Hit_trap(PlungueDrill_lv2 , PlungueDrill_lv2_ , "jump_vertical_attack");  
            }

            
        }
    }




    // 내려찍기 하다가 바닥에 닿으면 잔상효과를 꺼야 한다.
    public void jump_vertical_attack_ghost_stop()
    {
        jump_ghost = false;
        anim.SetBool("jump" , false);
        anim.SetBool("jump2" , false);
        move.jumpCount = 0;
        gameObject.layer = 10;
    }
    

 
    public void stabbing_anim()
    {
        damage = 5f;
        standingCombo = 3;
        if (!spriteRenderer.flipX) { Hit(stabbingTransform , stabbingTransform_ , "standingHit"); }
        else if (spriteRenderer.flipX) { Hit(stabbingTransform , stabbingTransform_ , "standingHit"); }
    }


    public void stabbing_jump_limit_anim()
    {
        isSliding_stabbing = false;
    }


    IEnumerator stabbing()
    {
        isSliding_stabbing = true;
        bool isFacingRight = spriteRenderer.flipX;

        // Debug.Log(isFacingRight);
        anim.SetTrigger("stabbing_attack");

        // 잔상효과
        ghost.ghostDelay = 0.06f;
        ghost.makeGhost = true;

        // 회피이므로 레이어 수정
        gameObject.layer = 30;


        float slideTimer = 0f;
        float initialSlideSpeed = slideSpeed;
        float currentSlideSpeed = initialSlideSpeed;

        while (slideTimer < slideDuration && !anim.GetBool("jump") && !anim.GetBool("jump2"))
        {
            
            // 이동 방향 설정
            float direction = isFacingRight ? 1f : -1f;

            // 현재 속도를 감속 없이 자연스럽게 감소시킴
            float t = slideTimer / slideDuration;
   
            // 이동 속도 및 방향에 따라 Rigidbody2D에 힘을 가함
           // Use Mathf.SmoothStep for smoother sliding speed interpolation
            currentSlideSpeed = Mathf.SmoothStep(initialSlideSpeed, 0f, t);



            // 점프 중에는 슬라이딩 속도를 수정하지 않음
            if (!anim.GetBool("jump") && !anim.GetBool("jump2") && !anim.GetBool("jump_run_landing"))
            {
                // 경사로 -180 ~ 180 사이로 각도 판정 
                RaycastHit2D SlidingRayHit = Physics2D.Raycast(rigid.position , Vector3.down,1, platformAndObstacleMask);
                if (SlidingRayHit.collider != null)
                {

                    Transform hitTransform = SlidingRayHit.collider.transform;

                    // 해당 객체의 z 축 기울기를 출력합니다.
                    float zRotation = hitTransform.eulerAngles.z;

                    // 만약 각도가 180도보다 크면, 보정합니다.
                    if (zRotation > 180f) {zRotation -= 360f;}

                    float SlidingAngle = zRotation;

                    if (spriteRenderer.flipX) { SlidingAngle += 180; }
                


                    // 이동 속도 및 방향에 따라 Rigidbody2D에 힘을 가함
                    rigid.velocity = new Vector2(-direction * currentSlideSpeed, rigid.velocity.y);

                    Vector2 currentVelocity = rigid.velocity;

                    // 각도를 라디안으로 변환합니다.
                    float radians = SlidingAngle * Mathf.Deg2Rad;
                    

                    // 주어진 각도로 새로운 속도를 설정합니다.
                    Vector2 newVelocity = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * currentVelocity.magnitude;

                    // 새로운 속도를 적용합니다.
                    rigid.velocity = newVelocity;
                }
                else
                {
                    float h = Input.GetAxisRaw("Horizontal"); 

                    if (h != 0 ) { anim.SetBool("jump2", true); anim.SetBool("jump", false); }
                    else if (h == 0){ anim.SetBool("jump", true); anim.SetBool("jump2", false); }
                }
            }
     
            
            
 
            // 슬라이딩 시간 증가
            slideTimer += Time.deltaTime;
            
            yield return null;
        }


        // Rigidbody2D에 가해지는 힘을 멈춤
        // 슬라이딩 종료 후 애니메이션으로 전환
 
        

        isSliding = false;
      
   
    }


    

 
  



    // 스탠딩 기본 공격 ------------------------------------------------------------------------------------------------------------------------------
    
    // 콤보.1
    public void standingAttack1()
    {
        damage = 5f;
        if (!spriteRenderer.flipX)
        {
            Hit(sideAttackTransformRight , sideAttackAreaRight , "standingHit");      
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
            GameObject effectInstance = Instantiate(sideAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
        else if (spriteRenderer.flipX)
        {
            Hit(sideAttackTransformLeft , sideAttackAreaLeft , "standingHit");            
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(1.7f, -1.2f, 0f);  
            GameObject effectInstance =  Instantiate(sideAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }    
    }



    // 콤보.2
    public void standingAttack2()
    {
        damage = 10f;
        if (!spriteRenderer.flipX)
        {
            Hit(sideAttackTransformRight , sideAttackAreaRight , "standingHit");    
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(-1.4f, -1.1f, 0f);  
            GameObject effectInstance = Instantiate(sideAttackEffect2, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }

        else if (spriteRenderer.flipX)
        {
            Hit(sideAttackTransformLeft , sideAttackAreaLeft , "standingHit");             
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(1.4f, -1.1f, 0f); 
            GameObject effectInstance =  Instantiate(sideAttackEffect2, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }    
    }


    // 콤보.3
    public void standingAttack3()
    {
        damage = 15f;
        if (!spriteRenderer.flipX)
        {
            Hit(sideAttackTransformRightBig , sideAttackAreaBigRight , "standingHit");      
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(-2.4f, -1.2f, 0f); 
            GameObject effectInstance = Instantiate(sideAttackEffect3, sideAttackTransformRightBig.position+offset, sideAttackTransformRightBig.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
        else if (spriteRenderer.flipX)
        {
            Hit(sideAttackTransformLeftBig , sideAttackAreaBigLeft , "standingHit");            
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(2.4f, -1.2f, 0f); 
            GameObject effectInstance =  Instantiate(sideAttackEffect3, sideAttackTransformLeftBig.position+offset, sideAttackTransformLeftBig.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }    
    }



    // 스탠딩 상단 공격
    public void standingUpattack()
    {
        damage = 10f;
        if (!spriteRenderer.flipX)
        {
            Hit(UpAttackTransform , UpAttackArea , "UpStandingHit");   
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position + offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }

        else if (spriteRenderer.flipX)
        {
            Hit(UpAttackTransform , UpAttackArea , "UpStandingHit");
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position+offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }
    }




    // 하단 공격 
    public void DownAttack()
    {
        damage = 5f;
        if (!spriteRenderer.flipX)
        {
            Hit(DownAttackTransformRight , DownAttackAreaRight , "DownHit");

            // 이펙트 위치
            Vector3 offset = new Vector3(-1.1f, -0.7f, 0f);   
            GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformRight.position + offset, DownAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
            
        else if (spriteRenderer.flipX)
        {
            Hit(DownAttackTransformLeft , DownAttackAreaLeft , "DownHit");

            // 이펙트 위치
            Vector3 offset = new Vector3(1.1f, -0.7f, 0f);   
            GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformLeft.position+offset, DownAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }
    }






    // 점프 중 정면 공격 - 콤보1
    public void jumpAttack1()
    {
        damage = 8f;
        if (!spriteRenderer.flipX)
        {
            Hit(sideAttackTransformRight , sideAttackAreaRight , "jumpFrontHit");      
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(-1.65f, -1.5f, 0f);  
            GameObject effectInstance = Instantiate(jumpAttackEffect1, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
        else if (spriteRenderer.flipX)
        {
            Hit(sideAttackTransformLeft , sideAttackAreaLeft , "jumpFrontHit");            
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(1.65f, -1.5f, 0f);  
            GameObject effectInstance =  Instantiate(jumpAttackEffect1, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }    
    }



    // 점프 중 정면 공격 - 콤보2
    public void jumpAttack2()
    {
        damage = 8f;
        if (!spriteRenderer.flipX)
        {
            Hit(sideAttackTransformRight , sideAttackAreaRight , "jumpFrontHit");      
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(-1.3f, -1.2f, 0f); 
            GameObject effectInstance = Instantiate(jumpAttackEffect2, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }
        else if (spriteRenderer.flipX)
        {
            Hit(sideAttackTransformLeft , sideAttackAreaLeft , "jumpFrontHit");            
            // 칼날 이펙트 위치
            Vector3 offset = new Vector3(1.3f, -1.2f, 0f);  
            GameObject effectInstance =  Instantiate(jumpAttackEffect2, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }    
    }










    // 점프 상단 공격
    public void jumpUpAttack()
    {
        damage = 8f;
        if (!spriteRenderer.flipX)
        {
            Hit(UpAttackTransform , UpAttackArea , "UpStandingHit");   
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position + offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
        }

        else if (spriteRenderer.flipX)
        {
            Hit(UpAttackTransform , UpAttackArea , "UpStandingHit");
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position+offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
        }
    }



    // 차징 샷
    public void standingBigShot()
    {
        damage = 30f;

        SpriteRenderer slidingSpriteRenderer = charging_effect.GetComponent<SpriteRenderer>();
        slidingSpriteRenderer.flipX = spriteRenderer.flipX;

        Vector3 offset = new Vector3(0f, 1.3f, 0f);

        if (!spriteRenderer.flipX) 
        { 
            Instantiate(charging_effect, transform.position + offset , transform.rotation);
            Hit(sideAttackTransformRightBig , sideAttackAreaBigRight , "bigShot");   
        }
        else if (spriteRenderer.flipX) 
        {
            Instantiate(charging_effect, transform.position + offset , transform.rotation);
            Hit(sideAttackTransformLeftBig , sideAttackAreaBigLeft , "bigShot");   
        }

    }





    // 패링 카운터 애니메이션 
    public void parrying_counter_anim()
    {
        damage = 25;
        standingCombo = 3;
        parrying_counter = true;
        
        if (!spriteRenderer.flipX) 
        { 
            Hit(sideAttackTransformRightBig , sideAttackAreaBigRight , "parrying_counter");   
        }
        else if (spriteRenderer.flipX) 
        {
            Hit(sideAttackTransformLeftBig , sideAttackAreaBigLeft , "parrying_counter");   
        }

    }



    // ----------------------------------------------------------------------------------------------------------------------------------








    // 애니메이션 중에는 이동 방향 전환 금지 ----------------------------------------------------------------------------------------------

    public void attackingAnim()
    {

        string[] attackingStates = { "1hit", "2hit", "3hit" ,"Upattack" , "crouchDownAttack"  };
        attacking = System.Array.Exists(attackingStates, state => anim.GetCurrentAnimatorStateInfo(0).IsName(state));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down, 0.5f ,platformAndObstacleMask);
            
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumpFrontAttack") || 
        anim.GetCurrentAnimatorStateInfo(0).IsName("jumpFrontAttack2") || 
        anim.GetCurrentAnimatorStateInfo(0).IsName("jumpUpAttack"))
        {
            if (rayHit.collider != null) 
            { 
                anim.SetTrigger("idle");
            }
        }

    }

    public void attackingLimit()
    {
        // 경사로에서 내려가는 것을 방지하는 기능 : 누르지 않는다면 x축 이동 정지
        if (attacking)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
    }


    // ----------------------------------------------------------------------------------------------------------------------------------







    // 공격 판정 시 --------------------------------------------------------------------------------------------------------------
    
    // 공격 판정 함수
    void Hit(Transform _attackTransform, Vector2 _attackArea , string type)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {

            if(objectsToHit[i].GetComponent<candle>() != null)
            {
                objectsToHit[i].GetComponent<candle>().end();
                event_F = true;
            }



            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                event_F = false;

                // 피격 데미지 처리
                objectsToHit[i].GetComponent<enemy_move>().EnemyHit(damage);

                // 데미지 피격시 애니메이션
                if(standingCombo == 3)  { get_hit.get_hit_2(objectsToHit[i] , spriteRenderer.flipX); }
                get_hit.get_hit_1(objectsToHit[i] , spriteRenderer.flipX);

                if (type == "standingHit")
                {
                    // 피격 콤보
                    if (standingCombo == 1)      {attackEffect.standingHitEffect(1 , objectsToHit[i].GetComponent<Transform>().position);}
                    else if (standingCombo == 2) {attackEffect.standingHitEffect(2 , objectsToHit[i].GetComponent<Transform>().position);}
                    else if(standingCombo == 3)  {attackEffect.standingHitEffect(3 , objectsToHit[i].GetComponent<Transform>().position);}
                }       


                else if (type == "jumpFrontHit")
                {
                    gravity_hit = true;
                    rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                    // 피격 콤보
                    if (jumpStandingCombo == 1)      
                    {
                        
                        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                        attackEffect.standingHitEffect(1 , objectsToHit[i].GetComponent<Transform>().position);
                    }
                    else if (jumpStandingCombo == 2) 
                    {
                        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                        attackEffect.standingHitEffect(2 , objectsToHit[i].GetComponent<Transform>().position);
                    }
            
                }


                else if (type == "DownHit")
                {
                    attackEffect.standingHitEffect(3 , objectsToHit[i].GetComponent<Transform>().position);
                }


                else if(type == "UpStandingHit")
                {
                    attackEffect.UpStandingHitEffect(objectsToHit[i].GetComponent<Transform>().position);    
                }


                else if(type == "jump_vertical_attack")
                {
                    // cameraScript.ShakeCamera();  진동을 세게 줄지 고민 중
                    get_hit.get_hit_2(objectsToHit[i] , spriteRenderer.flipX);
                    attackEffect.standingHitEffect(3 , objectsToHit[i].GetComponent<Transform>().position);
                }

                // 트랩제거
                else if(type == "jump_vertical_attack" && objectsToHit[i].gameObject.CompareTag("enemyGuard"))
                {
                    objectsToHit[i].GetComponent<VerticalPlungeDoor>().VerticalAttack("shake");
                    // objectsToHit[i].GetComponent<VerticalPlungeDoor>().VerticalAttack("destory");
                }         

                
                else if(type == "bigShot")
                {
                    // cameraScript.ShakeCamera();  진동을 세게 줄지 고민 중
                    get_hit.get_hit_2(objectsToHit[i] , spriteRenderer.flipX);
                    attackEffect.standingHitEffect(3 , objectsToHit[i].GetComponent<Transform>().position);
                }

                else if(type == "parrying_counter")
                {
                    // cameraScript.ShakeCamera();  진동을 세게 줄지 고민 중
                    attackEffect.standingHitEffect(3 , objectsToHit[i].GetComponent<Transform>().position);
                }
                
                // Debug.Log(objectsToHit[i].GetComponent<enemy_move>());
                // Debug.Log("Enemy position: " + objectsToHit[i].GetComponent<Transform>().position);



            }
        }


        // (효과음 카메라 쉐이크 중복 방지)
        if (objectsToHit.Length >= 1 && !event_F)
        {
            // 효과음
            comboSound_standing();

            // 카메라 쉐이크
            // cameraScript.ShakeCamera(); // 피격시 지진효과
            // Camera_Cm.ShakeCamera();
            
            // 메이토는 크기가 커서 눈이 너무 아픔 좀 줄여야함
            if(objectsToHit[0].name == "maito")
            {
                CameraShake.TriggerShake(5f, 4.5f, 0.16f);
            }
            else
            {
                CameraShake.TriggerShake(7f, 5.5f, 0.16f);
            }
            
            
            // 스탠딩 공격시 모션 딜레이
            if (type == "standingHit")
            {
                Invoke("waitCombo_standing" , 0.2f); // 둔탁한 느낌을 위해서 약간 기다리게 한다.
            }

            
        }

        // 점프 공격은 적을 맞추지 않아도 콤보가 올라가는 로직이다.
        Invoke("waitCombo_jump" , 0.03f);   // 둔탁한 느낌을 위해서 약간 기다리게 한다.
        
    


    }


    // 트랩을 향한 공격
    void Hit_trap(Transform _attackTransform, Vector2 _attackArea , string type)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);


        Collider2D[] allObjectsToHit_guard = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0);
        List<Collider2D> objectsToHit_guard = new List<Collider2D>();

        foreach (Collider2D collider in allObjectsToHit_guard)
        {
            if (collider.CompareTag("Vertical Plunge Door"))
            {
                if (jump_high == 0)
                {
                    collider.GetComponent<VerticalPlungeDoor>().VerticalAttack("shake");
                }
                else if (jump_high >= 1)
                {
                    collider.GetComponent<VerticalPlungeDoor>().VerticalAttack("destory");
                }
  


            }
        }

    }



    // 콤보별 효과음
    public void comboSound_standing()
    {
        if (standingCombo == 1)
        {
            // attackEffect.standingHitEffect(1);
            effectSound.PENITENT_ENEMY_HIT_1_function();
        }
        else if (standingCombo == 2)
        {
            // attackEffect.standingHitEffect(2);
            effectSound.PENITENT_ENEMY_HIT_2_function();
        }
        else if(standingCombo == 3)
        {
            // attackEffect.standingHitEffect(3);
            if (!parrying_counter) {effectSound.PENITENT_ENEMY_HIT_3_function();}
            else if (parrying_counter) {}

        }
    }


    // 스탠딩 콤보 추가
    public void waitCombo_standing()
    {
        standingCombo +=1 ;
    }


    // 점프 스탠딩 콤보 추가
    public void waitCombo_jump()
    {
        jumpStandingCombo +=1 ;
    }



    public void downHitSpeed_init()
    {
        downHitSpeed = 0;
    }

    // 스탠딩 콤보 초기화
    public void attackingOff_standing()
    {
        anim.SetInteger("attackCounter", standingCombo); // 애니메이터에 스탠딩 콤보 값을 설정합니다.

        if (standingCombo >= 2)         // 스탠딩 콤보가 2 이상일 때
        {
            timer += Time.deltaTime;    // 시간을 누적합니다.
            
            if (timer >= 0.9f)          // 0.8초 이상이 지났을 때      * 너무 짧게하면 빨리 눌러야 되서 조작감이 좋지 않다. (손가락이 아프다.)
            {
                standingCombo = 1;      // 스탠딩 콤보를 1로 설정합니다.
                timer = 0.0f;           // 타이머를 초기화합니다.
            }
        }
        else                            // 스탠딩 콤보가 2 미만일 때
        {
            timer = 0.0f;               // 타이머를 초기화합니다.
        }
    }



    // 점프 정면 콤보 초기화
    public void attackingOff_jump()
    {
        anim.SetInteger("jumpAttackCounter", jumpStandingCombo); // 애니메이터에 스탠딩 콤보 값을 설정합니다.

        if (jumpStandingCombo >= 2)         // 스탠딩 콤보가 2 이상일 때
        {
            jumpCombotimer += Time.deltaTime;    // 시간을 누적합니다.
            
            if (jumpCombotimer >= 0.5f)          // 0.8초 이상이 지났을 때      * 너무 짧게하면 빨리 눌러야 되서 조작감이 좋지 않다. (손가락이 아프다.)
            {
                jumpStandingCombo = 1;      // 스탠딩 콤보를 1로 설정합니다.
                jumpCombotimer = 0.0f;           // 타이머를 초기화합니다.
            }
        }
        else                            // 스탠딩 콤보가 2 미만일 때
        {
            jumpCombotimer = 0.0f;               // 타이머를 초기화합니다.
        }
    }







}





