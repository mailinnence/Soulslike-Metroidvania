using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_finish_background : MonoBehaviour
{
    public float radius = 5f; // 색을 변경할 범위의 반경
    public LayerMask targetLayers; // 대상 오브젝트들의 레이어

    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();


    public void ChangeColorToBlackInRange()
    {
        // 지정된 반경 내의 모든 콜라이더를 찾습니다
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, targetLayers);

        foreach (Collider2D collider in colliders)
        {
            // 각 콜라이더의 게임오브젝트에서 SpriteRenderer 컴포넌트를 가져옵니다
            SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // SpriteRenderer가 있다면 색을 검은색으로 변경합니다
                spriteRenderer.color = Color.black;
            }
        }
    }




    public void RestoreOriginalColors()
    {
        foreach (var kvp in originalColors)
        {
            SpriteRenderer spriteRenderer = kvp.Key;
            Color originalColor = kvp.Value;

            if (spriteRenderer != null)
            {
                // 원래 색상으로 복원
                spriteRenderer.color = originalColor;
            }
        }
        // 딕셔너리 초기화
        originalColors.Clear();
    }



    // 디버그를 위해 범위를 시각화합니다 (에디터에서만 보임)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}


