using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundController3 : MonoBehaviour
{
    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 

    private float startPos;
    public GameObject cam;
    public float parallaxEffect; // The speed at which the background should move relative to the camera
    public float empty;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position.x;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>(); 
        }
       
    }

    void FixedUpdate()
    {
        // Calculate distance background move based on cam movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = move with cam || 1 = won't move || 0.5 = half

        transform.position = new Vector3(startPos + distance + empty, transform.position.y, transform.position.z);

        // Call DescriptionText method
        DescriptionText(interactionArea, interactionArea_);
    }

    // 설명 텍스트 여부
    void DescriptionText(Transform interactionArea, Vector2 interactionArea_)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);



        if (objectsToHit.Length <= 0)
        {
            // 타일맵이 꺼짐
            SetTilemapVisibility(false);
        }
        else
        {
            // 타일맵이 켜짐
            SetTilemapVisibility(true);
        }
    }

    // 타일맵 가시성을 설정하는 메서드
    private void SetTilemapVisibility(bool isVisible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible;
        }
    }

    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position, interactionArea_);
    }
}