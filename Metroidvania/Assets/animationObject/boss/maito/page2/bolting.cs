using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolting : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D CapsuleCollider2D;   

    public bool attack_able;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void attack_able_on()
    {
        attack_able = true;
    }



    void attack_able_off()
    {
        attack_able = false;
    }


    
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        if (energyHpComponent != null && attack_able)
        {
            attack_able = false;
            energyHpComponent.monster_attack_lv1(spriteRenderer.flipX, 20);


        }
    }

}
