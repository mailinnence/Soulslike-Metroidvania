using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class event_background : MonoBehaviour
{
    [Header("UI")]
    public Image white_background;
    public Image black_background;

    [Header("Sound")]
    public effectSound effectSound;


    // 코루틴 내에서 시간의 경과에 따라 증가하며, Lerp 함수의 보간값으로 사용되어 알파값을 부드럽게 변화시킴
    private float time = 0f;
    private float alpha;

    // 페이드 효과의 속도를 제어
    // 값이 작을수록 페이드 효과가 빠르게 진행되고, 값이 클수록 느리게 진행
    // public float F_time = 0.2f;

    // 페이드 인 효과의 총 지속 시간을 정의
    // 페이드 인 효과의 전체 duration을 설정하는 데 사용
    // 즉 켜져있거나 어두워져 있는 시간
    // public float fadeDuration = 1.5f; 


    // Start is called before the first frame update
    void Start()
    {
        alpha = 1f;

        // Fade_white(1.5f , 1.5f);    // 적당함
        // Fade_black(1.5f , 1.5f);    // 적당함
        // Fade_white_In(1.5f , 1.5f);
        // Fade_white_out(1.5f , 1.5f);
        // Fade_black_In(1.5f , 1.5f);
        // Fade_black_out(3f , 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // 흰색 배경이 켜졌다 꺼지는 함수
    public void Fade_white(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_white_Flow(F_time , fadeDuration));
    }


    // 흰색 배경이 켜지는 함수
    public void Fade_white_In(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_white_In_( F_time , fadeDuration));
    }


    // 처음 화면이 희게 나와서 서서히 fade out되는 함수
    public void Fade_white_out(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_white_out_( F_time , fadeDuration));
    }






    // 검은 배경이 켜졌다 꺼지는 함수
    public void Fade_black(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_black_Flow(F_time , fadeDuration));
    }


    // 검은 배경이 켜졌다 꺼지는 함수
    public void Fade_black_In(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_black_In_(F_time , fadeDuration));
    }

    // 처음 화면이 검게 나와서 서서히 밝아지는 함수
    public void Fade_black_out(float F_time , float fadeDuration)
    {
        StartCoroutine(Fade_black_out_(F_time , fadeDuration));
    }






    // 켜졌다가 꺼지는 함수
    IEnumerator Fade_white_Flow(float F_time , float fadeDuration)
    {
        time = 0f;
        Color alpha = white_background.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            white_background.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(fadeDuration);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            white_background.color = alpha;
            yield return null;
        }

        yield return null;
    }






    // 화면이 하얗게 바뀌기만 하는 함수
    IEnumerator Fade_white_In_(float F_time , float fadeDuration)
    {
        float time = 0f;
    
        while (time < F_time)
        {
            time += Time.deltaTime;
            float t = time / F_time;
            alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha_white(alpha);
            yield return null;
        }

        SetAlpha_white(1f);
    }

    void SetAlpha_white(float alpha)
    {
        Color newColor = white_background.color;
        newColor.a = alpha;
        white_background.color = newColor;
    }






    // 처음 화면이 검게 나와서 서서히 밝아지는 함수
    IEnumerator Fade_white_out_(float F_time, float fadeDuration)
    {
        float time = 0f;
        SetAlpha_white_out(1); // 시작할 때 완전히 검은색으로 설정
        
        while (time < F_time)
        {
            time += Time.deltaTime;
            float t = time / F_time;
            float alpha = Mathf.Lerp(1f, 0f, t); // 알파값을 1에서 0으로 변경
            SetAlpha_white_out(alpha);
            yield return null;
        }

        SetAlpha_white_out(0f); // 완전히 투명하게 설정하여 마무리
    }

    void SetAlpha_white_out(float alpha)
    {
        Color newColor = white_background.color;
        newColor.a = alpha;
        white_background.color = newColor;
    }











    // 켜졌다가 꺼지는 함수 
    IEnumerator Fade_black_Flow(float F_time , float fadeDuration)
    {
        time = 0f;
        Color alpha = black_background.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            black_background.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(fadeDuration);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            black_background.color = alpha;
            yield return null;
        }

        yield return null;
    }




    // 화면이 검게 바뀌기만 하는 함수
    IEnumerator Fade_black_In_(float F_time , float fadeDuration)
    {
        float time = 0f;
    
        while (time < F_time)
        {
            time += Time.deltaTime;
            float t = time / F_time;
            alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha_black(alpha);
            yield return null;
        }

        SetAlpha_black(1f);
    }



    void SetAlpha_black(float alpha)
    {
        Color newColor = black_background.color;
        newColor.a = alpha;
        black_background.color = newColor;
    }



    // 처음 화면이 검게 나와서 서서히 밝아지는 함수
    IEnumerator Fade_black_out_(float F_time, float fadeDuration)
    {
        float time = 0f;
        SetAlpha_black_out(1); // 시작할 때 완전히 검은색으로 설정
        
        while (time < F_time)
        {
            time += Time.deltaTime;
            float t = time / F_time;
            float alpha = Mathf.Lerp(1f, 0f, t); // 알파값을 1에서 0으로 변경
            SetAlpha_black_out(alpha);
            yield return null;
        }

        SetAlpha_black_out(0f); // 완전히 투명하게 설정하여 마무리
    }

    void SetAlpha_black_out(float alpha)
    {
        Color newColor = black_background.color;
        newColor.a = alpha;
        black_background.color = newColor;
    }

}
