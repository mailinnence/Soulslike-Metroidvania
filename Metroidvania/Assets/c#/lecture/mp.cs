using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mp : MonoBehaviour
{

    [Header("캐릭터 mp 관련 변수")]
    public Slider mpbar;
    public float maxMp = 100;
    public float curMp = 100;
    private float imsi;

    // Start is called before the first frame update
    void Start()
    {
        mpbar.value = (float) curMp / (float) maxMp;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {   
           
            if(curMp >= 0)
            {
                curMp -= 10;
            }
            else
            {
                curMp = 0;
            }       
        }
        imsi = (float) curMp / (float) maxMp;
        HandleMp();
    }



    private void HandleMp()
    {
        mpbar.value = Mathf.Lerp(mpbar.value , imsi , Time.deltaTime * 10);
    }



}