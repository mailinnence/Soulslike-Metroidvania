using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : playerStatManager
{

    // 인스턴스 선언
    public static player camera;
    

    private move move;                  // 캐릭터 이동          
    // private attack attack;              // 캐릭터 공격
    private attack2 attack2;              // 캐릭터 공격

   


    void Awake()
    {
        // 컴포넌트 초기화
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        // 카메라
        if(camera != null && camera != this){Destroy(gameObject);} else{camera = this;}


        // 캐릭터 이동
        move = GetComponent<move>();

        // 캐릭터 공격
        // attack = GetComponent<attack>();

         attack2 = GetComponent<attack2>();

     
    }


    void Update()
    { 
        
        // 게임 끄기
        gameOff();

        if (!inventory && !stop)
        {
            if(Input.GetKeyDown(KeyCode.Z) )
            {

                anim.SetTrigger("respawn");
            }

            // 중력
            move.gravity_change();

            if (!acting && alive && !damaged && !parrying_action)
            {
                // 공격을 했다면 이동을 하면 안된다.
                if (!attacking && !itemUsingState) 
                {
                    move.InputBasedCharacterMovement();
                    move.laddersVerticalMovement();
                    move.Jump();                 // 로직이 꼬여서 올라타는 애니메이션 동안에는 방향 전환을 막는 안 된다. 때문에 변수로 강제로 막는 방법을 사용한다.
                    move.Sliding();
                    move.ignoreBox();
                }
            if(attackAble)  { attack2.playerAttackAction(); }
                
            }



            // 물리 , 마법을 사용할때는 이동과 애니메이션을 정지시킴
            else if (acting || attacking)
            {
                anim.SetBool("walk" , false);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }
        }

        if(stop)
        {
            anim.SetBool("walk" , false);
        }


    }

    void FixedUpdate()
    {
        if(!inventory && !stop)
        {
            if (!acting && alive && !damaged && !parrying_action)
            {
                if (!attacking && !itemUsingState) 
                {
                    move.ConstantSpeedMovement();
                    move.JumpLandingDetection();
                    move.laddersVerticalMovement();
                    move.ignoreBox();
                }
            }

            else if (acting || attacking)
            {
                anim.SetBool("walk" , false);
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 
            }

        }

    }




    void gameOff()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }



}
