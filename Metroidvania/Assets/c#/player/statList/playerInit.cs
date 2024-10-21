using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // File.WriteAllText 사용을 위해 추가

public class playerInit : playerStatManager
{


    [System.Serializable]
    public class SceneData
    {
        // 씬과 세이브존 위치
        public string save_Scene;
        public string save_Location;

        // 현재 에너지
        public int hp;
        public int mp;

        // 현재 포션 위치
        public int hpPotion;
        public int mpPotion;

        // 이벤트 아이템 리스트
        public string[] event_Item;

        // 위치 이벤트 처리 
        public int startZone;

    }



    // 이벤트 아이템 리스트를 저장할 변수
    private List<string> eventItemList;

    void Start()
    {
        init_item();
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    public void init_item()
    {
        // JSON 파일 로드
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("SceneSave");

        // JSON 파싱
        SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonTextAsset.text);

        // event_Item을 List<string>으로 변환
        if (sceneData.event_Item != null)
        {
            eventItemList = new List<string>(sceneData.event_Item);
        }
        else
        {
            eventItemList = new List<string>(); // 빈 리스트로 초기화
        }

        // 데이터 출력
        // foreach (string item in eventItemList)
        // {
        //     Debug.Log("Event Item: " + item);
        // }

        // 나머지 데이터도 초기화가 필요하면 여기에 추가
        // Debug.Log("Scene: " + sceneData.save_Scene);

    }




    public void save_Scene_function(string json)
    {
        // JSON 파일 로드
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("SceneSave");
        if (jsonTextAsset != null)
        {
            // JSON 파싱
            SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonTextAsset.text);

            // 값 수정
            sceneData.save_Scene = json;

            // 수정된 JSON 다시 문자열로 변환
            string modifiedJson = JsonUtility.ToJson(sceneData, true);

            // 필요한 경우 수정된 JSON을 파일로 저장
            File.WriteAllText("Assets/Resources/SceneSave.json", modifiedJson);
            
        }
    }





    // 화면마다 변수 초기화
    public void SceneInit()
    {
        // bool 값 처리
        damaged = false;  
        damagedMove = false;
        jumpAble = true;                   
        jump_verticalattack = false;       
        jump_ghost = false;         
        slidingExcept = false;    
        isSliding = false;     
        isSliding_stabbing = false;       
        stabbing_ = false;    
        airdooring = false;
        acting = false;
        hurt = false;
        attacking = false;                      
        attackAble = true;                           
        itemUsingState = false;                  
        textAble = false;                      
        ignoreButton = false;
        hangAble = false;
        parrying_action = false;
        parrying_counter = false;

        // 애니메이션 변수 초기화
        // anim.SetBool("walk" , false);
        anim.SetBool("jump" , false);
        anim.SetBool("jump2" , false);
        anim.SetBool("jump_falling" , false);
        anim.SetBool("crouchDown" , false);
        // anim.SetBool("crouchUp" , true);
        anim.SetBool("hang" , false);
        anim.SetBool("wallclimbing" , false);
        anim.SetBool("wallclimbing_jump" , false);
        anim.SetBool("charging_shot" , false);

        // 트리거 초기화        



    }
}
