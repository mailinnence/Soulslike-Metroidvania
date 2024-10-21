using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    public float followSpeed = 3;
    public Vector3 offset;

    void Start()
    {
        
    }



    void Update()
    {
        // transform.position = Vector3.Lerp(transform.position , lecture_1.Instance.transform.position + offset, followSpeed);
    }
}
