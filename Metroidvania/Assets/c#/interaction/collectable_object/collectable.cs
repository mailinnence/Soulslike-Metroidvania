using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class collectable : MonoBehaviour
{

    [HideInInspector] public SpriteRenderer spriteRenderer;      
    [HideInInspector] public Animator anim;   


    [Header("오브젝트의 유형")]
    public string type; 
    public string name;     
    // 이벤트 아이템 , 도서 , 스킬(이동기)  
    /*
    event_Item
    bible
    skill
    move_skill

    */

    [Header("이벤트 알림")]
    public item_alert item_alert;



    [Header("상호작용 구간 , 벡터 , 레이어")]
    public Transform interactionArea;
    public Vector2 interactionArea_;             
    public LayerMask interactionLayer; 


    [Header("상호작용 설명 텍스트")]
    public textBackground textBackground;
    public text text;
    public bool textAble;                      // 설명 텍스트 문 동작 여부


    [Header("상호작용 player 애니메이션")]
    public interaction_object interaction_object;


    // 여기만 바꿔주면 된다.
    void Start()
    {

    }


    // 공격 범위 draw 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactionArea.position , interactionArea_);
    }



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        textAble = true;
    }


    // Update is called once per frame
    void Update()
    {
        DescriptionText(interactionArea,  interactionArea_);
        pickUp(interactionArea, interactionArea_ );
    }




    // 설명 텍스트 여부
    void DescriptionText(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);

        if(textAble)
        {

            if (objectsToHit.Length >=1)
            {
                textBackground.Activate();
                text.Activate();
            }
            else
            {
                textBackground.Deactivate();
                text.Deactivate();
            }
        }
        else
        {
            textBackground.Deactivate();
            text.Deactivate();
        }
    }

    // 애니메이션 중에는 설명 텍스트가 뜰 필요가 없음
    void textAble_on()
    {
        textAble = true;
    }






    // 아이템 픽업 애니메이션 
    void pickUp(Transform interactionArea, Vector2 interactionArea_ )
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(interactionArea.position, interactionArea_, 0, interactionLayer);


        if (Input.GetKey(KeyCode.C) && objectsToHit.Length >=1 && !playerStatManager.acting)
        {
            textAble = false;
            Vector3 currentPosition = transform.position;
            interaction_object.pickUp_Anim(currentPosition);
            json_save();

            StartCoroutine(DestroyObjectAfterDelay());
        }
    }




    void json_save()
    {
        string path = Application.persistentDataPath + "/current_player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(json);
            int currentPlayer = currentPlayerData.current_player;

            string playerPath = Application.persistentDataPath + $"/player{currentPlayer}.json";
            if (File.Exists(playerPath))
            {
                string playerJson = File.ReadAllText(playerPath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

                switch (type)
                {
                    case "event_Item":
                        if (!playerData.event_Item.Contains(name))
                        {
                            playerData.event_Item.Add(name);
                        }
                        break;
                    case "bible":
                        if (!playerData.bible.Contains(name))
                        {
                            playerData.bible.Add(name);
                        }
                        break;
                    case "skill":
                        if (!playerData.skill.Contains(name))
                        {
                            playerData.skill.Add(name);
                        }
                        break;
                    case "move_skill":
                        if (!playerData.skill.Contains(name))
                        {
                            playerData.skill.Add(name);
                        }
                        break;
                    default:
                        Debug.LogWarning("Invalid item type.");
                        break;
                }

                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }


    
    // 2초 후 객체를 삭제하는 코루틴
    IEnumerator DestroyObjectAfterDelay()
    {
        yield return new WaitForSeconds(1f);
     

        item_alert.alert();
        if(name == "지식의 무지")
        {
            item_alert.ChangeText($"{name}를 획득하였습니다.");
        }
        else
        {
            item_alert.ChangeText($"{name}을 획득하였습니다.");
        }

     
        Destroy(gameObject);
    }



}




[System.Serializable]
public class CurrentPlayerData
{
    public int current_player;
}

namespace CollectableNamespace
{
    [System.Serializable]
    public class PlayerData
    {
        public int current_player;
        public string last_play_time;
        public string save_Scene;
        public string save_Location;
        public int hp;
        public int mp;
        public int money;
        public List<string> event_Item = new List<string>();
        public List<string> bible = new List<string>();
        public List<string> skill = new List<string>();
        public List<string> move_skill = new List<string>();
        public string current_skill;
        public List<string> current_Item = new List<string>();
        public Dictionary<string, int> consumable_Item = new Dictionary<string, int>();
        public List<Stage> stage_list = new List<Stage>();
    }

    [System.Serializable]
    public class Stage
    {
        public string key;
        public int value;
    }
}