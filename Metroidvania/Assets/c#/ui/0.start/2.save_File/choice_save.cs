using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class choice_save : MonoBehaviour
{
    public Image targetImage; // 이동시킬 UI 이미지

    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

        // 만약 Inspector에서 이미지를 할당하지 않았다면 자동으로 찾습니다.
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
    
    }


    public void MoveTo1()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 225f);
        }
    }

    public void MoveTo2()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 33f);
        }
    }

    public void MoveTo3()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, -159f);
        }
    }




   // 애니메이션 정지 함수
    public void StopAnimation()
    {
        // 애니메이터를 정지시킴
        animator.enabled = false;
    }

}