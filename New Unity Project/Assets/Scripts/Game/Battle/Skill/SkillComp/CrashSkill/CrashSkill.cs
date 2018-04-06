using System;
using System.Collections.Generic;
using UnityEngine;

public class CrashSkill : SkillBase
{
    public float movedis;
    public override void InitInfo(BattleSkillBaseInfo baseInfo)
    {
        
    }
    protected override bool CheckAndBreak()
    {
        if (base.CheckAndBreak() == false)
        {
            Debugger.Log("被打断");
            return false;    
        }
        if (skillMgr.entity.IsINGround() == false)
        {
            Debugger.Log("不在陆地");
            return false;
        }
        if (skillMgr.entity.isLockPos())
        {
            Debugger.Log("被锁住位置");
            return false;
        }
        return true;
    }
    protected override void CastSkill()
    {
        base.CastSkill();
    }

    protected override void MiddleSkill()
    {
        base.MiddleSkill();
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.AddLocker("skill" + skillId);
        playermodel.AddRotationLocker("skill" + skillId);
        Vector3 dir = playermodel.transform.forward;
        playermodel.actMoveCtrl.actSkillMove.SetSkillMove(dir, movedis, middletime,1f);
        if(playermodel.playerEntity.EntityType == EntityTypes.BattlePlayerFriend 
        || playermodel.playerEntity.EntityType == EntityTypes.BattlePlayerMy 
        || playermodel.playerEntity.EntityType == EntityTypes.BattlePlayerMyTeam )
        {
            playermodel.SetLimitLayer(Globals.BattlePlayerMineShanlayer, "skill" + skillId);
        }
        else if (playermodel.playerEntity.EntityType == EntityTypes.BattlePlayerDiRen)
        {
            playermodel.SetLimitLayer(Globals.BattlePlayerDirenShanlayer, "skill" + skillId);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.RemoveLocker("skill" + skillId);
        playermodel.rotationLocker.RemoveLocker("skill" + skillId);
        playermodel.animCtrl.PlayResetIdle();
        playermodel.ResetLayer("skill" + skillId);
    }
}