using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : playerStatManager
{

    [Header("Ui_object")]
    [HideInInspector] public Canvas InGame;
    [HideInInspector] public Canvas Inventory_1;
   public Canvas Inventory_2;
    [HideInInspector] public Canvas Inventory_3;
    [HideInInspector] public Canvas Inventory_4;
    [HideInInspector] public Canvas Inventory_5;
    [HideInInspector] public Canvas Inventory_6;
    [HideInInspector] public Canvas Inventory_7;



    [Header("Ui_menu")]
    [HideInInspector] public ui_Sound ui_Sound;










    void Start()
    {
        current = 0;
        choiceList = new List<int> { 1, 2 , 3 , 4 , 5 ,6 , 7};
        currentIndex = 0;


        inventory = false;
        current = 1;
        UpdateCurrent();
    }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    void Update()
    {

        OpenInventory();    // inventory bool 관리
        InventoryList();    // 페이지 별 관리
        // changeUi();



        if (inventory)
        {
            sound_Manager();    // 사운드 관리
            if (Input.GetKeyDown(KeyCode.Q))
            {
                MoveToPrevious();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                MoveToNext();
            }
        }
    }


    void MoveToPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateCurrent();
        }
    }

    void MoveToNext()
    {
        if (currentIndex < choiceList.Count - 1)
        {
            currentIndex++;
            UpdateCurrent();
        }
    }

    void UpdateCurrent()
    {
        current = choiceList[currentIndex];
    }




    void OpenInventory()
    {

        if (!inventory && Input.GetKeyDown(KeyCode.I)
        && !damaged && !isSlope && !anim.GetBool("jump") && !anim.GetBool("jump2") && alive)
        {
            inventory = true;
            InGame.enabled = false;
            ui_Sound._CHANGE_SELECTION_function();

        }
        else if(inventory&& Input.GetKeyDown(KeyCode.Escape))
        {
            inventory = false;
            InGame.enabled = true;
            ui_Sound._UNEQUIP_ITEM_function();
        }
    }



    void InventoryList()
    {
        if(inventory)
        {
            if(current == 1) { Inventory_1.enabled = true; Inventory_1_bool = true; }
            else {Inventory_1.enabled = false;  Inventory_1_bool = false; }

            if(current == 2) { Inventory_2.enabled = true; Inventory_2_bool = true; }
            else {Inventory_2.enabled = false; Inventory_2_bool = false; }

            if(current == 3) { Inventory_3.enabled = true; Inventory_3_bool = true; }
            else {Inventory_3.enabled = false; Inventory_3_bool = false; }

            if(current == 4) { Inventory_4.enabled = true; Inventory_4_bool = true; }
            else {Inventory_4.enabled = false; Inventory_4_bool = false; }

            if(current == 5) { Inventory_5.enabled = true; Inventory_5_bool = true; }
            else {Inventory_5.enabled = false; Inventory_5_bool = false; }

            if(current == 6) { Inventory_6.enabled = true; Inventory_6_bool = true; }
            else {Inventory_6.enabled = false; Inventory_6_bool = false; }

            if(current == 7) { Inventory_7.enabled = true; Inventory_7_bool = true; }
            else {Inventory_7.enabled = false; Inventory_7_bool = false; }
        }
        else 
        {
            Inventory_1.enabled = false; Inventory_1_bool = false;
            Inventory_2.enabled = false; Inventory_2_bool = false;
            Inventory_3.enabled = false; Inventory_3_bool = false;
            Inventory_4.enabled = false; Inventory_4_bool = false; 
            Inventory_5.enabled = false; Inventory_5_bool = false;
            Inventory_6.enabled = false; Inventory_6_bool = false;
            Inventory_7.enabled = false; Inventory_7_bool = false;

        }
    }



    // 사운드 소리
    void sound_Manager()
    {
        if (current == 1 && Input.GetKeyDown(KeyCode.E))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 2 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 3 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 4 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 5 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 6 && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
        else if (current == 7 && Input.GetKeyDown(KeyCode.Q))
        {
            ui_Sound._CHANGE_SELECTION_function();
        }
    }




}