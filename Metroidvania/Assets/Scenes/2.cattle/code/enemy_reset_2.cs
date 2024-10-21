using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_reset_2 : MonoBehaviour
{
    
    [Header("몬스터 리셋")]
    public enemy_controll_2 enemy_controll_2;



    public void enemy_resawpon()
    {
        enemy_controll_2.reset_enemy();
        enemy_controll_2.SpawnEnemy();
    }

}
