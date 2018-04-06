using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerEntity : PlayerEntity
{
    public SkillMgr skillMgr;
    public BuffMgr buffMgr;
    public BulletMgr bulletMgr;
    public AttributeMgr attributeMgr;
    public bool isHited = false;
    public BattlePlayerEntity chouhenEntity;
    public int chouhendamage = 0;
    public float chouhendmgtime = -1f;
    public float chouhenremoveTime = -1;
    protected override void InitChildData(EntityInitData data)
    {
        skillMgr = new SkillMgr();
        buffMgr = new BuffMgr(this);
        bulletMgr = new BulletMgr(this);
        attributeMgr = new AttributeMgr();
        skillMgr.InitSkills(data.skillBaseInfo, this);
        attributeMgr.InitAttri(data, this);
    }
    public void RemoveChouhen(bool forece = false)
    {
        chouhenEntity = null;
        if (chouhenEntity == null || chouhenEntity.IsDispose)
        {
            chouhenremoveTime = -1f;
            return;
        }
        if (chouhenremoveTime < 0 || forece)
        {
            chouhenremoveTime = 5f;
        }
    }
    private void UpdateChouhen(float dt)
    {
        if (chouhenremoveTime >= 0)
        {
            chouhenremoveTime -= dt;
            if (chouhenremoveTime < 0)
            {
                chouhenEntity = null;
            }
        }
        if (chouhendmgtime >= 0)
        {
            chouhendmgtime -= dt;
        }
    }
    public void SetHitDmg(BattlePlayerEntity chouhenentity, int dmg,bool baoji = false)
    {
        if (chouhenEntity == null || (chouhenEntity != chouhenentity && dmg > chouhendamage && chouhendmgtime < 0))
        {
            chouhenEntity = chouhenentity;
            chouhendamage = dmg;
            chouhendmgtime = 2f;
        }
        (model as PlayerModel).SetHitBackAnim(1f);
        attributeMgr.AddHp(-dmg, baoji);
        if (dmg > 0)
        {
            isHited = true;
        }
    }
    protected override void OnBattleUpdate(float dt)
    {
        base.OnBattleUpdate(dt);
        if (attributeMgr != null)
            attributeMgr.OnUpdate(dt);
        if(skillMgr != null)
            skillMgr.OnUpdate(dt);
        if (buffMgr != null)
            buffMgr.OnUpdate(dt);
        if (bulletMgr != null)
            bulletMgr.OnUpdate(dt);
        UpdateChouhen(dt);
    }

    protected override void DestroyOther()
    {
        if (skillMgr != null)
        {
            skillMgr.DestroyAll();
        }
        if (buffMgr != null)
        {
            buffMgr.DestroyAll();
        }
        if (bulletMgr != null)
        {
            bulletMgr.DestroyAll();
        }
        if (attributeMgr != null)
        {
            attributeMgr.DestroyAll();
        }
    }
}