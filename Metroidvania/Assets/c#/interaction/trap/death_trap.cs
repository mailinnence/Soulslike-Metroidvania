using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_trap : MonoBehaviour
{

    [Header("적용 함수")]
    public energyHp energyHp; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 벽이 왼쪽에 붙어 있을때는 플레이어가 왼쪽으로 바라봐야한다.
        if ((other.gameObject.layer == LayerMask.NameToLayer("player")) || 
        (other.gameObject.layer == LayerMask.NameToLayer("NonColider")) || 
        other.gameObject.layer == LayerMask.NameToLayer("playerDameged") || 
        other.gameObject.layer == LayerMask.NameToLayer("parrying"))
        {
            energyHp.death_trap();
        }

    }

}


// && wallClimb.spriteRenderer.flipX && spriteRenderer.flipX)
//         || (other.gameObject.layer == LayerMask.NameToLayer("player") && !wallClimb.spriteRenderer.flipX && !spriteRenderer.flipX