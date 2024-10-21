using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliff : MonoBehaviour
{

    [Header("방향 선정 : ture(우) false(좌)")]
    public bool direction;

    [Header("잡기 판정 공간")]
    public Transform left;      // 좌측
    public Transform right;       // 우측


    [Header("잡기 판정 백터")]
    public Vector2 left_;             // 좌측 
    public Vector2 right_;              // 우측

    [Header("적용 대상")]
    public LayerMask Layer; 

    [Header("적용 함수")]
    public hang hang; 
    



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(left.position , left_);
        Gizmos.DrawWireCube(right.position , right_);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Hit();
    }


    void Hit()
    {
        // 좌측
        if(!direction && !hang.spriteRenderer.flipX)
        {
           Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(left.position, left_, 0, Layer);
           if (objectsToHit.Length >= 1)
            {
                Vector3 currentPosition = transform.position;
                hang.hangon(currentPosition , direction);
            }
        }
        // 우측
        else if (direction && hang.spriteRenderer.flipX)
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(right.position, right_, 0, Layer);
            if (objectsToHit.Length >= 1)
            {
         
                Vector3 currentPosition = transform.position;
                hang.hangon(currentPosition , direction);
            }
        }
        


    }



    


}
