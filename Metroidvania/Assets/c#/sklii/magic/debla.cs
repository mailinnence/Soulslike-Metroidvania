using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debla : playerStatManager
{
    [Header("debla 범위")]
    // 데이지 처리
    public Transform deblaTransform;      
    public Vector2 deblaVector;        
    // 타깃
    public LayerMask deblaLayer; 

    // 후광 효과
    public Transform deblaColorTransform;
    public Vector2 deblaColorVector;
    

    // 자연스러운 데미지 처리를 위해서 두개의 변수로 데미지 시간을 이용한다.
    private bool deblaAble;
    private bool deblaAbleAnim;
    private bool deblaBackgoundAnim;

    private float startTime;


    [Header("debla 이펙트")]
    public GameObject debla1;
    public GameObject debla2;
    public GameObject debla3;


    [Header("debla 효과음")]
    public effectSound effectSound;
    private GameObject effectSoundGameObject; // Add this line



    void Start()
    {
        startTime = Time.time;
        deblaAbleAnim = true;
        deblaBackgoundAnim = false;

        inition();
    }

    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        effectSoundGameObject = GameObject.Find("Effect"); // Add this line
        effectSound = effectSoundGameObject.GetComponent<effectSound>();
    }


    void Update()
    {
         
        if(deblaAbleAnim) {Hit();}
        
        backgroundColor();

        float elapsedTime = Time.time - startTime;

        // 특정 시간 주기(예: 0.5초)마다 deblaAble을 true로 설정합니다.
        if (elapsedTime >= 0.5f) // 5초 경과 시
        {
            deblaAble = true;
            startTime = Time.time; // 다음 주기를 위해 시작 시간을 갱신합니다.
        }
        else
        {
            deblaAble = false;
        }
    }



    public void debla_explode_ready()
    {
        Vector3 offset = new Vector3(0f, 1.75f, 0f);    
        GameObject effectInstance = Instantiate(debla2, transform.position+offset, transform.rotation); 
    }



    public void debla_explode()
    {
        Vector3 offset = new Vector3(0f, 6.1f, 0f);    
        GameObject effectInstance = Instantiate(debla3, transform.position+offset, transform.rotation); 

        
    }
    




    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(deblaTransform.position , deblaVector);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(deblaColorTransform.position , deblaColorVector);
    }




    void inition()
    {
        // 공격 히트 변수 초기화 ------------------------------------------------------------------------------------
        // 새로운 게임 오브젝트를 생성합니다.
        GameObject deblaRangeObject = new GameObject("deblaRange");
  
        // 새로운 게임 오브젝트를 현재 게임 오브젝트의 자식으로 만듭니다.
        deblaRangeObject.transform.parent = transform;

        // deblaRangeObject의 위치를 조정하여 해당 위치를 기준으로 벡터를 설정합니다.
        deblaRangeObject.transform.localPosition = new Vector3(0f, 0f, 0f); // 원하는 위치로 수정하세요.

        // deblaTransform을 새로 생성한 자식 오브젝트로 초기화합니다.
        deblaTransform = deblaRangeObject.transform;




        // 나머지 변수들을 초기화합니다.
        deblaVector = new Vector2(8.5f, 24f);
        // ------------------------------------------------------------------------------------------------------------       

        



        // 백그라운드 색깔 변수 초기화 ------------------------------------------------------------------------------------
        // 새로운 게임 오브젝트를 생성합니다.
        GameObject deblaRangeObject1 = new GameObject("deblaColorTransform");
  
        // 새로운 게임 오브젝트를 현재 게임 오브젝트의 자식으로 만듭니다.
        deblaRangeObject1.transform.parent = transform;

        // deblaRangeObject의 위치를 조정하여 해당 위치를 기준으로 벡터를 설정합니다.
        deblaRangeObject1.transform.localPosition = new Vector3(0f, 0f, 0f); // 원하는 위치로 수정하세요.

        // deblaTransform을 새로 생성한 자식 오브젝트로 초기화합니다.
        deblaColorTransform = deblaRangeObject1.transform;


        // 나머지 변수들을 초기화합니다.
        deblaColorVector = new Vector2(1000f, 1000f);
        // ------------------------------------------------------------------------------------------------------------ 
    }







    // 공격 판정 함수
    void Hit()
    {
        deblaLayer = 1 << LayerMask.NameToLayer("enemy");
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(deblaTransform.position, deblaVector, 0, deblaLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null && deblaAble)
            {
                objectsToHit[i].GetComponent<enemy_move>().EnemyHit(10f);
                effectSound.debla_hit_function();
            }
        }
        
    }
    




    // 공격 판정 함수
    void backgroundColor()
    {
        // 레이어 이름을 사용하여 결합된 LayerMask 생성
        int targetLayer = LayerMask.GetMask("player" , "NonColider" , "death" , "enemy" , "playerDameged" ,"attackEffect" , "platform");

           // OverlapBoxAll 호출
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(deblaColorTransform.position, deblaColorVector, 0, targetLayer);
        
        for(int i = 0; i < objectsToHit.Length; i++)
        {
            // 각 Collider2D 객체의 Renderer 컴포넌트 가져오기
            Renderer objectRenderer = objectsToHit[i].GetComponent<Renderer>();

            // Renderer 컴포넌트가 존재하면 색깔 변경
            if (objectRenderer != null && deblaBackgoundAnim)
            {

                objectRenderer.material.color = Color.black;
                
                // x축 거리 차이 계산를 계산해서 일정 범위 안에서만 player 색깔 변경
                float distanceX = Mathf.Abs(objectsToHit[i].transform.position.x - deblaColorTransform.position.x);

                if (distanceX < 4.5 )   { objectRenderer.material.color = Color.black; }
                if (distanceX >=  4.5 ) { objectRenderer.material.color = Color.white; }
                
            }
        }

        if(!deblaBackgoundAnim)
        {
            for(int i = 0; i < objectsToHit.Length; i++)
            {
                // 각 Collider2D 객체의 Renderer 컴포넌트 가져오기
                Renderer objectRenderer = objectsToHit[i].GetComponent<Renderer>();
                objectRenderer.material.color = Color.white;
                
            }
        }
    }



    // hit 함수 활성화 여부
    public void deblaAbleAnimOff()
    {
        deblaAbleAnim = false;
    }


    // 후광 효과 적용on
    public void backgroundColorOn()
    {
        deblaBackgoundAnim = true;
    }

    // 후광 효과 적용off
    public void backgroundColorOff()
    {
        deblaBackgoundAnim = false;
    }


}



// new Color(62, 133, 172); 화이트