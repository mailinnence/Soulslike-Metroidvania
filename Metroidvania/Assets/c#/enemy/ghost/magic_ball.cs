using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_ball : MonoBehaviour
{
    private bool animationEnded = false;
    public float moveSpeed = 100f;
    private float bulletAngle;
    CapsuleCollider2D CapsuleCollider;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid; 

    public GameObject magic_ball_destroy;
    public int damage;
    public bool direction;
    

    void Start()
    {
        shotAngle();
        damage = 10;
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        
        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        Invoke("DestroyAndInstantiate", 8f);
        
    }

    // 못 맞추고 10초 지나면 삭제
    void DestroyAndInstantiate()
    {
        // 현재 오브젝트 파괴
        Destroy(gameObject);

        // 새로운 마법 구슬 생성
        Instantiate(magic_ball_destroy, transform.position, transform.rotation);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        HashSet<GameObject> attackedObjects = new HashSet<GameObject>();

        if (other.gameObject.layer == LayerMask.NameToLayer("player") 
            || other.gameObject.layer == LayerMask.NameToLayer("parrying")
            || other.gameObject.layer == LayerMask.NameToLayer("NonColider"))
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, bulletAngle);
            Instantiate(magic_ball_destroy, transform.position, rotation);
            Destroy(gameObject);

            // 충격량 방향
            if (other.transform.position.x > transform.position.x) { direction = false; }
            else if (other.transform.position.x < transform.position.x) { direction = true; }


            // 레이어 비교 코드
            if (other.gameObject.layer == LayerMask.NameToLayer("parrying"))
            {
                other.GetComponent<parrying>().parrying_interaction(direction, "guard" , 6);
            }

            else if (other.gameObject.layer != LayerMask.NameToLayer("parrying") && attackedObjects.Add(other.gameObject))
            {
                // enemy_sound.PENITENT_HEAVY_DAMAGE_function();
                other.GetComponent<energyHp>().monster_attack_lv1(direction, damage);
            }
        }
    }



    void shotAngle()
    {
        float raycastDistance = 15f; // 레이캐스트의 최대 거리
        LayerMask enemyLayerMask = LayerMask.GetMask("player", "parrying" , "NonColider" , "playerDameged"); // enemy 레이어에 대한 LayerMask
				
				// += 각에 따라서 정교함이 달라짐
        for (int angle = 0; angle < 360; angle += 1)
        {
            // 각도를 라디안으로 변환
            float radians = angle * Mathf.Deg2Rad;

            // 방향 벡터 계산
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            // 레이캐스트 발사
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, direction, raycastDistance, enemyLayerMask);

            // 충돌 검사
            if (raycastHit.collider != null)
            {
                bulletAngle = angle+1f;
                Debug.DrawLine(transform.position, raycastHit.point, Color.red);
                break;
            }
        }
    }
}

