using System.Collections;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float fadeTime = 0.5f;
    private bool isFadedIn = false;

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
        yield return StartCoroutine(FadeCoroutine(1f, 0f));
        isFadedIn = false;
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color currentColor = textMeshPro.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            currentColor.a = alpha;
            textMeshPro.color = currentColor;
            yield return null;
        }

        currentColor.a = endAlpha;
        textMeshPro.color = currentColor;
    }
}