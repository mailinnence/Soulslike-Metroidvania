using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundController : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect; // The speed at which the background should move relative to the camera
    public float empty;

    void Start()
    {
        startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        // Calculate distance background move based on cam movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = move with cam || 1 = won't move || 0.5 = half

        transform.position = new Vector3(startPos + distance + empty, transform.position.y, transform.position.z);
    }
}