using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpSkill : SkillBase
{
    public float jumpspeed;
    public override void InitInfo(BattleSkillBaseInfo baseInfo)
    {
        
    }
    protected override bool CheckAndBreak()
    {
        if (base.CheckAndBreak() == false)
        {
            return false;    
        }
        if (skillMgr.entity.IsINGround() == false)
        {
            return false;
        }
        if (skillMgr.entity.isLockPos())
        {
            return false;
        }
        return true;
    }

    protected override void MiddleSkill()
    {
        base.MiddleSkill();
        (skillMgr.entity.model as PlayerModel).actMoveCtrl.downGround.StartJump();
    }
}