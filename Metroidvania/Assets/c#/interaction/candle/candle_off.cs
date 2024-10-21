using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candle_off : playerStatManager
{

    
    [Header("페이드 인 앤 아웃 ")]
    public float fadeFloat;
    
    [Header("1 = 페이드 아웃 ")]
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        FadeOut();
        fadeFloat = 2f;
    }


    public void function()
    {
        if(type == 1)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    // 페이드 아웃
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    // 페이드 인
    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }


    // 페이드 아웃 루틴
    private IEnumerator FadeOutRoutine()
    {
        while (fadeFloat > 0f)
        {
            fadeFloat -= 0.5f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
        fadeFloat = 0f; // fadeFloat이 정확히 0이 되도록 설정합니다.

    }


        // 페이드 아웃 루틴
    private IEnumerator FadeInRoutine()
    {
        while (fadeFloat > 0f)
        {
            fadeFloat += 0.5f * Time.deltaTime;
            yield return null; // 한 프레임을 기다립니다.
        }
        fadeFloat = 0f; // fadeFloat이 정확히 0이 되도록 설정합니다.

    }

}
