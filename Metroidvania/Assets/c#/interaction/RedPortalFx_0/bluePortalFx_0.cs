using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class bluePortalFx_0 : playerStatManager
{
    [Header("페이드 인 앤 아웃 ")]
    public float fadeFloat;
    private bool activation;


    [HideInInspector] public SpriteRenderer portalSpriteRenderer;      
    [HideInInspector] public Animator anim_;   

    public event_background event_background;

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


    [Header("화이트 백")]
    public SceneWhite SceneWhite;
    public effectSound effectSound;



    void Awake()
    {
        portalSpriteRenderer = GetComponent<SpriteRenderer>();
        anim_ = GetComponent<Animator>();

        textAble = true;
    }



    void Update()
    {
        candle_init();
        portalSpriteRenderer.color = new Color(1, 1, 1, fadeFloat); 

        if(activation)
        {
            DescriptionText();
            knee_down();
        }
    }

    // 페이드 아웃
    public void FadeOut()
    {
        StartCoroutine(FadeOut_delay());
    }



    // 페이드 아웃 루틴
    private IEnumerator FadeOut_delay()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOutRoutine());
        
    }

    // 페이드 아웃 루틴
    private IEnumerator FadeOutRoutine()
    {
        while (fadeFloat <= 1f)
        {
            fadeFloat += 0.005f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
    }






    // 이미 끝 촛불은 시작시 꺼져야 하고 해당되는 그림은 바뀌어야 한다.
    public void candle_init()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");

        string currentPlayerJson = File.ReadAllText(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = File.ReadAllText(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

            // 오브젝트의 위치로 설명 텍스트 판단
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            
            
            if (playerData.candle.Contains(1) && playerData.candle.Contains(2) && playerData.candle.Contains(3))
            {
                FadeOut();
                activation = true;
            }



            // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
            string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerPath, updatedPlayerJson);
        }
    }


    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }


    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }






    // 포탈 이동
    
    // 설명 텍스트 여부
    void DescriptionText()
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





    // 포털 애니메이션
    void knee_down()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);


        if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && !playerStatManager.acting)
        {
            textAble = false;
            Vector3 currentPosition = transform.position;
            interaction_object.kneeDonw_Anim(currentPosition);
            Invoke("Fade_White_In_", 2f);  // 2초 후 Fade_White_In_ 메소드 호출
            
        }
    }

    void Fade_White_In_()
    {
        effectSound.MIRIAM_PORTAL_CHALLENGE_function();
        // SceneWhite.Fade_White_In();
        event_background.Fade_white_In(1.5f , 1.5f);
        Invoke("scene_Move", 3f);
    }


    
    // 씬 이동
    public void scene_Move()
    {
        player_setting_init();
        SceneManager.LoadScene("2_2"); // SceneManager를 사용하여 씬 로드
        
    }




    public void player_setting_init()
    {
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
