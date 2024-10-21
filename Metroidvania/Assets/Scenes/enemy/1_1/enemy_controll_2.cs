using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controll_2 : MonoBehaviour
{

   private BoxCollider2D boxCollider2D; 


    [Header("몬스터 리스폰")]
    public Transform interactionArea_1;
    public Transform interactionArea_one;
    public Transform interactionArea_two;
    public Transform interactionArea_three;
    public Vector2 interactionArea_;   
    public Vector2 interactionArea1_;  
    public Vector2 interactionArea2_;
    public Vector2 interactionArea3_;            
    public LayerMask interactionLayer; 



    [Header("몬스터")]
    public GameObject enemy_1;      // walkingtomb
    public GameObject enemy_2;      // wraith
    public GameObject enemy_3;      // flagllan
    public GameObject enemy_4;      // bishop
    public GameObject enemy_5;      // flying_2_1
    public GameObject enemy_6;      // lionhead
    public GameObject enemy_6_L;    // lionhead_L
    public GameObject enemy_7;      // menina
    public GameObject enemy_8;      // acolite



    [Header("소환 위치")]
    public Vector3[] spawnPositions;


    void Start()
    {
        SpawnEnemy();
    }


    public void SpawnEnemy()
    {
        Instantiate(enemy_1, spawnPositions[0], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[1], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[2], Quaternion.identity);
        Instantiate(enemy_1, spawnPositions[3], Quaternion.identity);
        Instantiate(enemy_1, spawnPositions[4], Quaternion.identity);
        Instantiate(enemy_1, spawnPositions[5], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[6], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[7], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[8], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[9], Quaternion.identity);
        Instantiate(enemy_6, spawnPositions[10], Quaternion.identity);
        Instantiate(enemy_6, spawnPositions[11], Quaternion.identity);
        Instantiate(enemy_6, spawnPositions[12], Quaternion.identity);
        Instantiate(enemy_6_L, spawnPositions[13], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[14], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[15], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[16], Quaternion.identity);
        Instantiate(enemy_2, spawnPositions[17], Quaternion.identity);
        Instantiate(enemy_5, spawnPositions[18], Quaternion.identity);
        Instantiate(enemy_5, spawnPositions[19], Quaternion.identity);
        Instantiate(enemy_5, spawnPositions[20], Quaternion.identity);
        Instantiate(enemy_8 , spawnPositions[21], Quaternion.identity);
        Instantiate(enemy_4 , spawnPositions[22], Quaternion.identity);
        Instantiate(enemy_3 , spawnPositions[23], Quaternion.identity);
        Instantiate(enemy_3 , spawnPositions[24], Quaternion.identity);
        Instantiate(enemy_4 , spawnPositions[25], Quaternion.identity);
        Instantiate(enemy_4 , spawnPositions[26], Quaternion.identity);
        Instantiate(enemy_4 , spawnPositions[27], Quaternion.identity);
        Instantiate(enemy_2 , spawnPositions[28], Quaternion.identity);
        Instantiate(enemy_3 , spawnPositions[29], Quaternion.identity);
        Instantiate(enemy_3 , spawnPositions[30], Quaternion.identity);
        Instantiate(enemy_8 , spawnPositions[31], Quaternion.identity);
        Instantiate(enemy_8 , spawnPositions[32], Quaternion.identity);
        Instantiate(enemy_7 , spawnPositions[33], Quaternion.identity);
        Instantiate(enemy_7 , spawnPositions[34], Quaternion.identity);
        Instantiate(enemy_2 , spawnPositions[35], Quaternion.identity);
        Instantiate(enemy_2 , spawnPositions[36], Quaternion.identity);
        Instantiate(enemy_2 , spawnPositions[37], Quaternion.identity);
        Instantiate(enemy_2 , spawnPositions[38], Quaternion.identity);
        Instantiate(enemy_5 , spawnPositions[39], Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        
    }



    public void reset_enemy()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea_1.position, interactionArea_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();
            }
        }
    }


    public void reset_enemy_1()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea_one.position, interactionArea1_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                if (objectsToHit[i].name == "flying_2_1(Clone)") {objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();}

                objectsToHit[i].GetComponent<enemy_move>().AnimationFinished_2();
            }
        }
    }


    public void reset_enemy_2()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea_two.position, interactionArea2_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                if (objectsToHit[i].name == "flying_2_1(Clone)") {objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();}

                objectsToHit[i].GetComponent<enemy_move>().AnimationFinished_2();
            }
        }
    }


    public void reset_enemy_3()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea_three.position, interactionArea3_, 0, interactionLayer);

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            if(objectsToHit[i].GetComponent<enemy_move>() != null)
            {
                if (objectsToHit[i].name == "flying_2_1(Clone)") {objectsToHit[i].GetComponent<enemy_move>().AnimationFinished();}

                else {objectsToHit[i].GetComponent<enemy_move>().AnimationFinished_2();}
            }
        }
    }




      // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(interactionArea_1.position , interactionArea_);

        Gizmos.DrawWireCube(interactionArea_one.position , interactionArea1_);
        Gizmos.DrawWireCube(interactionArea_two.position , interactionArea2_);
        Gizmos.DrawWireCube(interactionArea_three.position , interactionArea3_);
    }



}
