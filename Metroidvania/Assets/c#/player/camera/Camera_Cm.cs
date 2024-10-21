using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_Cm : MonoBehaviour
{

    [Header("피격시 카메라 진동 변수")]
    public float shakeMagnitude; // 지진의 세기
    public float shakeDuration;  // 지진 지속 시간

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    void Start()
    {
        // 씬에 있는 CinemachineVirtualCamera 오브젝트를 찾아 할당합니다.
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }



    public void ShakeCamera()
    {
       StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
    }
   

    public void ShakeCamera_lv2()
    {
        StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude * 1.5f)); // 강한 진동

    }

    IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsedTime = 0.0f;
        perlin.m_AmplitudeGain = magnitude;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        perlin.m_AmplitudeGain = 0f;
    }

}
