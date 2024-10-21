using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface moveFunction
{
    // 기본 이동
    public void InputBasedCharacterMovement() {}
    public void ConstantSpeedMovement() {} 

    // 점프
    public void Jump() {}                           // maxJumpCount 를 조절해서 점프의 단계를 조정한다.
    public void JumpLandingDetection() {}
    public void Sliding() {}
}




// 어떠한 영향도 받지 않은 이동 상태
public class move : playerStatManager ,  moveFunction
{

    [Header("확인용 변수")]
    public float temp; 


    [Header("캐릭터 이동 관련 변수")]
    public float maxSpeed = 10f;           // 속력
    public float jumpPower = 40f;          // 점프 파워
    public float jumpCount = 0f ;          // 현재까지의 점프 횟수
    public int maxJumpCount = 2;           // 최대 점프 횟수
    public float jumpRayfloat;             // 경사로마다 레이캐스트 값


    [Header("캐릭터 경사로 관련 변수")]
    public float angle;                    // 경사로 판단 각도
    public float maxangle;                 // 경사로 최대각도 : 조건문 사용시 사용
    public Vector2 perp;                   // 경사로 속도를 일정하게 할 벡터


    [Header("슬라이딩 관련 변수")]
    public float slideDuration = 0.7f;     // Duration of the sliding animation
    public float slideSpeed = 30f;


    [Header("에어도어 관련 변수")]          // 슬라이딩의 관련 변수를 상속함
    public float airdoorSpeed = 90f;
    public float airdoorDuration = 0.1f;
    public int airdoorCounter = 0;
    public int max_airdoorCounter = 1;
    public bool non_collider;


	[Header("효과 오브젝트")]
    public GameObject Sliding_effect;       // 슬라이딩 이펙트
    public GameObject Doublejump_effect;    // 더블 점프 이펙트
    public GameObject JumpLandingDust;      // 착지 이펙트
    public GameObject AirDoor_Effect;       // 에어대쉬 이펙트


    [Header("콜라이더 크기 조정")]
    public collider collider;


    [Header("효과음 변수")]
    public effectSound effectSound;


    [Header("현재 사다리 위치 초기화 변수")]
    public Vector3 laddering_position;


    [Header("잔상 효과")]
    public AfterImage ghost;                // 잔상효과
    
    
    [Header("중력 관련 변수")]
    public float timer = 0.0f;
    public float timeInterval; // 중력이 변경될 시간 간격 (초)



    [Header("높은 곳에서 떨어질때 카메라 쉐이크 관련 변수")]
    public CameraShake CameraShake;



