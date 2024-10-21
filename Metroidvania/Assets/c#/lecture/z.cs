using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class z : MonoBehaviour
{
    Rigidbody2D rigid;                  // 물리
    CapsuleCollider2D CapsuleCollider;  // 충돌
    SpriteRenderer spriteRenderer;      // 방향전환
    Animator anim;                      // 애니메이션
    
    // Start is called before the first frame update
    void Start()
    {
        // Animator 컴포넌트 가져오기
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // A 버튼을 누르면 애니메이션 재생
        if (Input.GetKeyDown(KeyCode.A))
        {
            // "YourAnimationName" 애니메이션 재생
            anim.SetTrigger("a");
        }
    }
}
