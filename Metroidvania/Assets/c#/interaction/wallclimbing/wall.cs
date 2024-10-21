using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : playerStatManager
{
    public wallClimb wallClimb;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerStay2D(Collider2D other)
    {
        // 벽이 왼쪽에 붙어 있을때는 플레이어가 왼쪽으로 바라봐야한다.
        if ((other.gameObject.layer == LayerMask.NameToLayer("player") && wallClimb.spriteRenderer.flipX && spriteRenderer.flipX)
        || (other.gameObject.layer == LayerMask.NameToLayer("player") && !wallClimb.spriteRenderer.flipX && !spriteRenderer.flipX))
        {
            attackAble = false;
            Vector3 currentPosition = transform.position;

            if (Input.GetKey(KeyCode.A) && !acting)
            { 
                wallClimb.climbStart(currentPosition); 
            }
        }

    }
    

    void OnTriggerExit2D(Collider2D other)
    {
        attackAble = true;
    }


}

