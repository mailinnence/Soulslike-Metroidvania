using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class boss_hp_1 : playerStatManager
{
    [Header("캐릭터 hp 관련 변수")]
    public Slider hpbar;
    public float maxHp;
    public float curHp;
    private float imsi;

    public Image background;

    public boss boss;
    public SceneMove SceneMove;
    public tile_map_color tile_map_color;



    public float slowMotionFactor = 0; // 느린 속도 배율 (0.5는 반속도)

    // 배경 관련 변수
    public float fadeDuration = 4f;
    public float displayDuration = 4f;

    public bool once_var;
    public event_background event_background;
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
            boss_1 = false;
            boss.all_dead();  // 혹시나 보스가 죽지 않는 상황 제거
            StartCoroutine(SlowDownForOneSecond());
            StartCoroutine(boss_clear_background());
            
            if(!once_var) { StartCoroutine(boss_clear_logo()); }
        }
    }

    public void init()
    {
        maxHp = 800;
        curHp = 800;
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
        yield return new WaitForSecondsRealtime(1.5f); // 3초 동안 유지
        Time.timeScale = 1f; // 원래 속도로 복귀
    }


    // 백그라운드가 화이트 앤 블랙으로 바뀜
    IEnumerator boss_clear_background()
    {
        boss_clear = true; 
        tile_map_color.Background_Out();
        yield return new WaitForSecondsRealtime(1f); 
        boss_clear = false; 
        tile_map_color.Background_In();
    }


    // 보스 클리어 로고
    IEnumerator boss_clear_logo()
    {
        once_var = true;
        yield return new WaitForSecondsRealtime(4f); 
        alert();            // 로고 백그라운드
        SceneMove.Fade2();  // 로고

        // 8초후 다음씬으로
        yield return new WaitForSecondsRealtime(8f); 
        next_scene_();
    }





    // 로고 백그라운드
    public void alert()
    {
        StartCoroutine(FadeInOutSequence());
    }



    // 로고 백그라운드
    public void next_scene_()
    {
        StartCoroutine(FadeOut_next_scene());
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



    // 다음씬으로 넘어가기 위해서 화면이 까맣게 바뀜
    IEnumerator FadeOut_next_scene()
    {
        event_background.Fade_black_In(1.5f , 1.5f);
        // 표시 지속 시간
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("1_2");
    }



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










}