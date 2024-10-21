using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class high_state : playerStatManager
{

    // 레이어 처리 변수
    private int platformAndObstacleMask;

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
        // 레이어 처리 변수
        platformAndObstacleMask = LayerMask.GetMask("platform", "ignorePlatform");
    }


    // Update is called once per frame
    void Update()
    {
        height_measurement();
    }


    public void height_measurement()
    {
        // Debug.Log(jump_high);
        if (rigid.velocity.y !=0)
        {
           
            RaycastHit2D[] rayHits = Physics2D.RaycastAll(rigid.position, Vector3.down, 30f, platformAndObstacleMask);
            Debug.DrawRay(rigid.position, Vector3.down * 15f, new Color(0, 1, 0));

            if (rayHits.Length > 0)
            {
                RaycastHit2D closestHit = rayHits[0];

                // Find the closest hit
                foreach (var hit in rayHits)
                {
                    if (hit.distance < closestHit.distance)
                    {
                        closestHit = hit;
                    }
                }
                if (closestHit.distance >= 30f)
                {
                    jump_high = 3;
                }

                else if (closestHit.distance >= 15f)
                {
                    jump_high = 2;
                    high_landing_ = true;
                }
                else if (closestHit.distance >= 7f)
                {
                    jump_high = 1;
                }
            }

        }

        else if(!anim.GetBool("jump"))
        {
            jump_high = 0;  
        }
    }




    public void high_landing_off_anim()
    {
        high_landing_ = false;
    }

}
