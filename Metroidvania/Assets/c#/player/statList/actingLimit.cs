using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actingLimit : playerStatManager
{

    
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
     
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        acting_Limit();
        if (!hurt) attacking_Limit();
        rigid_Limit();
        penitent_verticalattack_falling_bug_prevention();
        attacked_velocity_limit();
        stabbing_limit_off();
        parrying_Limit();


    }



    // 애니메이션 중 이동 금지
    public void acting_Limit()
    {
        string[] actingStates = { "Activation" , "knee_pray" , "knee_Up_pray" , "Recovery Action" , "skillReady" ,
         "hangon" , "hangup" , "wallclimbing_finish" , "jump_vertical_attack" , "jump_vertical_attaack_start" 
         , "penitent_verticalattack_landing" , "penitent_verticalattack_falling" ,"high_landing" , "item_pickUp" , "item_pickDown" , 
         "waiting" , "waiting_end" , "waiting_start" , "knee" , "risingUp" , "death_trap" ,"item_pickUp2" , "first_risingUp", "waiting_end2",
         };
        acting = System.Array.Exists(actingStates, state => anim.GetCurrentAnimatorStateInfo(0).IsName(state));    
    }


    public void attacking_Limit()
    {
        string[] actingStates = {"1hit", "2hit", "3hit" ,"Upattack" , "crouchDownAttack", "wallclimbing_start" , 
        "wallclimbing_ing" , "wallclimbing_finish"  , "hangon" , "hangup" , "climb"
        , "charging_start" , "charging" , "charging_attack"};
        attacking = System.Array.Exists(actingStates, state => anim.GetCurrentAnimatorStateInfo(0).IsName(state));    

        // 경사로에서 내려가는 것을 방지하는 기능 : 누르지 않는다면 x축 이동 정지
        if (attacking)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }


        if(anim.GetCurrentAnimatorStateInfo(0).IsName("animUp"))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
    }

    // 데미지를 입었을 경우 속도 제한
    public void attacked_velocity_limit()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attacked"))
        {
            
            Vector2 velocity = rigid.velocity;

            // x축 속도 제한
            if (Mathf.Abs(velocity.x) > 15)
            {
                velocity.x = 15 * Mathf.Sign(velocity.x);
            }

            // y축 속도 제한
            if (Mathf.Abs(velocity.y) > 15)
            {
                velocity.y = 15 * Mathf.Sign(velocity.y);
            }

            rigid.velocity = velocity;
        }
    }



    // 별도의 bool 값 처리 없이 특정 애니메이션 x축 속도 제어 + 축 변환 제어
    public void rigid_Limit()
    {

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("animUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("high_landing") || anim.GetCurrentAnimatorStateInfo(0).IsName("risingUp"))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
        }
    }



    // 패링 중 애니메이션 전환
    public void parrying_Limit()
    {
        string[] parrying_limit_ = { "parrying", "success" , "parrying_counter" ,"parrying_guard"};
        parrying_action = System.Array.Exists(parrying_limit_, state => anim.GetCurrentAnimatorStateInfo(0).IsName(state));  

        if (parrying_action)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation; 
            
        }

    }








    // 점프 내려찍기 버그 방지 - 레이캐스트에 맞지 않은 체 끼면 계속 내려찍기 상태를 유지하는 버그 수정 
    public void penitent_verticalattack_falling_bug_prevention()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("penitent_verticalattack_falling") && rigid.velocity.y == 0 )
        {
            anim.SetTrigger("jump_vertical_finish");
        }
    }



    // 찌르기 중복 방지 - 해결 못함
    public void stabbing_limit_off()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("stabbing_attack"))
        {
            stabbing_ = true;
        }
        else
        {
            stabbing_ = false;
        }
    }



    bool IsPositionXFrozen()
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        
        // FreezePositionX가 설정되어 있는지 확인합니다.
        return (rigid.constraints & RigidbodyConstraints2D.FreezePositionX) != 0;
    }



 

}
