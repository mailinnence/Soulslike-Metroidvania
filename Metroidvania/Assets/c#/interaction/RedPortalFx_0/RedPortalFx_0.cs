using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class RedPortalFx_0 : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;   



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

    public bool again_pickUp;
    
    public int progress;


    [Header("json")]
    public coordinate coordinate;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        textAble = true;
    }



    // Start is called before the first frame update
    void Start()
    {
        portal_start();
    }

    // Update is called once per frame
    void Update()
    {
        portal_init();  

        if(progress == 3)
        {
            DescriptionText(interactionArea,  interactionArea_);
            knee_down(interactionArea, interactionArea_ );
        }

        if(progress == 7)
        {
            DescriptionText(interactionArea,  interactionArea_);
            knee_down(interactionArea, interactionArea_ );
        }


    }

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeTo(1.0f, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeTo(0.0f, duration));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);


        
        waiting_end_delay();
    }



    // 플레이어 waiting_end
    public void waiting_end_delay()
    {
        StartCoroutine(waiting_end_delay_());
    }


 
    // 2초 후 객체를 삭제하는 코루틴
    IEnumerator waiting_end_delay_()
    {
        yield return new WaitForSeconds(1f);
        interaction_object.waiting_end();
        again_pickUp = true;

    }



    public void again_pickUp_()
    {
        if (again_pickUp)
        {
            interaction_object.again_pickUp();
            again_pickUp = false;
        }
    }




    // 진행도에 따라 포털 활성와
    public void portal_init()
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


            if (playerData.Progress == 3)
            {
                progress = playerData.Progress;
            }


            if (playerData.Progress == 7)
            {
                progress = playerData.Progress;
            }
        }
    }


    // 만약 포털 전에 게임을 끌 경우
    public void portal_start()
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


            if (playerData.Progress == 3)
            {
                StartCoroutine(FadeTo(1.0f, 1.0f));
            }

            if (playerData.Progress == 7)
            {
                StartCoroutine(FadeTo(1.0f, 1.0f));
            }
        }
    }



    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
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





    // 포털 애니메이션
    void knee_down(Transform interactionArea, Vector2 interactionArea_ )
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
        SceneWhite.Fade_White_In();
        Invoke("scene_Move", 3f);
    }


    
    // 씬 이동
    public void scene_Move()
    {
        if (progress == 3)
        {
            SceneManager.LoadScene("1_1"); // SceneManager를 사용하여 씬 로드
        }

        else if(progress == 7)
        {
            coordinate.save_coordinate(378.78f , -47.68f);
            SceneManager.LoadScene("2_1"); // SceneManager를 사용하여 씬 로드
        }

    }
















        // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }



}
