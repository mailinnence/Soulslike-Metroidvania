using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class boss_hp_2 : playerStatManager
{
    [Header("캐릭터 hp 관련 변수")]
    public Slider hpbar;
    public float maxHp;
    public float curHp;
    private float imsi;

    public Image background;

    public maito maito;
    public maito_white_back maito_white_back;
    public maito_blck_object body;
    public maito_blck_object eye;
    public boss2 boss2;
    public SceneMove SceneMove;
    public tile_map_color2 tile_map_color;
    public eye_off eye_off;


    public energyHp energyHp;



    public float slowMotionFactor = 0; // 느린 속도 배율 (0.5는 반속도)

    // 배경 관련 변수
    public float fadeDuration = 4f;
    public float displayDuration = 4f;

    public bool once_var;
    public bool once_var2;
    public bool once_var3;

    private bool object_off_;

    // Start is called before the first frame update
    void Start()
    {
        hpbar.value = (float) curHp / (float) maxHp;
    
    }

    // Update is called once per frame
    void Update()
    {
   
        imsi = (float) curHp / (float) maxHp;
        HandleHp();


        if(curHp <=0)
        {
            

            boss_2 = false;
            maito.collider_off();  // 사망시 콜라이도 제거 - 이 보스는 사망 모션이 없음

            // 시간 제어
            StartCoroutine(SlowDownForOneSecond());
            
            // 화면 제어
            StartCoroutine(boss_clear_background());
            
            // 클리어 로고 제어
            if(!once_var) { StartCoroutine(boss_clear_logo()); }
        }


        if(curHp <= 600 && !once_var2)
        {
            once_var2 = true;
            maito.page_2_pattern();
        }
        

                // 한번이라고 !boss_2 가 나오면 발생 
        if (curHp <= 0) 
        {
            object_off_ = true;

        }

        if(object_off_)
        {
            curHp = 1;
            boss2.object_off();
        }
 
           
        if (Input.GetKeyDown(KeyCode.M))
        {
            curHp = 0;
        }


        if(!alive && !once_var3)
        {
            StartCoroutine(death_scene());
        }

    }






    IEnumerator death_scene()
    {
        once_var3 = true;
        yield return new WaitForSecondsRealtime(6f); 
        player_setting_init();
        SceneManager.LoadScene("2_1");
        player_setting_init();
    }



    

    public void init()
    {
        maxHp = 1200;
        curHp = 1200;
    }

    public void boss_damaged(float _damageDone)
    {
        curHp -= _damageDone;
    }



    private void HandleHp()
    {
        hpbar.value = Mathf.Lerp(hpbar.value , imsi , Time.deltaTime * 10);
    }




    IEnumerator SlowDownForOneSecond()
    {
        Time.timeScale = 0f; // 게임 속도 멈추기
        maito_white_back.white_background();
        body.Set_black();
        eye.Set_black();


        yield return new WaitForSecondsRealtime(1.5f); // 3초 동안 유지
        
        eye_off.isFadingOut_= true;
        body.Set_white();
        eye.Set_white();
        Time.timeScale = 1f; // 원래 속도로 복귀
    }







    // 백그라운드가 화이트 앤 블랙으로 바뀜
    IEnumerator boss_clear_background()
    {
        boss_clear_2 = true; 
        tile_map_color.Background_Out();
        yield return new WaitForSecondsRealtime(1f); 
        curHp = 10;
        boss_clear_2 = false;  
        tile_map_color.Background_In();
    }


    // 보스 클리어 로고
    IEnumerator boss_clear_logo()
    {
        once_var = true;
        yield return new WaitForSecondsRealtime(8f); 
        alert();            // 로고 백그라운드
        SceneMove.Fade2();  // 로고

        // 8초후 다음씬으로
        yield return new WaitForSecondsRealtime(8f); 
        // SceneManager.LoadScene("2-3");
    
    }





    // 로고 백그라운드
    public void alert()
    {
        StartCoroutine(FadeInOutSequence());
    }



    

    IEnumerator FadeInOutSequence()
    {
        // 초기 알파값을 0으로 설정
        SetAlpha(0f);

        // 페이드인
        yield return StartCoroutine(Fade(0f, 0.4f)); // 최대 알파값을 0.5로 변경

        // 표시 지속 시간
        yield return new WaitForSeconds(displayDuration);

        // 페이드아웃
        yield return StartCoroutine(Fade(0.4f, 0f));
    }



    // // 다음씬으로 넘어가기 위해서 화면이 까맣게 바뀜
    // IEnumerator FadeOut_next_scene()
    // {
    //     // 초기 알파값을 0으로 설정
    //     SetAlpha(0f);

    //     // 페이드인
    //     yield return StartCoroutine(Fade(0f, 1f)); // 최대 알파값을 0.5로 변경

    //     // 표시 지속 시간
    //     yield return new WaitForSeconds(3f);
    //     SceneManager.LoadScene("1_2");
    // }



    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    void SetAlpha(float alpha)
    {
        Color newColor = background.color;
        newColor.a = alpha;
        background.color = newColor;
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