using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction : prayerTableManager
{
    // region_A1 로 가정

    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;                   


    [Header("해당 기도대 위치")]
    public bool location;


    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    [Header("상호작용 설명 텍스트")]
    public textBackground textBackground;
    public text text;
    public bool textAble;                      // 설명 텍스트 문 동작 여부


    [Header("상호작용 player 애니메이션")]
    public interaction_object interaction_object;
    private bool praying = false;


    [Header("hp , mp 아이템 초기화 및 기타")]
    public itemManager itemManager; 
    public hp playerHp;
    public mp playerMp;
    public playerStatManager playerStatManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        textAble = true;
    }


    void Start()
    {
        
    }


    void Update()
    {   
        // 지역 지정
        // region_A1 = location;

    
        DescriptionText(interactionArea,  interactionArea_);
        table_Activation(interactionArea, interactionArea_ );
        animState();


        // 기도대에서 일어나기
        knee_Up_Anim();
    }


    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }


    // 기도대의 활성화 여부에 따라 애니메이션 결정
    void animState()
    {
        if(region_A1)
        {
            anim.SetBool("Activation" , true);
        }
        else if(!region_A1)
        {
            anim.SetBool("Activation" , false);
        }
    }






    // 설명 텍스트 여부
    void DescriptionText(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if(textAble)
        {

            if (objectsToHit.Length >=1)
            {
                textBackground.Activate();
                text.Activate();
            }
            else
            {
                textBackground.Deactivate();
                text.Deactivate();
            }
        }
        else
        {
            textBackground.Deactivate();
            text.Deactivate();
        }
    }

    // 애니메이션 중에는 설명 텍스트가 뜰 필요가 없음
    void textAble_on()
    {
        textAble = true;
    }




    // 기도대 활성화 및 플레이어 애니메이션 
    void table_Activation(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        // 기도대가 활성화 되지 않았을 때
        if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && !location && !playerStatManager.acting)
        {
            textAble = false;

            interaction_object.Activation_Anim();
            Invoke("locationTime" , 1.22f);
            Invoke("textAble_on" , 2f);
            
        }

        // 기도대가 활성화 되었을때
        else if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && location && !playerStatManager.acting)
        {
            // 아이템을 초기화 한다.
            itemManager.hp_potion = 5;
            itemManager.mp_potion = 5;
            
            // 기도대 휴식 애니메이션
            interaction_object.knee_pray_Anim();

            // hp , mp 회복
            playerHp.curHp = 100;
            playerMp.curMp = 100;

            // 텍스트 , 애니메이션
            textAble = false;
            Invoke("prayTrueDelay" , 2f);
        }
    }



    // 중간에 UI 가 나와야 함으로 일단 여기까지만 구현한다.
    void knee_Up_Anim()
    {
        if(Input.GetKey(KeyCode.Escape) && praying)
        {
            interaction_object.knee_Up_Anim();
            Invoke("textAble_on" , 0.05f);
            praying = false;
        } 
    }




    void locationTime()
    {
        location = true;
    }


    
    void prayTrueDelay()
    {
        praying = true;
    }


}
