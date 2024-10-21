using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy_reset_1 : MonoBehaviour
{

    
    [Header("몬스터 리셋")]
    public enemy_controll_1 enemy_controll_1;


    public void enemy_resawpon()
    {
        enemy_controll_1.reset_enemy();
        enemy_controll_1.SpawnEnemy();
    }

}
