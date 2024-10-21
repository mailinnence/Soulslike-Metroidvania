using UnityEngine;

public class fire_collider : MonoBehaviour  // playerStatManager 대신 MonoBehaviour를 상속받도록 변경
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
            energyHpComponent.monster_attack_lv1(spriteRenderer.flipX, 20);
        }
    }


    public void boxCollider_out()
    {
        boxCollider.enabled = false;
    }
}