using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterDelay : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 0.5초 뒤에 스프라이트를 비활성화하는 코루틴을 시작합니다.
        StartCoroutine(HideSpriteAfterDelay(0.5f));
    }

    IEnumerator HideSpriteAfterDelay(float delay)
    {
        // 지정한 시간(0.5초) 동안 대기합니다.
        yield return new WaitForSeconds(delay);

        // 스프라이트 렌더러를 비활성화합니다.
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
