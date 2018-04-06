using System;
using System.Collections.Generic;
using UnityEngine;

public class LoliAnimatorCtrl : AnimatorCtrl
{
    public override void InitAnim(BaseModel mode)
    {
        base.InitAnim(mode);
        triggerName = "action";
    }
    protected override void AddAnimatorId()
    {
        base.AddAnimatorId();
        animatorsId.Add(0, "Idle_A");
        animatorsId.Add(1, "Idle_B");
        animatorsId.Add(2, "Idle_C");
        animatorsId.Add(3, "Idle_Atk");
        animatorsId.Add(41, "damaged");
        animatorsId.Add(21, "attack1");
        animatorsId.Add(22, "attack2");
        animatorsId.Add(23, "attack3");
        animatorsId.Add(11, "croll");
        animatorsId.Add(12, "jump");
        animatorsId.Add(46, "die");
        animatorsId.Add(50, "fear");
        animatorsId.Add(51, "Cry");
        animatorsId.Add(52, "pose");
        animatorsId.Add(53, "bye");
    }

    protected override bool isCanReplaceNewState(int newState)
    {
        if (newState == 41)
        {
            if ((_state < 10 || _state == 41) && animator.GetFloat("Speed") < 0.1)
            {
                return true;
            }
            return false;
        }
        return true;
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    protected override void UpdateAnimator(float dt)
    {
        base.UpdateAnimator(dt);
        if (continueTime > 0)
        {
            continueTime -= dt;
            if (continueTime <= 0 || animator.GetFloat("Speed") >= 0.1)
            {
                continueTime = -1f;
                SetAnimTrigger(0, false);
            }
        }
    }
}
