using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost_king : MonoBehaviour
{

    [Header("공격 범위")]
    public Transform attackLeft;   
    public Transform attackRight;      
    public Vector2 attackLeft_;         
    public Vector2 attackRight_;
    public LayerMask AttackableLayers;
    public int damage;


    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D CapsuleCollider;    
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    // 공격 
    public void attackPlayer_anim()
    {
        HashSet<GameObject> attackedObjects = new HashSet<GameObject>();

        // 오른쪽
        if (!spriteRenderer.flipX)
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackRight.position, attackRight_, 0, AttackableLayers);
            foreach (Collider2D collider in objectsToHit)
            {
                // 레이어 비교 코드
                if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
                {
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 12);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
                    // enemy_sound.PENITENT_HEAVY_DAMAGE_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
                }
            }
        }
        // 왼쪽
        else if (spriteRenderer.flipX)
        {
            Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackLeft.position, attackLeft_, 0, AttackableLayers);
            foreach (Collider2D collider in objectsToHit)
            {
                // 레이어 비교 코드
                if (collider.gameObject.layer == LayerMask.NameToLayer("parrying"))
                {
                    collider.GetComponent<parrying>().parrying_interaction(spriteRenderer.flipX, "guard" , 12);
                }

                else if (collider.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(collider.gameObject))
                {
                    // enemy_sound.PENITENT_HEAVY_DAMAGE_function();
                    collider.GetComponent<energyHp>().monster_attack_lv1(spriteRenderer.flipX, damage);
                }
            }
        }
    }




    public void out_()
    {
        Destroy(gameObject);
    }



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackLeft.position , attackLeft_);
        Gizmos.DrawWireCube(attackRight.position , attackRight_);

    }


}
