using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungueDrill_saw : MonoBehaviour
{
    public float moveSpeed = 5f;  // Adjust speed as needed
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;  // Assuming you have a Rigidbody2D for physics-based movement
    

    public int damage;
    public bool damageAble;

    [Header("피격 효과")]
    public attackEffect attackEffect;

    [HideInInspector] public BoxCollider2D boxCollider2D; 

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();  
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        
        GameObject penintentIdleAnim = GameObject.Find("penintent_idle_anim_0");
        attackEffect = penintentIdleAnim.GetComponent<attackEffect>();
    
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale = 0;

        // Check movement direction based on sprite flip
        if (spriteRenderer.flipX)
        {
            // Move left
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else
        {
            // Move right
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        damage = 10;

        if (collider.gameObject.layer == LayerMask.NameToLayer("enemy") && damageAble)
        {
            // Debug.Log(collider.gameObject);
            collider.GetComponent<enemy_move>().EnemyHit(damage);
            attackEffect.standingHitEffect(3 , collider.transform.position);
        }
    }


    public void damage_anim()
    {
        damageAble = true;
    }

    public void damage_anim_off()
    {
        damageAble = false;
    }


}
