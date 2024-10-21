using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class candle_camera : playerStatManager
{
    public event_background event_background;
    public GameObject camera;
    public float targetX; // 이동할 x축 위치
    public float targetY; // 이동할 x축 위치

    public CinemachineBrain cinemachineBrain;

    public void move_camera()
    {

        StartCoroutine(candle_off_());
    }

    IEnumerator candle_off_()
    {
        // 플레이어 행동 정지
        stop = true;

        // 페이드 아웃
        yield return new WaitForSeconds(1f);
        event_background.Fade_black(1.5f , 0.5f);
        cinemachineBrain.enabled = false;

        // 카메라 이동 , 이벤트 발생 
        yield return new WaitForSeconds(2f);
        StartCoroutine(MovePlayerToTargetX_delay());

        // 페이드 아웃
        yield return new WaitForSeconds(5f);
        event_background.Fade_black(1.5f , 0.5f);

        // 카메라 원상 복귀
        yield return new WaitForSeconds(2f);
        cinemachineBrain.enabled = true;
        stop = false;
        
    }


    IEnumerator MovePlayerToTargetX_delay()
    {
        yield return new WaitForSeconds(0.1f);
        MovePlayerToTargetX(camera);
    }

    private void MovePlayerToTargetX(GameObject camera)
    {
        Vector3 newPosition = camera.transform.position;
        newPosition.x = targetX;
        newPosition.y = targetY;
        camera.transform.position = newPosition;
    }
}
