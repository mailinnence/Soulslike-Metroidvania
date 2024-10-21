using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class choice_move : MonoBehaviour
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

    // 이미지의 Y 위치를 85.5로 이동시키는 함수
    public void MoveToUp()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 87f);
        }
    }

    // 이미지의 Y 위치를 5로 이동시키는 함수
    public void MoveToDown()
    {
        if (targetImage != null)
        {
            RectTransform rectTransform = targetImage.rectTransform;
            Vector2 currentPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPosition.x, 5.5f);
        }
    }




   // 애니메이션 정지 함수
    public void StopAnimation()
    {
        // 애니메이터를 정지시킴
        animator.enabled = false;
    }

}