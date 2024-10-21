using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ui_location : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Image image1;
    public Image image2;
    public float fadeDuration = 1f;
    public float displayDuration = 1f;

    void Start()
    {

    }

    public void alert()
    {
        StartCoroutine(FadeInOutSequence());
    }


    IEnumerator FadeInOutSequence()
    {
        // 초기 알파값을 0으로 설정
        SetAlpha(0f, 0f);

        // 페이드인
        yield return StartCoroutine(Fade(0f, 1f, 0f, 63f/255f));

        // 표시 지속 시간
        yield return new WaitForSeconds(displayDuration);

        // 페이드아웃
        yield return StartCoroutine(Fade(1f, 0f, 63f/255f, 0f));
    }

    IEnumerator Fade(float startAlpha1, float endAlpha1, float startAlpha2, float endAlpha2)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float alpha1 = Mathf.Lerp(startAlpha1, endAlpha1, t);
            float alpha2 = Mathf.Lerp(startAlpha2, endAlpha2, t);
            SetAlpha(alpha1, alpha2);
            yield return null;
        }

        SetAlpha(endAlpha1, endAlpha2);
    }

    void SetAlpha(float alpha1, float alpha2)
    {
        textMesh.alpha = alpha1;
        image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, alpha1);
        image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, alpha2);
    }
}