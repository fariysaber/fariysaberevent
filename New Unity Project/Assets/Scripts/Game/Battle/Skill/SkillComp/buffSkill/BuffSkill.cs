using System;
using System.Collections.Generic;
using UnityEngine;

//普通buff技能
public class BuffSkill : SkillBase
{
    public int addbuffId;
    protected override void CastSkill()
    {
        base.CastSkill();

        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.AddLocker("skill" + skillId);
        playermodel.AddRotationLocker("skill" + skillId);
        ResetData();
    }
    protected override void MiddleSkillEnd()
    {
        base.MiddleSkillEnd();
        BattlePlayerEntity batentity = skillMgr.entity as BattlePlayerEntity;
        if (addbuffId > 0)
        {
            batentity.buffMgr.AddBuff(addbuffId);
        }
    }
    protected override void CreateMiddleEffect()
    {
        base.CreateMiddleEffect();
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 pos = playermodel.transform.position;
        pos = new Vector3(0, 0f, 0);

        Effect attackEffect = skillMgr.GetEffect(skillVo.middleffect, ResourceType.scene, pos, new Vector3(0f, 0f, 0f), Vector3.one,
            Globals.normaleffectlayer, playermodel.GetEffectObjName(true), 1f);
    }
    public override void EndSkill()
    {
        base.EndSkill();
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.RemoveLocker("skill" + skillId);
        playermodel.rotationLocker.RemoveLocker("skill" + skillId);
        playermodel.animCtrl.PlayResetIdle();
        ResetData();
    }
    protected virtual void ResetData()
    {
    }
}