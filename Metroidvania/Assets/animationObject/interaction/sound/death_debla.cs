using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class death_debla : MonoBehaviour
{
    [Header("debla 이펙트")]
    public GameObject death_debla_;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public death_debla_sound death_debla_sound;

    public bool start;


    
    [Header("플레이어 감지")]
    public Transform playerDetection;          
    public Vector2 playerDetection_;
    public LayerMask attackableLayer1;

    


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generate_red_debla()
    {
        Vector3 offset = new Vector3(0f, -5f, 0f); 
        GameObject effectInstance = Instantiate(death_debla_, transform.position + offset, transform.rotation);
        start = true;
        
    }

    public void delay()
    {
        if(start)
        {
            Destroy_debla();
            animator.enabled = false;
            spriteRenderer.enabled = false;
            StartCoroutine(RestartAnimation());
        }
    }


    private IEnumerator RestartAnimation()
    {
        // 5초 대기
        yield return new WaitForSeconds(8f);
        spriteRenderer.enabled = true;
        animator.enabled = true;
        playerDetection_function();
    
    }


    public void Destroy_debla()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");

        string currentPlayerJson = File.ReadAllText(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = File.ReadAllText(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

            // 오브젝트의 위치로 설명 텍스트 판단
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);


            if (playerData.candle.Contains(2))
            {
                Destroy(gameObject);
            }

     

            // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
            string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerPath, updatedPlayerJson);
        }
    }


    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }



    public void charging_sound()
    {
        death_debla_sound.PlayFirstAudio();
    }



    public void starting_sound()
    {
        death_debla_sound.PlaySecondAudio();
    }




// 플레이어 탐지
    public void playerDetection_function()
    {
        // 플레이어 위치 판단 ----------------------------------------------------------------------------------------------------------------------------------------
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(playerDetection.position, playerDetection_, 0, attackableLayer1);
  
        // 좌측
        if(objectsToHit.Length >= 1)  
        { 
            Vector3 currentPosition = transform.position;
            objectsToHit[0].GetComponent<energyHp>().Instant_Death_Detection(currentPosition);
       
        }
    }

   


    private void OnDrawGizmos()
    {
        // 플레이어 감지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetection.position , playerDetection_);
    }



}
