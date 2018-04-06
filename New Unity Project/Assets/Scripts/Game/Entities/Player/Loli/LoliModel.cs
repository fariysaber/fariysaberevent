using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoliModel : PlayerModel
{
    protected override void RefreshAnimatorInfo()
    {
        animCtrl = new LoliAnimatorCtrl();
        animCtrl.InitAnim(this);
    }

    public void SetAnimSpeed(float speed)
    {
        (animCtrl as LoliAnimatorCtrl).SetSpeed(speed);
    }

    public override void SetHitBackAnim(float delayTime)
    {
        (animCtrl as LoliAnimatorCtrl).PlayAnimByState(41, 0, -1, delayTime);
    }

    protected override void RefreshAnimSpeedFloat(float finalSpeed)
    {
        if (animCtrl == null)
        {
            return;
        }
        Vector3 getStep = step;
        getStep.y = 0;
        if ((moving || joying || getStep != Vector3.zero) && !IsNotChangePos() && !IsLockAnimRunSpeed() && playerEntity.IsINGround())
        {
            SetAnimSpeed(finalSpeed / maxAnimSpeed);
        }
        else
        {
            SetAnimSpeed(0);
        }
    }
}
