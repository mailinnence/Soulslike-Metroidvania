using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ObjectFade : MonoBehaviour
{
    public ui_Sound ui_Sound;
    public Image panel;
    public float fadeTime = 0.5f;
    private bool isFadedIn = false;
    private bool sound = false;         // 소리는 한번만 나야한다.

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        // 아무 버튼이나 누르면 fade out
        if (isFadedIn && Input.anyKeyDown)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeCoroutine(0f, 1f));
        isFadedIn = true;
    }

    IEnumerator FadeOut()
    {
        // 사운드 : 한번만 나도록 변수처리
        if(!sound) 
        { 
            ui_Sound._Relic_function();
            sound = true;
        }

        yield return StartCoroutine(FadeCoroutine(1f, 0f));
        isFadedIn = false;
        SceneManager.LoadScene("1.menu_ui");
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {  

        float elapsedTime = 0f;
        Color currentColor = panel.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            currentColor.a = alpha;
            panel.color = currentColor;
            yield return null;
        }

        currentColor.a = endAlpha;
        panel.color = currentColor;
    }




}