using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye_off : MonoBehaviour
{
    [Header("페이드 인 앤 아웃 ")]

    public bool isFadingOut_; // 페이드 방향 제어
    public float fadeFloat_eye ;
    public float fadeSpeed_eye;
    
    public SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if(isFadingOut_)
        {
            FadeOut();
        }

        // spriteRenderer의 알파 값을 업데이트
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = fadeFloat_eye;
            spriteRenderer.color = color;
        }

    }


    public void FadeOut()
    {
 
        fadeFloat_eye -= fadeSpeed_eye * Time.deltaTime;
        if (fadeFloat_eye <= 0f)
        {
            fadeFloat_eye = 0f;
        }
    }


}
