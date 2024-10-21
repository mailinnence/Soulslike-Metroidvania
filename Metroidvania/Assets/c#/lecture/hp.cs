using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp : MonoBehaviour
{
    [Header("캐릭터 hp 관련 변수")]
    public Slider hpbar;
    public float maxHp = 100;
    public float curHp = 100;
    private float imsi;


    // Start is called before the first frame update
    void Start()
    {
        hpbar.value = (float) curHp / (float) maxHp;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(curHp >= 0)
            {
                curHp -= 10;
            }
            else
            {
                curHp = 0;
            }       
        }
        imsi = (float) curHp / (float) maxHp;
        HandleHp();
    }



    private void HandleHp()
    {
        hpbar.value = Mathf.Lerp(hpbar.value , imsi , Time.deltaTime * 10);
    }



}