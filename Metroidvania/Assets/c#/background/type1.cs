using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type1 : MonoBehaviour
{ 
    [SerializeField]
    private Transform target;         // 카메라의 위치
    [SerializeField]
    private float scrollAmount;       // 이어지는 두 배경 사이의 거리
    [SerializeField]
    private float moveSpeed;          // 이동 속도
    [SerializeField]
    private Vector3 moveDirection;    // 이동 방향

    private float previousX; // 이전 프레임의 target.position.x 값

    private void Start()
    {
        // 초기화
        previousX = target.position.x;
    }

    private void Update()
    {
        Debug.Log(target.position.x);
        float currentX = target.position.x; // 현재 프레임의 target.position.x 값

        // target.position.x의 변화 추적
        if (currentX > previousX)
        {
            // Debug.Log("target.position.x is increasing");
            moveDirection = new Vector3(1, 0, 0);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else if (currentX < previousX)
        {
            // Debug.Log("target.position.x is decreasing");
            moveDirection = new Vector3(-1, 0, 0);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else if (currentX < previousX)
        {
            // Debug.Log("target.position.x is unchanged");
            moveDirection = new Vector3(-1, 0, 0);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // 현재 프레임의 값을 다음 프레임에서 이전 값으로 사용하기 위해 저장
        previousX = currentX;

    }


}
