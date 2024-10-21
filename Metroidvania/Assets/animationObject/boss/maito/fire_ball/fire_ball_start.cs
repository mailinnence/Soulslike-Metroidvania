using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_ball_start : playerStatManager
{
    public GameObject fire_ball;

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    public void fire_start_()
    {
        Vector3 offset = new Vector3(0, 0f, 0f);
        float currentAngle = transform.eulerAngles.z; // 현재 오브젝트의 Z축 각도

        // 45도 간격으로 8개의 fire_ball 생성
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f; // 각도를 45도씩 증가
            float totalAngle = currentAngle + angle; // 현재 각도와 추가할 각도

            Quaternion rotation = Quaternion.Euler(0, 0, totalAngle);
            GameObject effectInstance = Instantiate(fire_ball, transform.position + offset, rotation);
        }

        Destroy(gameObject);
    }

}