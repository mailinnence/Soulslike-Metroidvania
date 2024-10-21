using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lecture_1 : MonoBehaviour
{
    // [SerializeField] 
    [Header("확인용 변수")]
    public float temp; 
    

    [Header("캐릭터 이동 관련 변수")]
    public float maxSpeed;      // 속력
    public float jumpPower;     // 점프 파워
    public float jumpCounter;     // 점프 파워
    private int jumpCount;          // 현재까지의 점프 횟수
    public int maxJumpCount = 2;    // 최대 점프 횟수
    private bool damaged = false; // 데미지 여부
    public float jumpRayfloat;    // 경사로마다 레이캐스트 값


    [Header("캐릭터 경사로 관련 변수")]
    public float angle;             // 경사로 판단 각도
    public bool isSlope;            // 경사로 유뮤
    public float maxangle;          // 경사로 최대각도 : 조건문 사용시 사용
    public Vector2 perp;            // 경사로 속도를 일정하게 할 벡터
    

    [Header("슬라이딩 관련 변수")]
    public bool slidingExcept = false; // 점프에서 슬라이딩 변환시점에서 슬라이딩 버그를 막기위한 변수
    public bool isSliding = false;      // Flag to check if the character is currently sliding
    public float slideDuration = 0.7f;  // Duration of the sliding animation
    public float slideSpeed = 17f;


    [Header("에어도어 관련 변수")]
    // 슬라이딩의 관련 변수를 상속함
    public bool airdooring = false;
    public float airdoorSpeed;
    public float airdoorDuration;
    public int airdoorCounter = 0;
    public int max_airdoorCounter = 1;
    



    [Header("효과 오브젝트")]
    public GameObject Sliding_effect;
    public GameObject Doublejump_effect;
    public GameObject JumpLandingDust;
    public GameObject AirDoor_Effect;
    
    

    [Header("캐릭터 물리/마법 제어")]
    public GameObject threeBulletsReady_effect;
    public GameObject threeBulletsShot_effect;
    public GameObject pairOfFire_effect;
    public bool acting;
    public bool actingButMove;

    [Header("캐릭터 공격")]
    public Transform sideAttackTransformRight;  // 스탠딩(우) 공격 
    public Transform sideAttackTransformLeft;   // 스탠딩(좌) 공격 
    public Transform sideAttackTransformRightBig;  // 스탠딩(우) 마무리 공격 
    public Transform sideAttackTransformLeftBig;   // 스탠딩(좌) 마무리  공격  

    public Transform UpAttackTransform;         // 상단 공격
    public Transform jumpDownAttackTransform;   // 점프 하단 공격
    public Transform DownAttackTransformRight;  // 하단 우측 공격
    public Transform DownAttackTransformLeft;   // 하단 좌측 공격

    public Vector2 sideAttackArea;              // 스탠딩(좌,우) 공격 범위 
    public Vector2 sideAttackAreaBig;           // 스탠딩(좌,우) 마무리 공격 범위 
    public Vector2 UpAttackArea;                // 상단 공격 범위
    public Vector2 jumpDownAttackArea;          // 점프 하단 공격
    public Vector2 DownAttackArea;

    public LayerMask attackableLayer; 
    bool attack = false;                        // 공격 여부 변수
    float timeBetweenAttack , timeSinceAttack;  // 초기화 하지 않으면 0 이다.
    public float damage = 1;                       // 기본 공격 데미지


    [Header("캐릭터 공격 콤버")]
    public bool hit1;
    public bool hit2;
    public bool hit3;



    [Header("캐릭터 공격 이펙트")]
    public GameObject sideAttackEffect;
    public GameObject sideAttackEffect2;
    public GameObject sideAttackEffect3;
    public GameObject UpAttackEffect;
    public GameObject DownAttackEffect;
    public GameObject jumpmAttackEffect;
    


    [Header("캐릭터 체력 / 마력")]
    public bool alive = true;
    public hp playerHp;
    public mp playerMp;
     


    // 잔상효과
    public afterImage ghost;
    
    


    // 카메라 관련 변수
    public static lecture_1 Instance;
    

    // 이펙트 위치 변수
    private Transform playerTransform;

    // Component
    Rigidbody2D rigid;                  // 물리
    CapsuleCollider2D CapsuleCollider;  // 충돌
    SpriteRenderer spriteRenderer;      // 방향전환
    Animator anim;                      // 애니메이션


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 잔상 효과
        ghost = GetComponent<afterImage>();

        // 카메라 인스텐스
        if(Instance != null && Instance != this){Destroy(gameObject);}
        else{Instance = this;}




        // 콤보
        hit1 = true;
        hit2 = false;
        hit3 = false;
    }


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  
    }



    void Update()
    {
        // Debug.Log(slidingExcept);

        // 행동과 생존되어 있을때만 작동
        if (!acting && alive && !actingButMove && !damaged)
        {
            InputBasedCharacterMovement();
            Jump();
            Sliding();
            // Debug.Log(rigid.velocity.x);
            // Debug.Log("현재 중력 값: " + rigid.gravityScale);
            PreventColliderPass();
            playerAction();
 
        }

        // 데미지를 받았을때 뒤로 밀려야 하기 때문에 damaged 변수를 이용해서 구현
        else if (!acting && alive && !actingButMove && damaged) 
        {
            InputBasedCharacterMovement();
 
        }   

        // 물리 , 마법을 사용할때는 이동과 애니메이션을 정지시킴
        else if (acting && !actingButMove)
        {
            anim.SetBool("walk" , false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }


        // 액션 중 이동은 가능해야 할떄 (점프 공격)
        if (actingButMove)
        {
            InputBasedCharacterMovement();

            Sliding();
            // Debug.Log(rigid.velocity.x);
            // Debug.Log("현재 중력 값: " + rigid.gravityScale);
            PreventColliderPass();
        }
        




        // 캐릭터 사망
        if (playerHp.curHp <= 0 && alive)
        {
            alive = false;
            gameObject.layer = 15;
            acting = true;
            anim.SetTrigger("death");
           
        }
        if (alive == false && gameObject.layer != 15)
        {
            spriteRenderer.sortingOrder = 30;
            gameObject.layer = 15;
        }
        


        // 잔상종류
        if (!isSliding || anim.GetBool("jump") && !airdooring)
        {
            ghost.makeGhost = false;
        }


        // 버그 방지 레이캐스트
        float exceptFloat = 0.4f;
        RaycastHit2D exceptRayHit = Physics2D.Raycast(rigid.position , Vector3.down, exceptFloat ,LayerMask.GetMask("platform"));
        Vector3 rayDirection = Vector3.down * exceptFloat;
        Debug.DrawRay(rigid.position, rayDirection, Color.green);

        if (exceptRayHit.collider != null) 
            {            
                // Debug.Log(rayHit.distance);
                if (exceptRayHit.distance <= exceptFloat) // or 1.0f
                {
                    slidingExcept = true;
                }
        }
        else 
        {
            slidingExcept = false;
        }




    }




    void FixedUpdate()
    {
        if (!acting)
        {
            ConstantSpeedMovement();
            JumpLandingDetection();
        }        

        // 물리 , 마법을 사용할때는 이동과 애니메이션을 정지시킴
        else if (acting)
        {
            anim.SetBool("walk" , false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
        
    }


 


    // 이동
    void InputBasedCharacterMovement()
    {
        // 경사로에서 중력에 의해 내려가는 것을 방지하는 기능 : 누르지 않는다면 x축 이동 정지
        // 슬라이딩을 하면 앞으로 가야 하기 떄문
        if (!Input.GetButton("Horizontal") && !isSliding && !damaged)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }

        // ConstantSpeedMovement() 에서 가속 방식을 택했다면 무한히 빨라지는 걸 막기 위해서 있어야 한다.
        else if (Input.GetButtonUp("Horizontal") )
        { 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); 
        }
        
        // 누르고 있다면 이동 정지 해제
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // 방향키에 따른 sprite 방향 전환
        // 자연스러운 물리 설정을 위해 슬라이딩 중에는 방향 전환이 불가능해야 한다.
        // 슬라이딩 중 방향이 바뀌면 안된다
        if (Input.GetButton("Horizontal") && !isSliding)  
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            // Debug.Log(spriteRenderer.flipX);
        }  
    
        // 걷기 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        { 
            anim.SetBool("walk" , false);
        }
        else
        {
            anim.SetBool("walk" , true);
        }

    }



    // 이동 속도 관련 설정
    void ConstantSpeedMovement()
    {
        // 방향키를 받아온다.
        float h = Input.GetAxisRaw("Horizontal");
        
        
        if (!isSliding) // 평지에서 슬라이딩할떄 걷는 힘이 더해지면 슬라이딩의 힘이 일정하지 않다 떄문에 슬라이딩시 걷는 힘을 제한해야 한다.
         {
    
            // 가속방식
            // rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            // 일정한 방식
            if (!damaged)
            {rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);}
            
        
        }


        if (rigid.velocity.x > maxSpeed && !isSliding )
        {   // right max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // y축이 0이면 공중에 멈춰있으니 현재 속도를 넣어주어야 한다.
        }
        else if (rigid.velocity.x < maxSpeed * (-1) && !isSliding )
        {   // left max
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // y축이 0이면 공중에 멈춰있으니 현재 속도를 넣어주어야 한다.
        }


        // 경사로 걷기 속도 일정하게
        // 슬라이딩을 하는 동안에는 속도가 더해지면 안되니 조건문을 걸어야 한다.
        // 경사로에서 슬라이딩할떄 걷는 힘이 더해지면 슬라이딩의 힘이 일정하지 않다 떄문에 경사로 슬라이딩시 걷는 힘을 제한해야 한다.
        else if (isSlope && angle > 0 && !isSliding)
        {
            // 경사로를 걷고 있다면
            if (!anim.GetBool("jump"))           
            {
                rigid.velocity = perp * maxSpeed * h * -1;
          
                // Debug.Log(rigid.velocity.y);
            }

            // 점프 중이었다면
            else{
                Vector2 velocityOnSlope = perp * maxSpeed * h * -1;
                rigid.velocity = new Vector2(velocityOnSlope.x, rigid.velocity.y);
            }

            // Debug.Log("현재 속도: " + rigid.velocity);     
        }


    }



    // 점프
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount && !anim.GetBool("crouchDown"))
        {

            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            anim.SetBool("jump", true);
            jumpCount++;

            // 첫 번째 점프가 아닌 경우 점프 애니메이션 상태를 재설정
            if (jumpCount > 1)
            {
                anim.Play("jump", 0, 0f); // 

                // 2단 점프 효과
                Vector3 offset = new Vector3(0f, 2.2f, 0f); 
                Instantiate(Doublejump_effect, transform.position + offset, transform.rotation);

            }

            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }



    // 점프 착지
    void JumpLandingDetection()
    {
        
        Vector2 offsetLeft = new Vector2(-0.27f, 0f); 
        Vector2 offsetRight = new Vector2(0.27f, 0f);

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down,3,LayerMask.GetMask("platform"));
        RaycastHit2D rayHitLeft = Physics2D.Raycast(rigid.position + offsetLeft , Vector3.down,3,LayerMask.GetMask("platform"));
        RaycastHit2D rayHitRight = Physics2D.Raycast(rigid.position + offsetRight , Vector3.down,3,LayerMask.GetMask("platform"));  


        // 레이의 길이 출력
        // Debug.Log("Raycast Distance: " + rayHit.distance);
        if (rigid) {SlopeChk(rayHit);}

        // Landing Platform
        if (rigid.velocity.y < 0) 
        { 
            // 레이캐스트 값
            jumpRayfloat = 1.5f;
            // 경사로에서는 바꿔줘야 한다.
            if (isSlope)
            {
                jumpRayfloat = 0.5f;
            }

            anim.SetBool("jump", true);
            Debug.DrawRay(rigid.position , Vector3.down, new Color(0, 3, 0));
            Debug.DrawRay(rigid.position  + offsetLeft, Vector3.down, new Color(0, 3, 0));
            Debug.DrawRay(rigid.position  + offsetRight, Vector3.down, new Color(0, 3, 0));



            if (rayHit.collider != null) 
            {            
                // Debug.Log(rayHit.distance);
                if (rayHit.distance < jumpRayfloat) // or 1.0f
                {
                    anim.SetBool("jump", false);
                    jumpCount = 0;  // 점프 횟수 초기화
                    airdoorCounter = 0; // 에어도어 횟수 초기화

                    // 점프 후에 땅에 닿았을때 먼지 효과
                    if (rayHit.distance < 0.1f) // or 1.0f
                    {
                        JumpLandingDustEffect();
                    }
                }
            }

            // 추가적인 레이캐스트를 쏴서 허공에 떠있는 현상을 방지
            else if ( rayHitRight.collider != null || rayHitLeft.collider != null && !isSlope) 
            {            
                // Debug.Log(rayHit.distance);
                if (rayHitRight.distance < jumpRayfloat || rayHitLeft.distance < jumpRayfloat) // or 1.0f
                {
                    anim.SetBool("jump", false);
                    jumpCount = 0;  // 점프 횟수 초기화
                    airdoorCounter = 0; // 에어도어 횟수 초기화
                
                }
            }
        }
        else if (rigid.velocity.y == 0 && anim.GetBool("jump"))
        {
            anim.SetBool("jump", false);
            jumpCount = 0;  // 점프 횟수 초기화
            airdoorCounter = 0; // 에어도어 횟수 초기화
          
        }
    }


    void JumpLandingDustEffect()
    {
        Vector3 offset = new Vector3(0f, 0.2f, 0f); 
        Instantiate(JumpLandingDust, transform.position + offset, transform.rotation);
    }
    


    // 경사로 판정
    void SlopeChk(RaycastHit2D rayHit)
    {
        angle = Vector2.Angle(rayHit.normal , Vector2.up);         // 법선 벡터
        perp = Vector2.Perpendicular(rayHit.normal).normalized;    // 법선 벡터와 수직인 벡터

        Debug.DrawLine(rayHit.point , rayHit.point + rayHit.normal , Color.blue);            
        Debug.DrawLine(rayHit.point , rayHit.point + perp , Color.red);
            
        if (angle != 0)
        {
            isSlope = true;
        }
        else
        {
            isSlope = false;
        }
    }

    // 지면 슬라이딩 (회피)
    void Sliding()
    {

        if (Input.GetButtonDown("Dash") && !anim.GetBool("jump") && !isSliding && slidingExcept )
        {
            StartCoroutine(Slide());
        }

        else if (Input.GetButtonDown("Dash") && !anim.GetBool("jump") && !isSliding && isSlope )
        {
            StartCoroutine(Slide());
        }

        // 점프 중이고 공중에서 딱 한번만 에어도어가 가능
        else if (Input.GetButtonDown("Dash") && anim.GetBool("jump") && max_airdoorCounter > airdoorCounter)
        {
            StartCoroutine(AirDoor());
        }

    }




    IEnumerator Slide()
    {

        bool isFacingRight = spriteRenderer.flipX;

        // Debug.Log(isFacingRight);
        isSliding = true;
        anim.SetTrigger("sliding");

        // 잔상효과
        ghost.ghostDelay = 0.05f;
        ghost.makeGhost = true;

        // 회피이므로 레이어 수정
        gameObject.layer = 30;

        // 슬라이딩 콜라이더 크기 조정
        Sliding_Coilder_Size();             

        // 슬라이딩 먼지 효과
        Sliding_dust_effect();

        float slideTimer = 0f;
        float initialSlideSpeed = slideSpeed;
        float currentSlideSpeed = initialSlideSpeed;



        while (slideTimer < slideDuration)
        {
            if (!anim.GetBool("jump")) 
            {
                // 슬라이딩 콜라이더 크기 조정
                Sliding_Coilder_Size();             
            }
           
            // 이동 방향 설정
            float direction = isFacingRight ? 1f : -1f;

            // 현재 속도를 감속 없이 자연스럽게 감소시킴
            float t = slideTimer / slideDuration;
   
            // 이동 속도 및 방향에 따라 Rigidbody2D에 힘을 가함
           // Use Mathf.SmoothStep for smoother sliding speed interpolation
            currentSlideSpeed = Mathf.SmoothStep(initialSlideSpeed, 0f, t);


            // 경사로 -180 ~ 180 사이로 각도 판정하기 위한 레이케스트 
            RaycastHit2D SlidingRayHit = Physics2D.Raycast(rigid.position , Vector3.down,5,LayerMask.GetMask("platform"));
                
                
            // 점프 중에는 슬라이딩 속도를 수정하지 않음
            if (!anim.GetBool("jump") && SlidingRayHit && SlidingRayHit.collider != null)
            {

                // 슬라이딩으로 지면에서 떨어질때 콜라이더를 조절해야한다.
                // 때문에 슬라이딩 상태를 종류한다.
                if (SlidingRayHit.distance > 0.4f ) 
                {
                    // Debug.Log(3333);
                    anim.Play("jump", 0, 1.0f);
                    Idle_Coilder_Size();
                    isSliding = false;
                    yield break; 
                }

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
            
 
            // 슬라이딩 시간 증가
            slideTimer += Time.deltaTime;

            yield return null;
        }


        // Rigidbody2D에 가해지는 힘을 멈춤
        // 슬라이딩 종료 후 애니메이션으로 전환
        if (!anim.GetBool("jump"))
        {
            // 자연스러운 물리 설정을 위해서 점프 중에 그 속도를 유지하도록 하기 위해서 조건문 배치함
            rigid.velocity = Vector2.zero;
            anim.Play("idle", 0, 1.0f);
        }

        isSliding = false;
      
        // 콜라이더 크기 원상복귀
        Idle_Coilder_Size();
        
   
    }



    IEnumerator AirDoor()
    {
        // 잔상효과를 위해서 변수 필요
        airdooring = true;

        // 잔상효과
        ghost.ghostDelay = 0.01f;
        ghost.makeGhost = true;

        gameObject.layer = 30;


        bool isFacingRight = spriteRenderer.flipX;
        float originalGravityScale = rigid.gravityScale;
        rigid.gravityScale = 0f;
        isSliding = true;

        // 에어도어 효과
        Vector3 offset = new Vector3(0f, 0.0f, 0f); 
        Instantiate(AirDoor_Effect, transform.position + offset, transform.rotation);

        // 이동 방향 설정
        float direction = isFacingRight ? -1f : 1f;

        // 이동 속도 및 방향에 따라 Rigidbody2D에 힘을 가함
        rigid.velocity = new Vector2(direction * airdoorSpeed, rigid.velocity.y);

        yield return new WaitForSeconds(airdoorDuration); // 에어도어 지속 시간동안 대기

        isSliding = false;
        rigid.gravityScale = originalGravityScale;
        airdoorCounter++;


        airdooring = false;

    }







    // 슬라이딩 시 콜라이더 크기 변경
    void Sliding_Coilder_Size()
    {
        // offset 조절
        CapsuleCollider.offset = new Vector2(0f, 0.05f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함

        // size 조절
        CapsuleCollider.size = new Vector2(0.18f, 0.3f); // width와 height는 각각 원하는 값으로 대체되어야 함
    }



    // 슬라이딩 시 콜라이더 크기 변경
    void Down_Coilder_Size()
    {
        // offset 조절
        CapsuleCollider.offset = new Vector2(0f, 0.05f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함

        // size 조절
        CapsuleCollider.size = new Vector2(0.18f, 0.3f); // width와 height는 각각 원하는 값으로 대체되어야 함
    }




    // 콜라이더 크기 원상복귀
    void Idle_Coilder_Size()
    {
        // 레이어 원상복귀
        gameObject.layer = 10;
        // offset 조절
        CapsuleCollider.offset = new Vector2(0f, 0.2f); // xOffset와 yOffset는 각각 원하는 값으로 대체되어야 함
        // size 조절
        CapsuleCollider.size = new Vector2(0.18f, 0.6f); // width와 height는 각각 원하는 값으로 대체되어야 함
    }



    // 슬라이딩 시 먼지 효과
    void Sliding_dust_effect()
    {
        SpriteRenderer slidingSpriteRenderer = Sliding_effect.GetComponent<SpriteRenderer>();
        slidingSpriteRenderer.flipX = spriteRenderer.flipX;
        if (spriteRenderer.flipX) 
        { 
            // 새로운 게임 오브젝트를 현재 위치에 원하는 만큼 이동한 위치에 생성
            Vector3 offset = new Vector3(-1f, 0f, 0f); 
            Instantiate(Sliding_effect, transform.position + offset, transform.rotation);
        }

        // 우측으로 갈때
        else if (!spriteRenderer.flipX) 
        {
            // 새로운 게임 오브젝트를 현재 위치에 원하는 만큼 이동한 위치에 생성
            Vector3 offset = new Vector3(1f, 0f, 0f); 
            Instantiate(Sliding_effect, transform.position + offset, transform.rotation);
        }    
    }


    // 통과 방지
    void PreventColliderPass()
    {
        RaycastHit2D AirDoorRayHit;

        if (spriteRenderer.flipX) {AirDoorRayHit = Physics2D.Raycast(transform.position, Vector2.left, 15f, LayerMask.GetMask("platform"));}
        else {AirDoorRayHit = Physics2D.Raycast(transform.position, Vector2.right, 15f, LayerMask.GetMask("platform"));}

        if (AirDoorRayHit.collider != null)
        {
            // 충돌 지점과 현재 위치 사이에 선 그리기
            Debug.DrawLine(rigid.position, AirDoorRayHit.point, Color.red);
            if (AirDoorRayHit.distance < 1f) 
            { 
                rigid.velocity = new Vector2(0, rigid.velocity.y); 
            }
            else 
            {
                rigid.velocity = new Vector2(rigid.velocity.x , rigid.velocity.y);
            }
        }


        RaycastHit2D UpRayHit;
        UpRayHit = Physics2D.Raycast(transform.position, Vector2.up, 15f, LayerMask.GetMask("platform"));

        if (UpRayHit.collider != null)
        {
            // 충돌 지점과 현재 위치 사이에 선 그리기
            Debug.DrawLine(rigid.position, UpRayHit.point, Color.red);
            if (UpRayHit.distance < 0.1f) { 

                rigid.velocity = new Vector2(rigid.velocity.x , 0); 
            }
            else 
            {
                rigid.velocity = new Vector2(rigid.velocity.x , rigid.velocity.y);
            }
        }

    }



    





    void playerAction()
    {
        // 마법 ------------------------------------------------------------------------------------------
     
        // 스킬 - 3개의 총알 
        if (Input.GetKeyDown(KeyCode.Z) && !anim.GetBool("jump") && !isSliding && !acting)
        {
            energe("threeBullets");
            StartCoroutine(threeBullets());
        }

        // else if (Input.GetKeyDown(KeyCode.X) && !anim.GetBool("jump") && !isSliding && !acting)
        // {
        //     StartCoroutine(pairOfFire());
        // }



        // 물리 ------------------------------------------------------------------------------------------
        // 패링
        else if (Input.GetKeyDown(KeyCode.D) && !anim.GetBool("jump") && !isSliding && !acting)
        {
            StartCoroutine(parrying());
        }


        // 스탠딩 공격
        else if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack >= timeBetweenAttack)
            {
                timeSinceAttack = 0;
 
                // 우측 공격
                if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
                { 
                    if (hit1)
                    {
                        anim.SetTrigger("attacking");
                        acting = true;
                    }
                    else if (hit2)
                    {
                        anim.SetTrigger("attacking2");
                        acting = true;
                    }
                    else if (hit3)
                    {
                        anim.SetTrigger("attacking3");
                        acting = true;
                    }


      
                }
                // 좌측 공격
                else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
                { 
                    if (hit1)
                    {
                        anim.SetTrigger("attacking");
                        acting = true;
                    }
                    else if (hit2)
                    {
                        anim.SetTrigger("attacking2");
                        acting = true;
                    }
                    else if (hit3)
                    {
                        anim.SetTrigger("attacking3");
                        acting = true;
                    }
                }

                // up 공격
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    // 우측 공격
                    if (!spriteRenderer.flipX) 
                    { 
                        acting = true;
                        anim.SetTrigger("Upattacking");
                    }
                    // 좌측 공격
                    else if (spriteRenderer.flipX ) 
                    { 
                        acting = true;
                        anim.SetTrigger("Upattacking");
                    }
                }
            }
        }
    
    


        // 점프 공격
        else if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && anim.GetBool("jump")  && !acting)
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack >= timeBetweenAttack)
            {
                timeSinceAttack = 0;
 
                // 우측 공격
                if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
                { 
                    actingButMove = true;
                    anim.SetTrigger("jumpFrontAttack");
                    
                }
                // 좌측 공격
                else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
                { 
                    actingButMove = true;
                    anim.SetTrigger("jumpFrontAttack");
                    
                }

                // up 공격
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    // 우측 공격
                    if (!spriteRenderer.flipX) 
                    { 
                        actingButMove = true;
                        anim.SetTrigger("jumpUpAttack");
                    }
                    // 좌측 공격
                    else if (spriteRenderer.flipX ) 
                    { 
                        actingButMove = true;
                        anim.SetTrigger("jumpUpAttack");
                    }
                }
            }
        }
    



        // 고개 숙이기
        else if (Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
        {

            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            Down_Coilder_Size();
            anim.SetBool("crouchDown" , true);
            if (Input.GetKeyDown(KeyCode.A))
            {

                // 우측 공격
                if (!spriteRenderer.flipX)
                {
                    anim.SetTrigger("crouchDownAttack");
                }
                // 좌측 공격
                else if (spriteRenderer.flipX)
                {
                    anim.SetTrigger("crouchDownAttack");
                }
            }

        }
        // 고개 들어올리기
        else if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting)
        {
            Idle_Coilder_Size();
            anim.SetBool("crouchDown" , false);
            anim.SetBool("crouchUp" , true);
        }


    }





    void attackEffect()
    {
        // 스탠딩 좌측 우측 공격
        if (!Input.GetKey(KeyCode.UpArrow) && !anim.GetBool("crouchDown"))
        {
            if (!spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("attackingRight"));
            }
            else if (spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("attackingLeft"));    
            }    
        }
        // 스탠딩 상단 공격
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("UpattackingRight")); 
            }
            else if (spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("UpattackingLeft")); 
            }
        }
        // 숙이고 좌우 공격
        else if (anim.GetBool("crouchDown"))
        {
            if (!spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("crouchDownAttackRight")); 
            }
            else if (spriteRenderer.flipX)
            {
                StartCoroutine(attackEffecter("crouchDownAttackLeft")); 
            }
        }
    }



    void jumpAttackEffect()
    {
        // 우측 공격
        if (!spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow))  
        { 
            StartCoroutine(attackEffecter("jumpmAttackRight")); 
        }
        // 좌측 공격
        else if (spriteRenderer.flipX && !Input.GetKey(KeyCode.UpArrow)) 
        { 
            StartCoroutine(attackEffecter("jumpmAttackLeft")); 
        }

        // up 공격
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // 우측 공격
            if (!spriteRenderer.flipX) 
            { 
                StartCoroutine(attackEffecter("UpattackingRight")); 
            }
            // 좌측 공격
            else if (spriteRenderer.flipX ) 
            { 
                StartCoroutine(attackEffecter("UpattackingLeft")); 
            }
        }
    }





    IEnumerator attackEffecter(string type)
    {
        // 스탠딩 공격
        if (type == "attackingRight")
        {
            if (hit1)
            {
                Hit(sideAttackTransformRight , sideAttackArea);      
                // 이펙트 위치
                Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
                GameObject effectInstance = Instantiate(sideAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
                effectInstance.transform.SetParent(playerTransform);
                yield return new WaitForSeconds(0.18f);    
                acting = false;
            }
            else if (hit2)
            {
                Hit(sideAttackTransformRight , sideAttackArea);      
                // 이펙트 위치
                Vector3 offset = new Vector3(-1.4f, -1.1f, 0f); 
                GameObject effectInstance = Instantiate(sideAttackEffect2, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
                effectInstance.transform.SetParent(playerTransform);
                yield return new WaitForSeconds(0.18f);    
                acting = false;
            }
            else if (hit3)
            {
                Hit(sideAttackTransformRightBig , sideAttackAreaBig);      
                // 이펙트 위치
                Vector3 offset = new Vector3(-2.4f, -1.2f, 0f); 
                GameObject effectInstance = Instantiate(sideAttackEffect3, sideAttackTransformRightBig.position+offset, sideAttackTransformRightBig.rotation);
                effectInstance.transform.SetParent(playerTransform);
                yield return new WaitForSeconds(0.18f);    
                acting = false;
            }
           
        }

        else if (type == "attackingLeft")
        {
            
            if (hit1)
            {
                Hit(sideAttackTransformLeft , sideAttackArea);            
                // 이펙트 위치
                Vector3 offset = new Vector3(1.7f, -1.2f, 0f); 
                GameObject effectInstance =  Instantiate(sideAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
                effectInstance.transform.SetParent(playerTransform);
                effectInstance.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.18f);
                acting = false;
            }
            else if (hit2)
            {
           
                Hit(sideAttackTransformLeft , sideAttackArea);            
                // 이펙트 위치
                Vector3 offset = new Vector3(1.4f, -1.1f, 0f); 
                GameObject effectInstance =  Instantiate(sideAttackEffect2, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
                effectInstance.transform.SetParent(playerTransform);
                effectInstance.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.18f);
                acting = false;
            }
            else if (hit3)
            {
           
                Hit(sideAttackTransformLeftBig , sideAttackAreaBig);            
                // 이펙트 위치
                Vector3 offset = new Vector3(2.4f, -1.2f, 0f); 
                GameObject effectInstance =  Instantiate(sideAttackEffect3, sideAttackTransformLeftBig.position+offset, sideAttackTransformLeftBig.rotation);
                effectInstance.transform.SetParent(playerTransform);
                effectInstance.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.18f);
                acting = false;
            }

        }


        // 스탠딩 상단 공격
        else if (type == "UpattackingRight")
        {
            Hit(UpAttackTransform , UpAttackArea);   
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position + offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            yield return new WaitForSeconds(0.25f);
            acting = false;
            actingButMove = false;
        }

        else if (type == "UpattackingLeft")
        {
            Hit(UpAttackTransform , UpAttackArea);
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, -3.0f, 0f);    
            GameObject effectInstance = Instantiate(UpAttackEffect, UpAttackTransform.position+offset, UpAttackTransform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;

            if (anim.GetBool("jump"))
            {
                yield return new WaitForSeconds(0.15f);
            }
            else if (!anim.GetBool("jump"))
            {
                yield return new WaitForSeconds(0.15f);
            }
            
            acting = false;
            actingButMove = false;
        }

        // 하단 공격 
        else if (type =="crouchDownAttackRight")
        {
            acting = true;
            Hit(DownAttackTransformRight , DownAttackArea);
            // 이펙트 위치
            Vector3 offset = new Vector3(-1.1f, -0.7f, 0f);   
            GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformRight.position + offset, DownAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
            yield return new WaitForSeconds(0.15f);
            acting = false;
        }
        
        else if (type =="crouchDownAttackLeft")
        {
            acting = true;
            Hit(DownAttackTransformLeft , DownAttackArea);

            // 이펙트 위치
            Vector3 offset = new Vector3(1.1f, -0.7f, 0f);   
            GameObject effectInstance = Instantiate(DownAttackEffect, DownAttackTransformLeft.position+offset, DownAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(0.15f);
            acting = false;

        }


        // 점프 정면 공격
        else if (type == "jumpmAttackRight")
        {
            Hit(sideAttackTransformRight , sideAttackArea);      
            // 이펙트 위치
            Vector3 offset = new Vector3(-1.7f, -1.2f, 0f); 
            GameObject effectInstance = Instantiate(jumpmAttackEffect, sideAttackTransformRight.position+offset, sideAttackTransformRight.rotation);
            effectInstance.transform.SetParent(playerTransform);
            yield return new WaitForSeconds(0.1f);
            actingButMove = false;
        }


        else if (type == "jumpmAttackLeft")
        {
            Hit(sideAttackTransformLeft , sideAttackArea);            
            // 이펙트 위치
            Vector3 offset = new Vector3(1.7f, -1.2f, 0f); 
            GameObject effectInstance =  Instantiate(jumpmAttackEffect, sideAttackTransformLeft.position+offset, sideAttackTransformLeft.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(0.1f);
            actingButMove = false;
        }





    }








    // 공격 관련 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransformRight.position , sideAttackArea);
        Gizmos.DrawWireCube(sideAttackTransformLeft.position , sideAttackArea);

        Gizmos.DrawWireCube(UpAttackTransform.position , UpAttackArea);
        Gizmos.DrawWireCube(jumpDownAttackTransform.position , jumpDownAttackArea);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(DownAttackTransformRight.position , DownAttackArea);
        Gizmos.DrawWireCube(DownAttackTransformLeft.position , DownAttackArea);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(sideAttackTransformRightBig.position , sideAttackAreaBig);
        Gizmos.DrawWireCube(sideAttackTransformLeftBig.position , sideAttackAreaBig);

    }



    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                // Debug.Log(objectsToHit[i].GetComponent<enemy_move>());
                objectsToHit[i].GetComponent<enemy_move>().EnemyHit(damage);
                if (hit1)
                {
                    hit1 = false;
                    hit2 = true;
                }
                else if (hit2)
                {
                    hit2 = false;
                    hit3 = true;
                }
                else if (hit3)
                {
                    hit1 = true;
                    hit3 = false;
                }
            }

        }


        // if (_attackTransform.name  == "sideAttackTransformRight" || _attackTransform.name  == "sideAttackTransformRight")
        // {
        //     Debug.Log(33333);
        // }


    }






    IEnumerator pairOfFire()
    {
        acting = true;
        anim.SetTrigger("magicReady");
        Vector3 offset1 = new Vector3(4f, 0.7f, 0f); 
        GameObject fire1 = Instantiate(pairOfFire_effect, transform.position + offset1, transform.rotation);
        Vector3 offset2 = new Vector3(8f, 0.7f, 0f); 
        GameObject fire2 = Instantiate(pairOfFire_effect, transform.position + offset2, transform.rotation);
        Vector3 offset3 = new Vector3(12f, 0.7f, 0f); 
        GameObject fire3 = Instantiate(pairOfFire_effect, transform.position + offset3, transform.rotation);

        yield return new WaitForSeconds(1.35f); // 첫 번째 총알 생성 후 0.2초 대기
        acting = false;
    }



    IEnumerator threeBullets()
    {
        acting = true;
        anim.SetTrigger("magicReady"); // 1.35초를 기다려야함 (동작 속도는 4)

        // 탄환 기모으는 애니메이션

        Vector3 offset1 = new Vector3(2f, 3f, 0f); 

        
        GameObject bullet1 = Instantiate(threeBulletsReady_effect, transform.position + offset1, transform.rotation);
        Vector3 bullet1Position1 = transform.position + offset1;
        bullet1.transform.localScale = new Vector3(4f, 4f, 1f); // 이미지 크기 변경
        yield return new WaitForSeconds(0.2f); // 첫 번째 총알 생성 후 0.2초 대기
        

        Vector3 offset2 = new Vector3(-2f, 3f, 0f); 
        GameObject bullet2 = Instantiate(threeBulletsReady_effect, transform.position + offset2, transform.rotation);
        Vector3 bullet1Position2 = transform.position + offset2;
        bullet2.transform.localScale = new Vector3(4f, 4f, 1f); // 이미지 크기 변경
       
        yield return new WaitForSeconds(0.25f); // 두 번째 총알 생성 후 0.25초 대기

        Vector3 offset3 = new Vector3(0f, 6f, 0f); 
        GameObject bullet3 = Instantiate(threeBulletsReady_effect, transform.position + offset3, transform.rotation);
        Vector3 bullet1Position3 = transform.position + offset3;
        bullet3.transform.localScale = new Vector3(3.5f, 3.5f, 1f); // 이미지 크기 변경

        yield return new WaitForSeconds(0.3f); // 세 번째 총알 생성 후 0.3초 대기


        // 발사되는 탄환 생성
        yield return new WaitForSeconds(0.1f); 
        Instantiate(threeBulletsShot_effect, bullet1Position1, transform.rotation);
        yield return new WaitForSeconds(0.2f); 
        Instantiate(threeBulletsShot_effect, bullet1Position2, transform.rotation);
        acting = false;
        yield return new WaitForSeconds(0.3f);
        Instantiate(threeBulletsShot_effect, bullet1Position3, transform.rotation);

        


        Vector3 offset4 = new Vector3(1f, 2.5f, 0f); 
        GameObject bullet4 = Instantiate(threeBulletsReady_effect, transform.position + offset4, transform.rotation);
        bullet4.transform.localScale = new Vector3(4f, 4f, 1f); // 이미지 크기 변경
        Vector3 bullet1Position4 = transform.position + offset4;
        yield return new WaitForSeconds(0.2f); // 첫 번째 총알 생성 후 0.2초 대기
        

        Vector3 offset5 = new Vector3(-2f, 4f, 0f); 
        GameObject bullet5 = Instantiate(threeBulletsReady_effect, transform.position + offset5, transform.rotation);
        bullet5.transform.localScale = new Vector3(4f, 4f, 1f); // 이미지 크기 변경
        Vector3 bullet1Position5 = transform.position + offset5;
        yield return new WaitForSeconds(0.25f); // 두 번째 총알 생성 후 0.25초 대기

        Vector3 offset6 = new Vector3(-1f, 6f, 0f); 
        GameObject bullet6 = Instantiate(threeBulletsReady_effect, transform.position + offset6, transform.rotation);
        bullet6.transform.localScale = new Vector3(3.5f, 3.5f, 1f); // 이미지 크기 변경
        Vector3 bullet1Position6 = transform.position + offset6;
        yield return new WaitForSeconds(0.3f); // 세 번째 총알 생성 후 0.3초 대기


        // 발사되는 탄환 생성
        yield return new WaitForSeconds(0.1f); 
        Instantiate(threeBulletsShot_effect, bullet1Position4, transform.rotation);
        yield return new WaitForSeconds(0.2f); 
        Instantiate(threeBulletsShot_effect, bullet1Position5, transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Instantiate(threeBulletsShot_effect, bullet1Position6, transform.rotation);


    }



    IEnumerator parrying()
    {
        acting = true;
        anim.SetTrigger("parrying");
        yield return new WaitForSeconds(0.55f);
        acting = false;
    }










    // 데미지 판정
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 생존 상태일때만 작동하도록 설정해야 한다.
        if(collision.gameObject.tag =="enemy" && alive)
        {
            acting = false;
            actingButMove = false; 
            slidingExcept = false;
            OnDamaged(collision.transform.position);

        }
    }

    // 데미지 입은 경우 (무적상태)
    void OnDamaged(Vector2 targetPos)
    {
        // 데미지 상태로 전환
        damaged = true;

        // change layer
        gameObject.layer = 11;
        // or
        // gameObject.layer = LayerMask.NameToLayer("playerDamaged");
        
        // view alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); // 4번쨰는 알파값

        // reaction force
        // 부딪힌 목표물에서 오른쪽에 있으면 +1 왼쪽이면 -1
        int dirc = transform.position.x - targetPos.x  > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1) * 15 , ForceMode2D.Impulse);
       

        // anim
        anim.SetTrigger("damaged");

        energe("collision");

        Invoke("NoneDamged" , 0.3f);
        Invoke("OffDamaged" , 0.3f);
    }

    // 데미지 상태 해체 >> 이동 상태로 만듬
    // 경사로 속도처리로 인해서 다시 false 처리를 해주어야한다.
    // true 동안에는 이동할 수 없으므로 무적처리보다 더 빨리 처리해주어야 한다.
    void NoneDamged() { damaged = false; }


    // 데미지 상태 벗어난 경우 (무적 해제)
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    } 






    // 체력 / 에너지 관리
    void energe(string type)
    {
        // hp ---------------------------------
        // 충돌
        if (type == "collision")
        {
            playerHp.curHp -= 10;
        }
        
    



        // 0일 경우 게임오버
        if(playerHp.curHp < 0)
        {
            playerHp.curHp = 0;
        }





        // mp -----------------------------------
        if (type == "threeBullets")
        {
            playerMp.curMp -= 50;
        }
        

        // 0일 경우 게임오버
        if(playerMp.curMp < 0)
        {
            playerMp.curMp = 0;
        }
        


    }



}