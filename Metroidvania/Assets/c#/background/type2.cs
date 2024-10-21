using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type2 : MonoBehaviour
{
    [SerializeField]
    private Transform target;  // 카메라 위치
    [SerializeField]
    private Transform player;  // 플레이어 위치

    [SerializeField]
    private float backgroundMoveSpeed;  // 배경화면 이동 속도

    private Vector3 previousCameraPosition;  // 이전 프레임에서의 카메라 위치
    private Vector3 previousPlayerPosition;  // 이전 프레임에서의 플레이어 위치

    private bool isPaused = false;

    void Start()
    {
        // 초기화
        previousCameraPosition = target.position;
        previousPlayerPosition = player.position;
    }

    void Update()
    {
        if (!isPaused)
        {
            // 현재 프레임의 카메라 위치와 플레이어 위치를 가져옴
            Vector3 currentCameraPosition = target.position;
            Vector3 currentPlayerPosition = player.position;

            // 카메라와 플레이어의 x축 이동 방향을 비교
            bool movingForward = currentCameraPosition.x > previousCameraPosition.x && currentCameraPosition.x > previousPlayerPosition.x;
            bool movingBackward = currentCameraPosition.x < previousCameraPosition.x && currentCameraPosition.x < previousPlayerPosition.x;

            // 정방향 또는 역방향일 때만 배경화면을 이동시킴
            if (movingForward || movingBackward)
            {
                // 배경화면을 부드럽게 이동시킴
                Vector2 offset = GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
                float moveAmount = backgroundMoveSpeed * Time.deltaTime * (movingBackward ? 1 : -1);
                offset.x += moveAmount;

                // 보간을 사용하여 부드럽게 이동
                float interpolationFactor = 0.9f; // 보간 계수 (0에 가까울수록 더 부드럽게 이동)
                offset.x = Mathf.Lerp(offset.x, offset.x + moveAmount, interpolationFactor);

                GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);

                // 이전 프레임의 위치를 현재 위치로 업데이트
                previousCameraPosition = currentCameraPosition;
                previousPlayerPosition = currentPlayerPosition;
            }
            else
            {
                // 움직이지 않을 때는 배경화면을 멈춤
                // optional: 배경화면을 멈추는 코드를 추가할 수 있습니다.
            }
        }

        // ESC 키를 눌러서 게임을 멈추거나 재개할 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            // 시간 스케일 조정으로 모든 애니메이션과 물리 연산을 멈추거나 재개함
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}
