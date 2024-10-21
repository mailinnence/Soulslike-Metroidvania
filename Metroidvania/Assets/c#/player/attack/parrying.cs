using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parrying : playerStatManager
{

    [Header("공격 피격시 관련 변수")]
    public CameraShake CameraShake;


    [Header("이펙트")]
    public GameObject parrying_effect;

   
    public Transform playerTransform;      // 이펙트 위치 변수

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
        parrying_();
        // 패링 중에 절벽으로 떨어질때 애니메이셔 전환이 되지 않음 일단 보류
        // if(anim.GetCurrentAnimatorStateInfo(0).IsName("parrying") && rigid.velocity.y < 0)
        // {
        //     anim.SetBool("jump" , true);
        // }
    }


    public void parrying_()
    {

        if(Input.GetKeyDown(KeyCode.D) && 
        !Input.GetKey(KeyCode.DownArrow) && 
        !anim.GetBool("jump") && 
        !anim.GetBool("jump2") && 
        !isSliding && !acting && !acting && !attacking && !damaged && !parrying_action )
        {
            anim.SetTrigger("parrying");
        }
    }



    public void parrying_failed_layer_anim_on()
    {
        gameObject.layer = 16;
    }


    public void parrying_failed_layer_anim_off()
    {
        gameObject.layer = 10;
    }







    public void parrying_interaction(bool isFlipped , string type , int Strength = 0)
    {
        if(type == "guard")
        {

            SpriteRenderer slidingSpriteRenderer = parrying_effect.GetComponent<SpriteRenderer>();
            slidingSpriteRenderer.flipX = spriteRenderer.flipX;

            Vector3 offset = new Vector3(0f, 0f, 0f);

            anim.SetTrigger("parrying_guard");
            CameraShake.TriggerShake(7f, 6f, 0.15f);
            if (isFlipped) 
            {
                GameObject effectInstance = Instantiate(parrying_effect, transform.position+offset, transform.rotation);
                effectInstance.transform.SetParent(playerTransform);

                spriteRenderer.flipX = false;
                rigid.AddForce(new Vector2(-1 * Strength,0) , ForceMode2D.Impulse);
            }
            else
            { 
                GameObject effectInstance = Instantiate(parrying_effect, transform.position+offset, transform.rotation);
                effectInstance.transform.SetParent(playerTransform);
                effectInstance.GetComponent<SpriteRenderer>().flipX = true;

                spriteRenderer.flipX = true;
                rigid.AddForce(new Vector2(Strength,0) , ForceMode2D.Impulse);
            }
        }


        else if (type == "counter")
        {
            CameraShake.TriggerShake(7f, 6f, 0.15f);
            anim.SetTrigger("parrying_counter");
            if (isFlipped) 
            {
                spriteRenderer.flipX = false;
                rigid.AddForce(new Vector2(-1 * Strength,0) , ForceMode2D.Impulse);
            }
            else
            { 
                spriteRenderer.flipX = true;
                rigid.AddForce(new Vector2(Strength,0) , ForceMode2D.Impulse);
            }
        }   
    }



    public void parrying_sound_counter_off_anim()
    {
        parrying_counter = false;
    }
 


}
