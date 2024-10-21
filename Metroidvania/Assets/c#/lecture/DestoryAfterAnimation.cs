using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterAnimation : playerStatManager
{
    
    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }



    void Start()
    {
        Destroy(gameObject , GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
      
    }

    // Update is called once per frame
    void Update()
    {
        
        // 보스클리어시 색깔 변환
        if(boss_clear)
        {
            spriteRenderer.color = new Color(0, 0, 0, 255f); 
        }
        else
        {
            if (gameObject.CompareTag("Detection"))
            {
                spriteRenderer.color = new Color(1f, 0f, 0f, 1f); // 빨간색
            }
            else if(!gameObject.CompareTag("potion"))
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 흰색
            }
        }

    }



}
