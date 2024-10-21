using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controll_1 : playerStatManager
{

    private BoxCollider2D boxCollider2D; 


    [Header("몬스터")]
    public GameObject enemy_1;    
    public GameObject enemy_2;    
    public GameObject enemy_3;     
    public GameObject enemy_4;       
    public GameObject enemy_5;    


    public GameObject isabel;    
    public GameObject izabel;    


    [Header("몬스터 리스폰")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    // 소환 위치
    private Vector3 spawnPosition1 = new Vector3(242.6531f, -30.53057f, 0f);
    private Vector3 spawnPosition2 = new Vector3(230.5971f, -49.39824f, 0f);
    private Vector3 spawnPosition3 = new Vector3(241.1848f,  -49.5747f, 0f);    
    private Vector3 spawnPosition4 = new Vector3(263.2863f, -30.42881f, 0f);
    private Vector3 spawnPosition5 = new Vector3(255.4779f, -39.42828f, 0f);
    private Vector3 spawnPosition6 = new Vector3(260.0217f, -39.38417f, 0f);
    private Vector3 spawnPosition7 = new Vector3(229.6266f, -44.54558f, 0f);
    private Vector3 spawnPosition8 = new Vector3(258.1f, -44.3f, 0f);
    private Vector3 spawnPosition9 = new Vector3(234.6266f, -44.54558f, 0f);

    private Vector3 spawnPosition10 = new Vector3(191.2f, -85.73836f, 0f);
    private Vector3 spawnPosition11 = new Vector3(174.3766f, -85.73836f, 0f);




    void Start()
    {
        SpawnEnemy();
    }


    public void SpawnEnemy()
    {
        Instantiate(enemy_1, spawnPosition1, Quaternion.identity);
        Instantiate(enemy_1, spawnPosition2, Quaternion.identity);
        Instantiate(enemy_1, spawnPosition3, Quaternion.identity);
        Instantiate(enemy_2, spawnPosition4, Quaternion.identity);
        Instantiate(enemy_3, spawnPosition5, Quaternion.identity);
        Instantiate(enemy_3, spawnPosition6, Quaternion.identity);
        Instantiate(enemy_3, spawnPosition7, Quaternion.identity);
        Instantiate(enemy_4, spawnPosition8, Quaternion.identity);
        Instantiate(enemy_5, spawnPosition9, Quaternion.identity);

    }




    public void reset_enemy()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();
            }
        }
    }



    public void reset_enemy_except_boss()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null && objectsToHit[i].name != "isabel(Clone)"  && objectsToHit[i].name != "izabel(Clone)")
            {  
                objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !boss_1)  // 또는 특정 레이어 확인
        {
            boss_1 = true;
            StartCoroutine(boss_start_delay());
        }
    }



    IEnumerator boss_start_delay()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(isabel, spawnPosition10, Quaternion.identity);
        Instantiate(izabel, spawnPosition11, Quaternion.identity);
    }



    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }

 
}
