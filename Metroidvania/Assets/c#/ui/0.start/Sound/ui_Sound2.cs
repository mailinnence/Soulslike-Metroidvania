using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_Sound2 : MonoBehaviour
{
    public AudioClip _Relic;    
    public AudioClip _CHANGE_SELECTION;                           
    public AudioClip _EQUIP_ITEM;
    public AudioClip _UNEQUIP_ITEM;



    public void _Relic_function()
    {

        SoundManager.Instance.PlaySound(_Relic , volume: 0.3f );  //, volume: 4f
    }

    public void _CHANGE_SELECTION_function()
    {

        SoundManager.Instance.PlaySound(_CHANGE_SELECTION , volume: 0.3f);  //, volume: 4f
    }

    public void _EQUIP_ITEM_function()
    {

        SoundManager.Instance.PlaySound(_EQUIP_ITEM , volume: 0.3f);  //, volume: 4f
    }

    public void _UNEQUIP_ITEM_function()
    {

        SoundManager.Instance.PlaySound(_UNEQUIP_ITEM, volume: 0.3f ) ;  //, volume: 4f
    }



}