    // 레이어 처리 변수
    private int platformAndObstacleMask;
    private int platformIgnoreBox;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ghost = GetComponent<AfterImage>();                     // 잔상 효과
    }

    void Start()
    {
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
        platformIgnoreBox = LayerMask.GetMask("player", "playerDameged");
    }



    void Update()
    {
        


        // 애니메이션 전환
        // JumpAnim_change();

        // 잔상종류
        if (!isSliding || anim.GetBool("jump") && !airdooring)
        {
            ghost.makeGhost = false;
        }
        
        if (jump_ghost)
        {
            ghost.makeGhost = true;
        }

        if (damaged == true || 
        anim.GetCurrentAnimatorStateInfo(0).IsName("hangon") || 
        anim.GetCurrentAnimatorStateInfo(0).IsName("hangup") )
        {
            jumpCount = 0;
        }
    }


    



    public void InputBasedCharacterMovement()
    {
        // 경사로에서 내려가는 것을 방지하는 기능 : 누르지 않는다면 x축 이동 정지
        if (!Input.GetButton("Horizontal") && !isSliding)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }

        // ConstantSpeedMovement() 에서 가속 방식을 택했다면 무한히 빨라지는 걸 막기 위해서 있어야 한다.
        else if (Input.GetButtonUp("Horizontal"))
        { 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); 
        }
        
        // 누르고 있다면 이동 정지 해제
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // 방향키에 따른 sprite 방향 전환
        if (Input.GetButton("Horizontal") && !isSliding && !isLadder) 
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
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

    // 사다리 ----------------------------------------------------------------------------------------------------------------------
    
    // 사다리 위치 초기화 함수
    public void laddering_position_init(Vector3 position)
    {
        laddering_position = position;
    }



    // 사다리 이동 함수 
    public void laddersVerticalMovement() 
    {
        int platformLayerMask = (1 << LayerMask.NameToLayer("platform")) | (1 << LayerMask.NameToLayer("ignorePlatform"));


        // 올라오기
        // 애니메이션 전환 방지
        if(Input.GetKeyUp(KeyCode.DownArrow) && anim.GetBool("laddering") )
        {
            anim.SetBool("jump", false); anim.SetBool("jump2", false);
        }


        // 사다리에 닿아서 위 , 아래 방향키를 눌렀을때 애니메이션 전환 및 위 아래로 이동
        if (isLadder && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            // x 의 위치 고정
            Vector3 newPosition = transform.position;
            newPosition.x = laddering_position.x;
            transform.position = newPosition;

            anim.SetBool("jump", false); anim.SetBool("jump2", false);

            anim.speed = 1f;
            anim.SetBool("laddering", true);

            float ver = Input.GetAxis("Vertical");
            rigid.gravityScale = 0;
            rigid.velocity = new Vector2(0, ver * 3.5f); // ver 값에 따라 위로 또는 아래로 이동

        }

        // 사다리에 닿아서 위 , 아래 방향키를 누른 후 애니메이션 전환 중 위 아래로 이동을 멈추었을때 애니메이션 정지
        else if(isLadder && (!Input.GetKey(KeyCode.UpArrow) || !Input.GetKey(KeyCode.DownArrow)) && anim.GetBool("laddering") ) 
        { 
            rigid.velocity = new Vector2(0 , 0f);
            anim.speed = 0f;
        }
        

        // 중력 속도 , 애니메이션 원상 복귀
        else if(!isLadder && !jump_verticalattack || hurt)
        {
            isLadder = false;
            anim.speed = 1f;
            rigid.gravityScale = gravity;
            rigid.velocity = new Vector2( rigid.velocity.x, rigid.velocity.y);
            anim.SetBool("laddering", false);
        }

   

        // 매달려 있다가 점프나 떨어지는 버튼을 누르면 점프로 전환
        if (isLadder  && anim.GetBool("laddering") && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftShift)) )
        {
            isLadder = false;
            jumpCount = 0;

            anim.speed = 1f;
            rigid.gravityScale = gravity;
            rigid.velocity = new Vector2( rigid.velocity.x, rigid.velocity.y);
            anim.SetBool("jump", true);
        }

        // 사다리로 이동하다가 땅에 닿으면 사다리 상태 해체
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down, 0.5f , platformAndObstacleMask);
        Debug.DrawRay(rigid.position, Vector3.down *  0.5f, new Color(1, 1, 1));

        if (isLadder && Input.GetKey(KeyCode.DownArrow) && anim.GetBool("laddering") && rayHit.collider != null ) 
        { 
            isLadder = false;
            jumpCount = 0;
            anim.speed = 1f;
            rigid.gravityScale = gravity;
            rigid.velocity = new Vector2( rigid.velocity.x, rigid.velocity.y);
            anim.SetBool("laddering", false);
        }




    }

    // 사다리 >> 지면 올라가기
    public void laddersUpAnim(Vector3 position) 
    {
        if(rigid.velocity.y > 0 && !isLadderDown)
        {
            isLadder = false;
        
            Vector3 offset = new Vector3(0f, 0.5f, 0f);

            Vector3 newPosition = transform.position;
            newPosition = position + offset;
            transform.position = newPosition;
            anim.SetBool("ladder_up", true);
        }
    }

    // 사다리 >> 지면 올라가기 - 애니메이션 딜레이
    public void laddersUpAnim_delay()
    {
        anim.SetBool("ladder_up", false);
    }



    // 사다리 >> 지면 올라가기 - 세팅 변경 딜레이
    public void laddersUpSetting_delay()
    {
        jumpCount = 0;
        anim.speed = 1f;
        rigid.gravityScale = gravity;
        rigid.velocity = new Vector2( rigid.velocity.x, rigid.velocity.y);
    }






    // ---------------------------------------------------------------------------------------------------------------------------





    public void ConstantSpeedMovement()
    {
        // 방향키를 받아온다.
        float h = Input.GetAxisRaw("Horizontal"); 


        // !isSliding :평지에서 슬라이딩할떄 걷는 힘이 더해지면 슬라이딩의 힘이 일정하지 않다 떄문에 슬라이딩시 걷는 힘을 제한해야 한다.

        if (!isSliding && !anim.GetBool("crouchDown")) 
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }


        if (rigid.velocity.x > maxSpeed && !isSliding )
        {   // right max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // y축이 0이면 공중에 멈춰있으니 현재 속도를 넣어주어야 한다.
        }
        else if (rigid.velocity.x < maxSpeed * (-1) && !isSliding )
        {   // left max
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // y축이 0이면 공중에 멈춰있으니 현재 속도를 넣어주어야 한다.
        }
        // 경사로 속도 일정하게
        else if (isSlope && angle > 0 && !isSliding)
        {

            if (!anim.GetBool("jump"))
            {
                rigid.velocity = perp * maxSpeed * h * -1;
            }
            else{
                Vector2 velocityOnSlope = perp * maxSpeed * h * -1;
                rigid.velocity = new Vector2(velocityOnSlope.x, rigid.velocity.y);
            }
        }
    }





    public void Jump()
    {
       
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount && !anim.GetBool("crouchDown")  
        && !anim.GetBool("hangon")  && !anim.GetBool("hangup") && !anim.GetBool("stabbing_attack") && !isSliding_stabbing)
        {
            // 점프효과음
            if (jumpCount == 0) { effectSound.PENITENT_JUMP_function(); }
            
            
            // 속도 조절
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            
            // 모션에 따른 각기 다은 점프
            float h = Input.GetAxisRaw("Horizontal"); 
            if (h != 0 && !anim.GetBool("laddering") &&  anim.GetBool("walk") ) { anim.SetBool("jump2", true); anim.SetBool("jump", false); }
            else { anim.SetBool("jump", true); anim.SetBool("jump2", false); }
            
            jumpCount++;

            // 첫 번째 점프가 아닌 경우 점프 애니메이션 상태를 재설정
            if (jumpCount > 1)
            {
                // anim.Play("jump", 0, 0f); // 
              
                anim.SetBool("jump", true); anim.SetBool("jump2", false); 
                anim.Play("jump", 0, 0f);

                // 과잉 방지
                rigid.velocity = new Vector2(rigid.velocity.x , 0);


                // 2단 점프 효과
                Vector3 offset = new Vector3(0f, 0.5f, 0f); 
                Instantiate(Doublejump_effect, transform.position + offset, transform.rotation);

                // 2단 점프 이펙트 효과음
                effectSound.jump2effectSound_function(); 

            }

            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }



    public void jump_run()
    {
        if(rigid.velocity.y < 0)
        {
            anim.SetTrigger("jump_run");
        }
    }



    public void jump2_run()
    {
        anim.SetTrigger("jump2_run");
    }


    public void JumpLandingDetection()
    {
        // 중앙
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down, 3 , platformAndObstacleMask);

        // 좌우
        Vector2 offsetLeft = new Vector2(-0.17f, 0f); 
        Vector2 offsetRight = new Vector2(0.17f, 0f);

        RaycastHit2D rayHitLeft = Physics2D.Raycast(rigid.position + offsetLeft , Vector3.down,0.3f, platformAndObstacleMask);
        RaycastHit2D rayHitRight = Physics2D.Raycast(rigid.position + offsetRight , Vector3.down,0.3f, platformAndObstacleMask);  


        if (rigid) {SlopeChk(rayHit);}

        // Landing Platform
        if (rigid.velocity.y < 0 ) 
        {     
            anim.SetBool("jump", true);

            Debug.DrawRay(rigid.position, Vector3.down *  0.8f, new Color(0, 1, 0));
            Debug.DrawRay(rigid.position + offsetLeft, Vector3.down *  0.3f, new Color(0, 1, 0));
            Debug.DrawRay(rigid.position + offsetRight, Vector3.down *  0.3f, new Color(0, 1, 0));
            
            // 중앙 레이케스트
            if (rayHit.collider != null) 
            {            
                if (rayHit.distance < 0.75f)
                {
                    anim.SetBool("jump", false);
                    anim.SetBool("jump2", false);

                    jumpCount = 0;  // 점프 횟수 초기화
                    airdoorCounter = 0;

                    // 점프 후에 땅에 닿았을때 먼지 효과
                    if (jump_high == 1) // or 1.0f
                    {
                        if (high_landing_) 
                        {
                            CameraShake.TriggerShake(5f, 5f, 0.1f);
                            anim.SetTrigger("high_landing");
                        }
                        else
                        {
                            JumpLandingDustEffect();
                            effectSound.PENITENT_LANDING_MARBLE_function();
                        }
                        jump_high = 0;
                    }
    
                }
            }
            

            else if (rayHitLeft.collider != null && rayHitLeft.distance < 0.3f || rayHitRight.collider != null && rayHitRight.distance < 0.3f )
            {
                anim.SetBool("jump", false);
                anim.SetBool("jump2", false);


                jumpCount = 0;  // 점프 횟수 초기화
                airdoorCounter = 0;
            }
        }
    }


    // 착지 이펙트 함수
    public void JumpLandingDustEffect()
    {
        Vector3 offset = new Vector3(0f, -0.3f, 0f); 
        Instantiate(JumpLandingDust, transform.position + offset, transform.rotation);
    }
    

    // 점프 중일때 애니메이션 전환 
    // public void JumpAnim_change()
    // {
 

    // }





    

    public void SlopeChk(RaycastHit2D rayHit)
    {
        angle = Vector2.Angle(rayHit.normal , Vector2.up);
        perp = Vector2.Perpendicular(rayHit.normal).normalized;

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




    // 슬라이딩 (회피)
    public void Sliding()
    {

        if (Input.GetButtonDown("Dash") && !anim.GetBool("jump") && !anim.GetBool("jump2")  && !isSliding &&! anim.GetBool("laddering"))
        {

            effectSound.PENITENT_DASH_function();
            StartCoroutine(Slide());
        }

        // 점프 중이고 공중에서 딱 한번만 에어도어가 가능
        else if (Input.GetButtonDown("Dash") && (anim.GetBool("jump") || anim.GetBool("jump2")) && max_airdoorCounter > airdoorCounter &&! anim.GetBool("laddering"))
        {
          
            StartCoroutine(AirDoor());
        }
    }

    public void SlidingGhostOff()
    {
        ghost.makeGhost = false;
    }
        


    IEnumerator Slide()
    {

        bool isFacingRight = spriteRenderer.flipX;

        // Debug.Log(isFacingRight);
        isSliding = true;
        anim.SetTrigger("sliding");

        // 잔상효과
        ghost.ghostDelay = 0.07f;
        ghost.makeGhost = true;

        // 회피이므로 레이어 수정
        gameObject.layer = 30;


        // 슬라이딩 콜라이더 크기 조정
        collider.Sliding_Coilder_Size(); 

        // 슬라이딩 먼지 효과
        Sliding_dust_effect();


        float slideTimer = 0f;
        float initialSlideSpeed = slideSpeed;
        float currentSlideSpeed = initialSlideSpeed;



        while (slideTimer < slideDuration && !anim.GetBool("jump") && !anim.GetBool("jump2"))
        {
            // 찌르기
            if (Input.GetKeyDown(KeyCode.A))
            {
                // 'A' 키가 눌리면 슬라이딩 속도를 다시 초기 슬라이딩 속도로 설정합니다.
                currentSlideSpeed = initialSlideSpeed;
                // 슬라이드 타이머를 초기화하거나 원하는 값으로 설정할 수 있습니다.
                slideTimer = 0f; // 예를 들어, 슬라이드 타이머를 초기화하여 다시 슬라이딩 시작처럼 만들 수 있습니다.
            }


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
        if (!anim.GetBool("jump") && !anim.GetBool("jump2") && !anim.GetBool("jump_run_landing"))
        {
            // 자연스러운 물리 설정을 위해서 점프 중에 그 속도를 유지하도록 하기 위해서 조건문 배치함
            // rigid.velocity = Vector2.zero;
   
            // anim.Play("idle", 0, 0.01f);
        }
        

        isSliding = false;
      
        stabbing_ = false;
        // 콜라이더 크기 원상복귀
        collider.Idle_Coilder_Size();
        gameObject.layer = 10;
    }






    IEnumerator AirDoor()
    {
        non_collider = true;

        anim.Play("jump2", 0, 0f);
        anim.SetBool("jump2", true);
        anim.SetBool("jump", false);

        // 효과음
        effectSound.airdash_function();

        airdooring = true;

        gameObject.layer = 30;
        ghost.ghostDelay = 0.01f;
        ghost.makeGhost = true;

        bool isFacingRight = spriteRenderer.flipX;
        float originalGravityScale = rigid.gravityScale;
        rigid.gravityScale = 0f;
        isSliding = true;

        // 에어도어 효과
        Vector3 offset = new Vector3(0f, 0.0f, 0f); 
        Instantiate(AirDoor_Effect, transform.position + offset, transform.rotation);

        // 이동 방향 설정
        float direction = isFacingRight ? 1f : -1f;

        // 이동 속도 및 방향에 따라 Rigidbody2D에 힘을 가함
        rigid.velocity = new Vector2(-direction * airdoorSpeed, rigid.velocity.y);

        yield return new WaitForSeconds(airdoorDuration); // 에어도어 지속 시간동안 대기

        isSliding = false;
        rigid.gravityScale = originalGravityScale;
        airdoorCounter++;

        airdooring = false;
        gameObject.layer = 10;

        non_collider = false;
    }




    // 슬라이딩 시 먼지 효과
    public void Sliding_dust_effect()
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





    // ignore box 관련 처리 ---------------------------------------------------------------------------------------
    private float ignoreDuration = 0.4f; // 충돌 무시 지속 시간
    public void ignoreBox()
    {
        if(!ignoreButton)
        {
            if(rigid.velocity.y > 0f || anim.GetBool("laddering") || anim.GetBool("ladder_down"))
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("player"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("playerDameged"), true);
            }
            else if(rigid.velocity.y <= 0f)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("player"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("playerDameged"), false);
            }
        }
        else
        {
            anim.SetBool("jump" , true);
            StartCoroutine(TemporaryIgnoreCollision());
        }
        


        // 래이캐스트로 아래쪽에 ignorePlatform가 있는지 확인할 것!
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 아래에 ignorePlatform 있을때에만 LeftShift를 누를 수 있게 설정
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position , Vector3.down, 3 , LayerMask.GetMask("ignorePlatform"));
            Debug.DrawRay(rigid.position, Vector3.down *  0.8f, new Color(1, 1, 1));

            if (rayHit.collider != null) 
            {           
                ignoreButton = true;
            }

        }
    }


    private IEnumerator TemporaryIgnoreCollision()
    {
        anim.SetBool("crouchDown" , false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("playerDameged"), true);

        yield return new WaitForSeconds(ignoreDuration);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("player"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ignorePlatform"), LayerMask.NameToLayer("playerDameged"), false);
        ignoreButton = false;
    }


        



    // 점프 거리 측정이랑 같이가야함
    // 애니메이션에 넣을것
    
    public void gravity_change()
    {
        // Debug.Log(gravity);

        if (gravity_anim_) { timer += Time.deltaTime; }

        if (timer >= 0.3f)
        {
            gravity = 3.5f;
        }

        if (timer >= 0.5f)
        {
            gravity = 4f;
        }

        if ((!anim.GetBool("jump") && !anim.GetBool("jump2")) 
        || anim.GetBool("wallclimbing") || gravity_hit)
        {
            gravity_anim_ = false;
            gravity_hit = false;
            timer = 0f;
            gravity = 2.2f;  // 2.0 으로 갈지 고민
        }

    }



    public void gravity_anim()
    {
        gravity_anim_ = true;
    }







    
    // 리스펀시 행동 초기화
    public void player_setting_init()
    {
        // 세부 설정 초기화
        jumpCount = 0;
        ghost.makeGhost = false;

        // 애니메이션 초기화
        anim.SetBool("walk" , false);
        anim.SetBool("jump" , false);
        anim.SetBool("jump2" , false);
        anim.SetBool("jump_falling" , false);
        anim.SetBool("crouchDown" , false);
        anim.SetBool("crouchUp" , true);
        anim.SetBool("wallclimbing" , false);
        anim.SetBool("wallclimbing_jump" , false);
        anim.SetBool("laddering" , false);
        anim.SetBool("ladder_up" , false);
        anim.SetBool("ladder_down" , false);
        anim.SetBool("charging_shot" , false);

        // 트리거 초기화
        anim.ResetTrigger("jump_run");
        anim.ResetTrigger("jump2_run");
        anim.ResetTrigger("jump1_2");
        anim.ResetTrigger("high_landing");
        anim.ResetTrigger("sliding");
        anim.ResetTrigger("stabbing_attack");
        anim.ResetTrigger("damaged");
        anim.ResetTrigger("damaged2");
        anim.ResetTrigger("death");
        anim.ResetTrigger("death_trap");
        anim.ResetTrigger("crouchDownAttack");
        anim.ResetTrigger("Upattacking");
        anim.ResetTrigger("attacking");
        anim.ResetTrigger("jumpUpAttack");
        anim.ResetTrigger("jumpFrontAttack");
        anim.ResetTrigger("RecoveryAction");
        anim.ResetTrigger("Activation");
        anim.ResetTrigger("knee_pray");
        anim.ResetTrigger("up_pray");
        anim.ResetTrigger("skillReady");
        anim.ResetTrigger("hangup");
        anim.ResetTrigger("jump_vertical_attack");
        anim.ResetTrigger("jump_vertical_finish");
        anim.ResetTrigger("charging");
        anim.ResetTrigger("parrying");
        anim.ResetTrigger("parrying_counter");
        anim.ResetTrigger("parrying_guard");
        anim.ResetTrigger("respawn");
        anim.ResetTrigger("item_pickUp");
        anim.ResetTrigger("item_pickDown");
        anim.ResetTrigger("waiting");
        anim.ResetTrigger("waiting_end");
        anim.ResetTrigger("knee");



        


        // 변수 초기화
        damaged = false;              
        damagedMove = false;          
        isSlope = false;                       
        jumpAble = true;                       
        jump_verticalattack = false;       
        jump_ghost = false;        
        jump_high = 0;          
        gravity_anim_ = false;          
        gravity_hit = false;
        high_landing_ = false;   
        slidingExcept = false;        
        isSliding = false;            
        isSliding_stabbing = false;            
        stabbing_ = false;   
        airdooring = false;
        acting = false;
        actingButMove = false;
        alive = true;      
        hurt = false;            
        attacking = false;                             
        attackAble = true;                                        
        itemUsingState = false;                        
        textAble = false;                                      
        isLadder = false;             
        isLadderDown = false;           
        DownKeyLimit = false;            
        hangAble = false;
        parrying_action = false;
        parrying_counter = false;
    }




}






