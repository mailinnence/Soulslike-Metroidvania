using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void TriggerShake(float amplitudeGain, float frequencyGain, float duration)
    {
        StartCoroutine(ShakeCoroutine(amplitudeGain, frequencyGain, duration));
    }

    private IEnumerator ShakeCoroutine(float amplitudeGain, float frequencyGain, float duration)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitudeGain;
            noise.m_FrequencyGain = frequencyGain;
            
            yield return new WaitForSeconds(duration);
            
            // Reset the noise values to 0 after the duration
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
