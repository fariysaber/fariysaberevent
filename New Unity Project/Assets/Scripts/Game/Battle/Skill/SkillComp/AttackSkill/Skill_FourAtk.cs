using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FourAtk : AttackSkill
{
    public int time1;
    public int bullet1;
    protected Bullet bulletC1;
    public int time2;
    public int bullet2;
    protected Bullet bulletC2;
    public int time3;
    public int bullet3;
    protected Bullet bulletC3;
    public int time4;
    public int bullet4;
    protected Bullet bulletC4;
    private int useIndex = 0;
    protected override void MiddleSkill()
    {
        base.MiddleSkill();
        useIndex = 0;
    }
    protected override void MiddleSkillUpdate(float dt)
    {
        base.MiddleSkillUpdate(dt);
        if (skillStateTime >= (float)time1 / 1000f && useIndex < 1)
        {
            createSkillEffect1();
            bulletC1 = (skillMgr.entity as BattlePlayerEntity).bulletMgr.CreateBullet(bullet1, skillId, skillLevel);
            useIndex += 1;
        }
        if (skillStateTime >= (float)time2 / 1000f && useIndex < 2)
        {
            createSkillEffect2();
            bulletC2 = (skillMgr.entity as BattlePlayerEntity).bulletMgr.CreateBullet(bullet2, skillId, skillLevel);
            useIndex += 1;
        }
        if (skillStateTime >= (float)time3 / 1000f && useIndex < 3)
        {
            createSkillEffect3();
            bulletC3 = (skillMgr.entity as BattlePlayerEntity).bulletMgr.CreateBullet(bullet3, skillId, skillLevel);
            useIndex += 1;
        }
        if (skillStateTime >= (float)time4 / 1000f && useIndex < 4)
        {
            createSkillEffect4();
            bulletC4 = (skillMgr.entity as BattlePlayerEntity).bulletMgr.CreateBullet(bullet4, skillId, skillLevel);
            useIndex += 1;
        }
    }

    protected virtual void createSkillEffect1()
    {
    }
    protected virtual void createSkillEffect2()
    {
    }
    protected virtual void createSkillEffect3()
    {
    }
    protected virtual void createSkillEffect4()
    {
    }
    public override void EndSkill()
    {
        base.EndSkill();
        if (bulletC1 != null)
        {
            bulletC1.Stop();
            bulletC1 = null;
        }
        if (bulletC2 != null)
        {
            bulletC2.Stop();
            bulletC2 = null;
        }
        if (bulletC3 != null)
        {
            bulletC3.Stop();
            bulletC3 = null;
        }
        if (bulletC4 != null)
        {
            bulletC4.Stop();
            bulletC4 = null;
        }
    }
}
