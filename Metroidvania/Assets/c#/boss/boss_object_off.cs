using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_object_off : playerStatManager
{


    private bool object_off;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // 한번이라고 !boss_2 가 나오면  
        if (!boss_2) 
        {
            object_off = true;
        }


        if (object_off) 
        {
            gameObject.SetActive(false);
        }

        
    }


}