using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charging : playerStatManager
{
    public float moveSpeed;  // 이동 속도를 증가시킴
    public int damage;       // 데미지 처리

    // 잔상효과
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost;

    [Header("이펙트")]
    public GameObject charging_effect;


    void Awake()
    {
        
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void Start()
    {
        // 잔상효과
        ghostDelaySeconds = ghostDelay;
        makeGhost = true;

        rigid.gravityScale = 0;
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }


    void Update()
    {
        if (spriteRenderer.flipX)
        {
            rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }


        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // generate a ghost
                Vector3 offset = new Vector3(0f, 0f, 0f); 
                GameObject currentGhost = Instantiate(ghost, transform.position + offset, transform.rotation);
                SpriteRenderer currentSpriteRenderer = GetComponent<SpriteRenderer>();
                Sprite currentSprite = currentSpriteRenderer.sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                
                // Flip x if needed
                bool flipX = currentSpriteRenderer.flipX; // Get the flipX value from the original sprite
                currentGhost.GetComponent<SpriteRenderer>().flipX = flipX; // Set the flipX value for the ghost

                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1f);
            }
        }

    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        damage = 35;

        if (collider.gameObject.layer == LayerMask.NameToLayer("enemy") )
        {
            // Debug.Log(collider.gameObject);
            collider.GetComponent<enemy_move>().EnemyHit(damage);
            // attackEffect.standingHitEffect(3 , collider.transform.position);


            SpriteRenderer slidingSpriteRenderer = charging_effect.GetComponent<SpriteRenderer>();
            slidingSpriteRenderer.flipX = spriteRenderer.flipX;


            if (!spriteRenderer.flipX) 
            { 
                Vector3 offset = new Vector3(-1.0f, 1.3f, 0f);
                Instantiate(charging_effect, collider.transform.position + offset , collider.transform.rotation); 
                Destroy(gameObject);
            }
            else if (spriteRenderer.flipX) 
            {
                Vector3 offset = new Vector3(1.0f, 1.3f, 0f);
                Instantiate(charging_effect, collider.transform.position + offset , collider.transform.rotation);
                Destroy(gameObject);
            }



        }
    }


}
