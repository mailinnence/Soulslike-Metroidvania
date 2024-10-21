using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ladders_move : MonoBehaviour
{
    // public float leftBound = -328.87f;
    // public float rightBound = -317.63f;
    // public float moveSpeed = 2f;
    // public float pauseDuration = 1f;

    public float leftBound;
    public float rightBound;
    public float moveSpeed;
    public float pauseDuration;

    private bool movingRight = true;

    void Start()
    {
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            // 오른쪽으로 이동
            if (movingRight)
            {
                while (transform.position.x < rightBound)
                {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                    yield return null;
                }
                // 오른쪽 끝에 도달하면 1초 대기
                yield return new WaitForSeconds(pauseDuration);
                movingRight = false;
            }
            // 왼쪽으로 이동
            else
            {
                while (transform.position.x > leftBound)
                {
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                    yield return null;
                }
                // 왼쪽 끝에 도달하면 1초 대기
                yield return new WaitForSeconds(pauseDuration);
                movingRight = true;
            }
        }
    }
}
