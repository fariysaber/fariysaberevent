using System;
using System.Collections.Generic;
using UnityEngine;

//普通攻击技能
public class AttackSkill : SkillBase
{
    protected Bullet bullet;
    protected override void CastSkill()
    {
        base.CastSkill();

        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.moveLocker.AddLocker("skill" + skillId);
        playermodel.AddRotationLocker("skill" + skillId);
        ResetData();
    }
    protected override void MiddleSkill()
    {
        base.MiddleSkill();
        StartCheck();
    }
    protected virtual void StartCheck()
    {
        if (skillVo.bullet <= 0)
        {
            return;
        }
        bullet = (skillMgr.entity as BattlePlayerEntity).bulletMgr.CreateBullet(skillVo.bullet, skillId, skillLevel);
    }
    protected override void CreateMiddleEffect()
    {
        base.CreateMiddleEffect();
        if (skillVo.middleffect.Equals(""))
        {
            return;
        }
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 dir = playermodel.transform.forward;
        Vector3 pos = playermodel.transform.position;
        pos = new Vector3(0, 0.91f, 0);

        float engleY = playermodel.transform.eulerAngles.y;

        Effect attackEffect = skillMgr.GetEffect(skillVo.middleffect, ResourceType.scene, pos, new Vector3(0f, 0f,0f), Vector3.one,
            Globals.normaleffectlayer, playermodel.GetEffectObjName(), 1f);
    }
    protected override void MiddleSkillEnd()
    {
        base.MiddleSkillEnd();
        if (bullet != null)
        {
            bullet.Stop();
            bullet = null;
        }
    }
    protected override void MiddleSkillUpdate(float dt)
    {
        base.MiddleSkillUpdate(dt);
    }
    public override void EndSkill()
    {
        base.EndSkill();
        if (bullet != null)
        {
            bullet.Stop();
            bullet = null;
        }
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