using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 스프라이트 랜더러 컴포넌트
    private BoxCollider2D boxCollider; // BoxCollider2D 컴포넌트


    AudioSource audioSource;


    void Start()
    {
        // 스프라이트 랜더러와 BoxCollider2D 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 캐릭터와 충돌했을 때만 처리합니다.
        if (other.CompareTag("Player"))
        {
            // BoxCollider2D를 비활성화하고 1초 뒤에 활성화합니다.
            boxCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(1f));
        }
    }

    // 지연 시간이 지난 후에 BoxCollider2D를 활성화하는 Coroutine
    IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 스프라이트 렌더러를 0.5초 동안 활성화하고 다시 비활성화합니다.
        spriteRenderer.enabled = true;
        audioSource.Play();
        yield return new WaitForSeconds(30.2f);
        spriteRenderer.enabled = false;

        // BoxCollider2D를 활성화합니다.
        boxCollider.enabled = true;
    }
}
