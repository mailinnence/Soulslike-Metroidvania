using UnityEngine;
using TMPro;

public class TextColor : MonoBehaviour
{
    public TMP_Text textMeshPro;

    void Start()
    {
        // TextMeshPro 컴포넌트가 할당되지 않았다면 자동으로 찾습니다.
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }
    }

    // 텍스트 색상을 흰색(#FFFFFF)으로 변경하는 함수
    public void ChangeToWhite()
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = Color.white; // Unity에서 제공하는 흰색 상수 사용
        }
    }

    // 텍스트 색상을 밝은 노란색(#F9E902)으로 변경하는 함수
    public void ChangeToBrightYellow()
    {
        if (textMeshPro != null)
        {
            // ColorUtility를 사용하여 16진수 색상 코드를 Color로 변환
            Color brightYellow;
            if (ColorUtility.TryParseHtmlString("#F9E902", out brightYellow))
            {
                textMeshPro.color = brightYellow;
            }
        }
    }
}