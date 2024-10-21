using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pairOfFire : MonoBehaviour
{


    [Header("이펙트")]
    public GameObject pairOfFiring_effect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AnimationFinished()
    {
     
        for (int i = 0; i < 15; i++)
        {
            float yOffset = i * 1.2f; // 각 반복에서의 y 오프셋 계산
            Vector3 offset = new Vector3(0f, yOffset, 0f);
            Instantiate(pairOfFiring_effect, transform.position + offset, transform.rotation);
        }

    }



 



}
