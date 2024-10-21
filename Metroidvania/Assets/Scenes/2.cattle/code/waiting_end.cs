using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waiting_end : playerStatManager
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체에서 interaction_object 컴포넌트를 가져옵니다.
        interaction_object interaction = collision.GetComponent<interaction_object>();

        // interaction_object가 있을 경우, waiting_Anim 메서드를 호출합니다.
        if (interaction != null)
        {

            interaction.waiting_end_Anim();


        }
    }
}
