using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkill : CrashSkill
{
    protected override void MiddleSkill()
    {
        base.MiddleSkill();
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.AddLocker("skill" + skillId);
        playermodel.AddRotationLocker("skill" + skillId);
        Vector3 dir = playermodel.transform.forward;
        playermodel.actMoveCtrl.actSkillMove.SetSkillMove(dir, movedis, middletime, 1f);
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
