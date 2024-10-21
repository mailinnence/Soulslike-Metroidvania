using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_death_collider : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
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


    public void boxCollider_out()
    {
        boxCollider.enabled = false;
    }
}
