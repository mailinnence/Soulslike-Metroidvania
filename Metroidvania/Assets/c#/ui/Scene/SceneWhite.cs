using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneWhite : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 2f;


    void Start()
    {
        
    }




    public void Fade_White_In()
    {
        StartCoroutine(FadeIn());
    }


    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }





    // Flow
    IEnumerator FadeIn()
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
        yield return null;
    }








    // Flow
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


}
