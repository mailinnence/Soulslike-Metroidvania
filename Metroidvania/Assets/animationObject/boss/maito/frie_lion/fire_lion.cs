using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_lion : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D CapsuleCollider;    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        if (energyHpComponent != null)
        {
            energyHpComponent.death_trigger_shake();
            energyHpComponent.playerHp.curHp = 0;
        }
    }

    public void CapsuleCollider_in()
    {
        CapsuleCollider.enabled = true;
    }


    public void CapsuleCollider_out()
    {
        CapsuleCollider.enabled = false;
    }
}
