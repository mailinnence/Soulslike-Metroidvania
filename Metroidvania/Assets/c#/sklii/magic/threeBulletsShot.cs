using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threeBulletsShot : MonoBehaviour
{

    private bool animationEnded = false;
    public float moveSpeed = 100f;
    private float bulletAngle;
    CapsuleCollider2D CapsuleCollider;

    Rigidbody2D rigid; 

    [Header("이펙트")]
    public GameObject threeBulletsEnd_effect;


    void Start()
    {
        shotAngle();
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        
    }

    void Update()
    {
        
        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        Destroy(gameObject, 10f);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("platform") || collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {

            Quaternion rotation = Quaternion.Euler(0f, 0f, bulletAngle);
            Instantiate(threeBulletsEnd_effect, transform.position , transform.rotation);
            Destroy(gameObject);
            
        }
    }



    void shotAngle()
    {

       
        float raycastDistance = 10f; // 레이캐스트의 최대 거리
        LayerMask enemyLayerMask = LayerMask.GetMask("enemy"); // enemy 레이어에 대한 LayerMask

        for (int angle = 0; angle <= 360; angle += 1)
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
                bulletAngle = angle+8f;
                Debug.DrawLine(transform.position, raycastHit.point, Color.red);
                break;
            }


            // 공격 대상이 없을때 그 자리에서 폭발
            else if (angle == 360 && raycastHit.collider == null)
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                Instantiate(threeBulletsEnd_effect, transform.position , transform.rotation);
                Destroy(gameObject);
                break;
            }

        }



    }





}
