using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction_object : playerStatManager
{
    
    [Header("효과음 변수")]
    public effectSound effectSound;
    
    [Header("제물 변수")]
    public RedPortalFx_0 RedPortalFx_0;
    public event_item_1 event_item_1;
    public event_item_2 event_item_2;



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


    void Update()
    {
       
    }

    // 기도대 활성화 애니메이션
    public void Activation_Anim()
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            effectSound.prayTable_Charging_function();
            anim.SetTrigger("Activation");
        }
    }


    // 기도대 휴식
    public void knee_pray_Anim()
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !anim.GetBool("walk") && !isSliding && !acting )
        {
            anim.SetTrigger("knee_pray");
        }
    }


    // 기도대 휴식 중단
    public void knee_Up_Anim()
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding )
        {
            
            effectSound.prayTable_knee_up_function();
            anim.SetTrigger("up_pray");
        }
    }



    public void pickUp_Anim(Vector3 currentPosition)
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            // 현재 위치의 x 값을 현재 Transform의 y, z 값과 함께 설정
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);

            // effectSound.prayTable_Charging_function();
            anim.SetTrigger("item_pickUp");
        }
    }




    public void pickUp_Anim2(Vector3 currentPosition)
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            // 현재 위치의 x 값을 현재 Transform의 y, z 값과 함께 설정
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);

            // effectSound.prayTable_Charging_function();
            anim.SetTrigger("item_pickUp2");
        }
    }





    public void pickDown_Anim(Vector3 currentPosition)
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {

            // 현재 위치의 x 값을 현재 Transform의 y, z 값과 함께 설정
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);

            // effectSound.prayTable_Charging_function();
            anim.SetTrigger("item_pickDown");
            
            spriteRenderer.flipX = true;
            StartCoroutine(pickDown_waiting());
        }
    }


    
    IEnumerator pickDown_waiting()
    {

        yield return new WaitForSeconds(1.5f);
        spriteRenderer.flipX = true;
        anim.SetTrigger("waiting");
        yield return new WaitForSeconds(3.5f);
        RedPortalFx_0.FadeIn(1f);
        // json 파일 변경
    }

    


    public void waiting_end()
    {
        // spriteRenderer.flipX = true;
        anim.SetTrigger("waiting_end");

    }


    public void again_pickUp()
    {
        anim.SetTrigger("item_pickUp");

    }



    // 제물 이벤트 
    public void event_item_1_on()
    {
        event_item_1.object_put_down();
    }

    public void event_item_1_of()
    {
        event_item_1.object_put_on();
    }


    public void event_item_2_on()
    {
        event_item_2.object_put_down();
    }

    public void event_item_2_of()
    {
        event_item_2.object_put_on();
    }





    // 포털 애니메이션
    public void kneeDonw_Anim(Vector3 currentPosition)
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            // 현재 위치의 x 값을 현재 Transform의 y, z 값과 함께 설정
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);

            // effectSound.prayTable_Charging_function();
            anim.SetTrigger("knee");
        }
    }


    public void waiting_Anim()
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            anim.SetTrigger("waiting");
        }
    }

    public void waiting_end_Anim()
    {
        if (!Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
        {
            anim.SetTrigger("waiting_end2");
        }
    }

    public void waiting_end_Anim2()
    {

        anim.SetTrigger("waiting_end2");
        
    }



}
