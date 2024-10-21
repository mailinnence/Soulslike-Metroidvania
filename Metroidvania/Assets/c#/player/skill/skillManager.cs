using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillManager : playerStatManager
{
    // ui 이로 인헤 지정딜 스킬 이름
    // 현재 챕터에서는 변수만 만들어 놓고 ui 파트에서 구현하자.
    
    
    [Header("현재 장착한 스킬")]
    public string skill_name;           // 스킬 이름
    private string skill_ready_type;    // 스킬 준비 모션

    [Header("Mp")]
    public mp playerMp;
    public bool skillAble;              // 스킬 가능 여부
    private int mana;


    [Header("효과음 변수")]
    public effectSound effectSound;


    [Header("스킬 변수")]
    public GameObject debla1;
 



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
        if(!inventory)
        {
            // 스킬 준비 애니메이션
            skillReady_Anim();

            // 스킬 애니메이션 타입 지정
            skill_type(skill_name);

        }
    }




    // 스킬 유형 타입
    public void skill_type(string str)
    {
        // 마법 스킬 준비 
        string[] magic = { "debla" };
        
        // 검술 스킬 준비
        string[] sword = { "Recovery Action" };

        // 전달 받은 스킬이름이 마법 스킬 목록에 있는지 확인
        foreach (string name in magic) { if (str == name) { skill_ready_type = "magic"; } }

        // 전달 받은 스킬이름이 검술 스킬 목록에 있는지 확인
        foreach (string name in sword) { if (str == name) { skill_ready_type = "sword"; } }
    }



 
    
    // 스킬 발동
    public void skillReady_Anim()
    {
        // z키를 누르고 점프 중X 걷는 중X 슬라이딩X 기타 acting x 마법 타입의 스킬일때만 
        if (Input.GetKeyDown(KeyCode.Q) && !anim.GetBool("jump") && !isSliding && !acting && skill_ready_type == "magic" && alive)
        {
            skill_mp_limit(skill_name);
            if (skillAble)
            {
                skill_Invocation(skill_name);
                effectSound.skill_ready_function();
                anim.SetTrigger("skillReady");
            }
            else if(!skillAble)
            {
                // 스킬 불가 효과음 및 추가 애니메이션 추가
            }
        }
    }



    // 스킬 발동
    public void skill_Invocation(string skill_name)
    {
        if (skill_name ==  "debla")
        { 
            Vector3 offset = new Vector3(0f, 3f, 0f);   
            GameObject effectInstance = Instantiate(debla1, transform.position+offset, transform.rotation);
        }
        
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}

    }


    // 스킬별 마력 관리
    public void skill_mp_limit(string skill_name)
    {
        if (skill_name ==  "debla") { mana = 45;}
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}
        else if (skill_name ==  "") {}
        
        // 스킬 가능 여부
        if ( playerMp.curMp - mana >= 0) 
        {
            playerMp.curMp -= mana;
            skillAble = true; 
        }
        else if ( playerMp.curMp - mana < 0) 
        {
            skillAble = false; 
        }

    }


}
