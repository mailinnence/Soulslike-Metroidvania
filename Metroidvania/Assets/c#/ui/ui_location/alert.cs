using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alert : playerStatManager
{


    [Header("알람 기준")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alert_function(interactionArea, interactionArea_);
    }


  // 아이템 픽업 애니메이션 
    void alert_function(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if (objectsToHit.Length >=1)
        {
            
            sad_memory = true;
        }
    }




    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }


}
