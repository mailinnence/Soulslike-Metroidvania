using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class itemManager : playerStatManager
{
    // 아이템 목록

    [Header("hp")]
    public int hp_potion;
    // [HideInInspector] public int hp_potion_able = 5;
    // public int hp_potion_pocket;
    // public int hp_potion_total;
    
    
    [Header("mp")]
    public int mp_potion;
    // public int mp_potion_pocket;
    // public int mp_potion_total;


    // 나주엥 ui 에서 크게 수정되어야 하기 때문에 일단은 넘김

    [Header("현재 아이템 장착 상태")]
    public string item1;
    public string item2;
    public string item3;


    [Header("아이템 적용")]
	public Text item1State;
    public Text item2State;
    public Text item3State;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void Start()
    {

    }


    void Update()
    {
        // 아이템 갯수 관리
        // total_Item_Manager();

        // item1 = "hp_potion";
        // item2 = "mp_potion";

        item1State.text = (hp_potion).ToString();
        item2State.text = (mp_potion).ToString();

    }



    // 아이템 회복
    // 기도대에서 충전하여 최대 갯수까지 들고 다니거나 아이템을 주워서 확보하거나

    // 아이템 줍기 (애니메이션)

    // 아이템 줍기 (애니메이션x)



    // 아이템 수 관리
    // 내가 현재 들고 다니면서 사용할 수 있는 아이템의 양
    // 들고 있는 아이템을 제외한 총 양
    // 들고 있는 아이템까지 모두 포함한 총량

    // void total_Item_Manager()
    // {
    //     // hp 포션 갯수 관리
    //     hp_potion_total = hp_potion + hp_potion_pocket;
    //     hp_potion_pocket = hp_potion_total - hp_potion;


    //     // mp 포션 갯수 관리
    //     mp_potion_total = mp_potion + mp_potion_pocket;
    //     mp_potion_pocket = mp_potion_total - mp_potion;


    // }

}
