using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요한 네임스페이스


public class sad_memory : playerStatManager
{


    [Header("알람 기준")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 

    public ui_location ui_location;

    public TextMeshProUGUI textMeshPro;
    public effectSound effectSound;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        sad_memory = false;
    }

    // Update is called once per frame
    void Update()
    {
        alert_function(interactionArea, interactionArea_);
    }


  // 아이템 픽업 애니메이션 
    void alert_function(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if (objectsToHit.Length >=1)
        {
            if (!sad_memory)
            {
                StartCoroutine(ui_location_delay());
                ChangeText("슬픔의 기억");
                
                
            }
            sad_memory = true;
        }
        else
        {
            sad_memory = false;
        }
    }


    IEnumerator ui_location_delay()
    {
        yield return new WaitForSeconds(delay);
        effectSound.ZONE_INFO_function();
        ui_location.alert();

    }

    // 텍스트를 바꾸기
    public void ChangeText(string newText)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = newText;
        }
    }



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }


}
