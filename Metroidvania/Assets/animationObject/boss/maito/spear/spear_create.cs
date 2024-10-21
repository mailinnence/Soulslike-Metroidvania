using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spear_create : playerStatManager
{
    public GameObject spear;
    public bool start;

    private bool animationEnded = false;

    private float bulletAngle;


    public AudioClip clip1;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        if(!start)
        {
            // 자연스러운 방향 전환을 위해서 초반에는 안 보이게 한다.
            spriteRenderer.enabled = true;
            shotAngle();
            transform.rotation = Quaternion.Euler(0f, 0f, 90f +bulletAngle);
        }
        
    }


    public void sound_1()
    {
        if (audioSource != null && clip1 != null)
        {
            audioSource.clip = clip1;
            audioSource.Play();
        }
    }



    public void spear_start_()
    {
        start = false;
        Vector3 offset = new Vector3(0f, 0f, 0f);
        GameObject effectInstance = Instantiate(spear, transform.position + offset, transform.rotation);
        Destroy(gameObject);
    }



    void shotAngle()
    {
        float raycastDistance = 100f; // 레이캐스트의 최대 거리
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
