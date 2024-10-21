using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [Header("캐릭터 고정 위치 변수")]
    public float followSpeed ; // 3
    public Vector3 offset;



    [Header("피격시 카메라 진동 변수")]
    public attack2 attack2;

    public float shakeMagnitude; // 지진의 세기
    public float shakeSpeedf; // 지진의 속도
    public float shakeDuration; // 지진 지속 시간

    private Vector3 originalPosition; // 카메라 원래 위치
    
    public float shakeX;
    public float shakeY;



    [Header("카메라 영역 제한 변수")]
    public Vector2 center;
    public Vector2 size;





    void Start()
    {

    }

    void Update()
    {

        transform.position = Vector3.Lerp(transform.position , player.camera.transform.position + offset,  followSpeed);
        
        // 카메라 방식
        // transform.position = Vector3.Lerp(transform.position , player.camera.transform.position + offset, Time.deltaTime * followSpeed);
        // transform.position = new Vector3(transform.position.x , transform.position.y , -10f);



    }





    public void ShakeCamera()
    {
        StartCoroutine(ShakeCoroutine());
    }


    
    public void ShakeCamer_lv2()
    {
        StartCoroutine(ShakeCoroutine_lv2());
    }



    IEnumerator ShakeCoroutine()
    {
        // 지진의 방향
        float shakeX = 2f;
        float shakeY = 0.8f;

        // 지진의 시간과 세기
        float shakeMagnitude = 0.5f;    // 지진의 세기
        float shakeSpeed = 50.0f;       // 지진의 속도
        float shakeDuration = 0.1f;    // 지진 지속 시간

        float elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration)
        {
            // 랜덤한 지진 효과 생성
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0.0f) * shakeX - shakeY;
            float offsetY = Mathf.PerlinNoise(0.0f, Time.time * shakeSpeed) * shakeX - shakeY;
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0) * shakeMagnitude;

            // 카메라 위치에 더하여 지진 효과 적용
            transform.localPosition = transform.position + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 지진 효과가 끝나면 카메라를 원래 위치로 복원
        transform.localPosition = transform.position;
    }




    IEnumerator ShakeCoroutine_lv2()
    {
        // 지진의 방향
        float shakeX = 3f;
        float shakeY = 2f;

        // 지진의 시간과 세기
        float shakeMagnitude = 0.5f;    // 지진의 세기
        float shakeSpeed = 50.0f;       // 지진의 속도
        float shakeDuration = 0.1f;    // 지진 지속 시간

        float elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration)
        {
            // 랜덤한 지진 효과 생성
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0.0f) * shakeX - shakeY;
            float offsetY = Mathf.PerlinNoise(0.0f, Time.time * shakeSpeed) * shakeX - shakeY;
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0) * shakeMagnitude;

            // 카메라 위치에 더하여 지진 효과 적용
            transform.localPosition = transform.position + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 지진 효과가 끝나면 카메라를 원래 위치로 복원
        transform.localPosition = transform.position;
    }




}
