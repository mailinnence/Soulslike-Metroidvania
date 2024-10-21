using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneMove : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    public float F_time = 0.2f;

    public float fadeDuration = 1.5f; // 페이드 인 지속 시간

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }


    public void Fade2()
    {
        StartCoroutine(FadeFlow_2());
    }



    // 켜졌다가 꺼지는 함수 - 시간 간격이 짧음
    IEnumerator FadeFlow()
    {
        // Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(0.6f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        // Panel.gameObject.SetActive(false);
        yield return null;
    }






    // 켜졌다가 꺼지는 함수 - 시간 간격이 김
    IEnumerator FadeFlow_2()
    {
        // Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(4f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        // Panel.gameObject.SetActive(false);
        yield return null;
    }






    //  켜지기만 함
    public void fadeOut_ui()
    {
        time = 0f;
        Color alpha = Panel.color;
        // Panel.gameObject.SetActive(true);
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
        }

        time = 0f;

    }

    // 꺼지기만 함
    public void fadeIn_ui()
    {
        time = 0f;
        Color alpha = Panel.color;
        time = 0f;

        // yield return new WaitForSeconds(0.6f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
        }

    } 


    public void fadeIn_ui_slow()
    {
        StartCoroutine(FadeInUI());
    }

    public void fadeOut_ui_slow()
    {
        StartCoroutine(FadeOutUI());
    }


    private IEnumerator FadeInUI()
    {
        float time = 0f;
        Color alpha = Panel.color;
        alpha.a = 0f; // 초기 알파 값 설정 (투명)
        Panel.color = alpha;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fadeDuration;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        // 알파 값이 1이 정확히 되도록 설정
        alpha.a = 1f;
        Panel.color = alpha;
    }



    private IEnumerator FadeOutUI()
    {
        float time = 0f;
        Color alpha = Panel.color;
        alpha.a = 1f; // 초기 알파 값 설정 (완전히 불투명)
        Panel.color = alpha;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fadeDuration;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        // 알파 값이 0이 정확히 되도록 설정
        alpha.a = 0f;
        Panel.color = alpha;
    }

}
