using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class pedestal : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;   


    [Header("이벤트 알림")]
    public item_alert item_alert;


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


    // 진행도
    private int progress;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        textAble = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress_init();
        if(progress == 2)
        {
            DescriptionText(interactionArea,  interactionArea_);
            pickDown(interactionArea, interactionArea_ );
        }

        else if(progress == 5)
        {
            DescriptionText(interactionArea,  interactionArea_);
            pickDown(interactionArea, interactionArea_ );
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





    // 아이템 픽업 애니메이션 
    void pickDown(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);


        if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && !playerStatManager.acting)
        {
            textAble = false;
            Vector3 currentPosition = transform.position;
            interaction_object.pickDown_Anim(currentPosition);
       
        }
    }




    // 진행도 초기화 함수
    public void progress_init()
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


            progress = playerData.Progress;
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


}
