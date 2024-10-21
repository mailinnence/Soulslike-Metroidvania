using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : playerStatManager
{
    [Header("Hp")]
    public hp playerHp;

    [Header("Mp")]
    public mp playerMp;

    [Header("이펙트")]
    public GameObject RecoveryEffect;
    [HideInInspector] string recoveryAnimColor;


    [Header("효과음 변수")]
    public effectSound effectSound;

    [Header("아이템 갯수")]
    public itemManager itemManager; 


    // 향후에 ui 에서 변경할 수 있도로 설정할 예정
    // 일단 아이템이 체력과 마력만 회복하는 아이템만 있다고 가정
    [HideInInspector] public string item_1;
    [HideInInspector] public string item_2;
    [HideInInspector] public string item_3;





    [HideInInspector]
    private Transform playerTransform;      // 이펙트 위치 변수

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        item_1 = "hp_potion";
        item_2 = "mp_potion";
    }



    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

        if(!inventory) 
        {
            // 아이템 키 입력
            inputKey();

            // 중복 사용 금지
            Preventduplicateuse();

            // 애니메이션 중 이동 중지
            // RecoveryAnim();
        }

    }


    // 아이템 키 입력
    // ui 파트에서 아이템을 바꾸는 함수가 추가 될 것이다. hp , mp 포션만 있다고 가정해보자
    // 각 번호에 있는 문자열을 가져와서
    public void inputKey()
    {
        if (!itemUsingState)
        {
            // 중복 사용 방지 변수
            itemUsingState = true;  
            if (Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
            {
                itemUsing(item_1);    
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
            {
                itemUsing(item_2);           
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("jump") && !isSliding && !acting )
            {
                itemUsing(item_3);          
            }     
        }
    }


    // 아이템 사용
    public void itemUsing(string str)
    {
        if (str == "hp_potion") { Recovery("hp_potion"); } 
        else if (str == "mp_potion") { Recovery("mp_potion"); }        
    }






    // 아이템 사용 중 이동 금지
    public void RecoveryAnim()
    {   
        string[] actingStates = { "Recovery Action" };
        acting = System.Array.Exists(actingStates, state => anim.GetCurrentAnimatorStateInfo(0).IsName(state));    
    }



    // 아이템 중복 사용 방지
    public void Preventduplicateuse()
    {
        string[] actingStates = { "Recovery Action" };

        foreach (string state in actingStates)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state))
            {
                itemUsingState = false;
                break;
            }
        }
    }










    // 아이템 목록 ----------------------------------------------------------------------------------------------------------------

    // 회복 애니메이션
    public void Recovery(string type)
    {   
        // hp , mp 구분 변수
        recoveryAnimColor = type;
        
        if (!acting && type == "hp_potion" && itemManager.hp_potion > 0 && (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("walk")))
        { 
            // 사운드
            effectSound.recovery_function();
            // 트리거
            anim.SetTrigger("RecoveryAction");   
            itemManager.hp_potion -= 1;
            playerHp.curHp += 30;
        }
        


        else if(!acting && type == "mp_potion" && itemManager.mp_potion > 0 && (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("walk")))
        {
            // 사운드
            effectSound.recovery_function();
            // 트리거
            anim.SetTrigger("RecoveryAction");       
            itemManager.mp_potion -= 1;
            playerMp.curMp += 30;
        }

        else if ((type == "hp_potion" && itemManager.hp_potion == 0) || (type == "mp_potion" && itemManager.mp_potion == 0))
        {
            effectSound.recoveryFail_function();
        }
    }


    // hp 이펙트 애니메이션
    public void RecoveryEffectAnim()
    {
        if (!spriteRenderer.flipX)
        {
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, 0f, 0f);   
            GameObject effectInstance = Instantiate(RecoveryEffect, transform.position + offset, transform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            
            // 색깔 변경
            if ( recoveryAnimColor ==  "hp_potion")
                effectInstance.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);         // 체력
            else if ( recoveryAnimColor ==  "mp_potion")
                effectInstance.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.67f, 0.89f, 1f); // 마력
        }
                
        else if (spriteRenderer.flipX)
        {
            // 이펙트 위치
            Vector3 offset = new Vector3(0f, 0f, 0f);   
            GameObject effectInstance = Instantiate(RecoveryEffect, transform.position+offset, transform.rotation);
            effectInstance.transform.SetParent(playerTransform);
            effectInstance.GetComponent<SpriteRenderer>().flipX = true;
            
            // 색깔 변경
            if ( recoveryAnimColor ==  "hp_potion")
            effectInstance.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);         // 체력
            else if ( recoveryAnimColor ==  "mp_potion")
            effectInstance.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.67f, 0.89f, 1f); // 마력

        }
    }

}

