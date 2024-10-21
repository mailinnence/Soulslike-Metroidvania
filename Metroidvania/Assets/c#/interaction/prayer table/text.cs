using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class text : MonoBehaviour
{

    [HideInInspector] public TMP_Text textField; // 텍스트 필드

    public void Awake()
    {
        textField = GetComponent<TMP_Text>();
        // 처음에는 렌더러와 애니메이션을 꺼둡니다.
        Deactivate();
    }


    public void Activate()
    {
        textField.enabled = true;
    }


    // 이 함수가 호출될 때 렌더러와 애니메이션을 비활성화합니다.
    public void Deactivate()
    {
        textField.enabled = false;
    }
}
