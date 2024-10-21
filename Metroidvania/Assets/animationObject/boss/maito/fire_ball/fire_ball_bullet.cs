using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_ball_bullet : MonoBehaviour
{
    public GameObject fire_ball_end;

    public float speed = 5f;  // 탄환의 속도
    private float lifetime = 10f;  // 탄환이 날아가는 시간


    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D CapsuleCollider2D;   



    void Start()
    {
        // 15초 후에 탄환을 파괴
        Destroy(gameObject, lifetime);
    }


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }


    void Update()
    {
        // 직선으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }



    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        energyHp energyHpComponent = collision.GetComponent<energyHp>();
        if (energyHpComponent != null)
        {
            energyHpComponent.monster_attack_lv1(spriteRenderer.flipX, 20);

            Vector3 offset = new Vector3(0f, 0f, 0f);
            GameObject effectInstance = Instantiate(fire_ball_end, transform.position + offset, transform.rotation);
            Destroy(gameObject);

        }
    }



}
