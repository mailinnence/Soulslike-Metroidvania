using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class playerStatManager : MonoBehaviour 
{

   // 인스턴스 선언
    public static playerStatManager p;



    // Component
    [HideInInspector] public Rigidbody2D rigid;                  // 물리
    [HideInInspector] public CapsuleCollider2D CapsuleCollider;  // 충돌
    [HideInInspector] public SpriteRenderer spriteRenderer;      // 방향전환
    [HideInInspector] public Animator anim;                      // 애니메이션
    [HideInInspector] public BoxCollider2D boxCollider;    


    [HideInInspector] public static bool stop = false;    


    [Header("캐릭터 데미지 관련 변수")]
    [HideInInspector] public static bool damaged = false;               // 데미지 여부
    [HideInInspector] public static bool damagedMove = false;               // 데미지 여부

    [Header("캐릭터 경사로 관련 변수")]
    [HideInInspector] public static bool isSlope;                       // 경사로 유뮤


    [Header("캐릭터 점프 관련 변수")]
    [HideInInspector] public static bool jumpAble = true;                       // 점프 관련 여부 true 일때만 
    [HideInInspector] public static bool jump_verticalattack = false;           // 중력을 다른 함수와 분리하기 위한 변수
    [HideInInspector] public static bool jump_ghost = false;           // 중력을 다른 함수와 분리하기 위한 변수
    [HideInInspector] public static int  jump_high = 0;           // 중력을 다른 함수와 분리하기 위한 변수
    [HideInInspector] public static bool  gravity_anim_;             // 중력별 떨어지는 속도 제어
    [HideInInspector] public static float gravity;
    [HideInInspector] public static bool  gravity_hit;
    [HideInInspector] public static bool  high_landing_;        // 높은 곳에서의 착지 애니메이션
    

    [Header("슬라이딩 관련 변수")]
    [HideInInspector] public static bool slidingExcept = false;         // 점프에서 슬라이딩 변환시점에서 슬라이딩 버그를 막기위한 변수
    [HideInInspector] public static bool isSliding = false;             // Flag to check if the character is currently sliding
    [HideInInspector] public static bool isSliding_stabbing = false;             // Flag to check if the character is currently sliding

    [Header("찌르기 관련 변수")]
    [HideInInspector] public static bool stabbing_ = false;    


    [Header("에어도어 관련 변수")]                                 // 슬라이딩의 관련 변수를 상속함
    [HideInInspector] public static bool airdooring = false;

    
    [Header("캐릭터 물리/마법 제어")]
    [HideInInspector] public static bool acting = false;
    [HideInInspector] public static bool actingButMove;


    [Header("캐릭터 체력 / 마력")]
    [HideInInspector] public static bool alive = true;      // 생존 여부 변수
    [HideInInspector] public static bool hurt;              // 무적시간 관련 변수


    [Header("캐릭터 공격 콤버")]
    [HideInInspector] public static bool attacking = false;                        // 공격 여부 변수           

    [Header("캐릭터 공격 불가")]
    [HideInInspector] public static bool attackAble = true;                        // 공격 여부 변수                     


    [Header("캐릭터 아이템 사용 여부")]
    [HideInInspector] public static bool itemUsingState = false;                        // 공격 여부 변수     


    [Header("설명 텍스트 문 동작 여부")]
    [HideInInspector] public static bool textAble = false;                        // 공격 여부 변수     

    [Header("현재 세이브 존 위치")]
    [HideInInspector] public static string saveZonelocation;                        // 공격 여부 변수     



    [Header("사다리 가능 여부")]
    [HideInInspector] public static bool isLadder;              // 사다리를 올라탈 수 있는지 변수
    [HideInInspector] public static bool isLadderDown;              // 사다리를 올라탈 수 있는지 변수
    [HideInInspector] public static bool DownKeyLimit;              // 사다리를 내려갈때 숙이는 모션 방지
    

    [Header("ignore box")]
    [HideInInspector] public static bool ignoreButton = false;


    [Header("매달리기")]
    [HideInInspector] public static bool hangAble = false;


    [Header("패링")]
    [HideInInspector] public static bool parrying_action = false;
    [HideInInspector] public static bool parrying_counter = false;
    

    [Header("인벤터리")]
    [HideInInspector] public static bool inventory = false;
    [HideInInspector] public static bool Inventory_1_bool;
    [HideInInspector] public static bool Inventory_2_bool;
    [HideInInspector] public static bool Inventory_3_bool;
    [HideInInspector] public static bool Inventory_4_bool;
    [HideInInspector] public static bool Inventory_5_bool;
    [HideInInspector] public static bool Inventory_6_bool;
    [HideInInspector] public static bool Inventory_7_bool;


    [Header("Inventory_List")]
    [HideInInspector] public int current;
    [HideInInspector] public List<int> choiceList = new List<int>();
    [HideInInspector] public int currentIndex;




    [Header("location_alert")]
    [HideInInspector] public static bool sad_memory;
    [HideInInspector] public static bool Salares;
    [HideInInspector] public static bool Salares_abandoned;
    [HideInInspector] public static bool Shore;
    [HideInInspector] public static bool maito_art;
    


    [Header("boss")]
    [HideInInspector] public static bool boss_1;
    [HideInInspector] public static bool boss_clear;

    [HideInInspector] public static bool boss_2;
    [HideInInspector] public static bool boss_2_start;
    [HideInInspector] public static bool boss_clear_2;







    [Header("boss_2")]
    [HideInInspector] public static float fadeFloat;
    [HideInInspector] public static float fadeSpeed;

    [HideInInspector] public static float fadeFloat_fire;
    [HideInInspector] public static float fadeSpeed_fire;

    [HideInInspector] public static float fadeFloat_fire_back;
    [HideInInspector] public static float fadeSpeed_fire_back;

    
 












}




