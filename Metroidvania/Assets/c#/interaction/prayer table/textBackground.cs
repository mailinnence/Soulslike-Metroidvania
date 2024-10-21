using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textBackground : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer; // 방향전환
    [HideInInspector] public TMP_Text textField; // 텍스트 필드

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // TextMeshPro의 Text 컴포넌트 가져오기
        textField = GetComponent<TMP_Text>();
        // 처음에는 렌더러와 애니메이션을 꺼둡니다.
        Deactivate();
    }


    public void Activate()
    {
        spriteRenderer.enabled = true;
    }


    // 이 함수가 호출될 때 렌더러와 애니메이션을 비활성화합니다.
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
    }
}
