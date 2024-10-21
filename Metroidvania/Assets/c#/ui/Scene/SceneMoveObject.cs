using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMoveObject : MonoBehaviour
{
    public SceneMove SceneMove;
    public float targetX; // 이동할 x축 위치

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"일 때만 Fade 함수를 호출합니다.
        if (collision.gameObject.tag == "Player")
        {
            SceneMove.Fade();
            StartCoroutine(MovePlayerToTargetX_delay(collision.gameObject));
        }
    }

    IEnumerator MovePlayerToTargetX_delay(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);
        MovePlayerToTargetX(player);
    }

    private void MovePlayerToTargetX(GameObject player)
    {
        Vector3 newPosition = player.transform.position;
        newPosition.x = targetX;
        player.transform.position = newPosition;
    }
}
