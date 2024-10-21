using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back_sound : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource를 연결

    private bool one_var;

    public void Start_music()
    {
        StartCoroutine(sound_delay());
    }



    IEnumerator sound_delay()
    {
        audioSource.loop = true; // 음악을 루프로 설정
        audioSource.enabled = false; // 처음에는 AudioSource를 비활성화
        yield return new WaitForSeconds(1f);
        audioSource.enabled = true;
    }
}
