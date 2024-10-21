using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer; // 방향전환
    [HideInInspector] public Animator anim; // 애니메이션

    public player p;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 처음에는 렌더러와 애니메이션을 꺼둡니다.
        DeactivateBloodEffect();
    }

    // 이 함수가 호출될 때 렌더러와 애니메이션을 활성화합니다.
    public void ActivateBloodEffect(float x, float y)
    {
        // 현재 오브젝트의 스케일을 가져옵니다.
        Vector3 newScale = new Vector3(x, y, 1f);

       if (p.spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        } 
        else 
        {
            spriteRenderer.flipX = false;
        }

        // 변경된 스케일을 적용합니다.
        transform.localScale = newScale;

        // 나머지 코드는 그대로 유지됩니다.
        spriteRenderer.enabled = true;
        anim.enabled = true;
    }


    // 이 함수가 호출될 때 렌더러와 애니메이션을 비활성화합니다.
    public void DeactivateBloodEffect()
    {
        spriteRenderer.enabled = false;
        anim.enabled = false;
    }
}