using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackReset :  StateMachineBehaviour
{

    public string trigger1;
    public string trigger2;
    public string trigger3;
    public string trigger4;
    public string trigger5;
    public string trigger6;
    public string trigger7;
    public string trigger8;
    public string trigger9;
    public string trigger10;


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(trigger1);
        animator.ResetTrigger(trigger2);
        animator.ResetTrigger(trigger3);
        animator.ResetTrigger(trigger4);
        animator.ResetTrigger(trigger5);
        animator.ResetTrigger(trigger6);
        animator.ResetTrigger(trigger7);
        animator.ResetTrigger(trigger8);
        animator.ResetTrigger(trigger9);
        animator.ResetTrigger(trigger10);
    }


    

}




