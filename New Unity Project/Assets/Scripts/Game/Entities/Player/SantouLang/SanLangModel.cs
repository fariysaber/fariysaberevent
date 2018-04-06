using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanLangModel : PlayerModel
{
    protected override void RefreshAnimatorInfo()
    {
        animCtrl = new SanLangAnimatorCtrl();
        animCtrl.InitAnim(this);
    }

    public void SetAnimSpeed(float speed)
    {
        (animCtrl as AnimationCtrl).SetRuning(speed > 0);
    }

    public override void SetHitBackAnim(float delayTime)
    {
        (animCtrl as AnimationCtrl).PlayAnimByState(8, 0, -1, delayTime);
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
