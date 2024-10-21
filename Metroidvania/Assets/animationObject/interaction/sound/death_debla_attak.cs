using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_debla_attak : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;



    void Start()
    {
        // 스프라이트 렌더러를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }



    void off_collider()
    {
        boxCollider2D.enabled = false;
    }



    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            energyHp playerEnergyHp = collider.GetComponent<energyHp>();
            if (playerEnergyHp != null)
            {
                playerEnergyHp.monster_attack_lv1(spriteRenderer.flipX, 9999);
            }
        }
    }
}
