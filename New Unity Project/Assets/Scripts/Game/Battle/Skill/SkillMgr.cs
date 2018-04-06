using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillMgr
{
    public List<SkillBase> skillList;
    protected EffectMgr effectMgr;
    public PlayerEntity entity;
    public SkillMgr()
    {
        skillList = new List<SkillBase>();
        effectMgr = new EffectMgr();
    }
    public Effect GetEffect(string path, ResourceType resType, Vector3 localPos, Vector3 rotate, Vector3 scale, int layer, Transform parent,float time = -1,
        bool resetPause = true)
    {
        Effect effect = effectMgr.GetEffect(path);
        effect.InitEffect(path, resType, localPos, rotate, scale, layer, parent, time, parent);
        return effect;
    }
    public void AddTeampEffect(Effect effect)
    {
        effectMgr.Addtemp(effect);
    }
    public void InitSkills(List<BattleSkillBaseInfo> info, PlayerEntity getentity)
    {
        entity = getentity;
        for (int i = 0; i < info.Count; i++)
        {
            Type getType = GameTools.GetType("Skill_" + info[i].skillId);
            object getSkill = Activator.CreateInstance(getType);

            Globals.GetSkillBattleParam(info[i].skillId, ref getSkill);

            SkillBase skillbase = getSkill as SkillBase;
            skillbase.Init(info[i], this);
            skillList.Add(skillbase);
        }
    }

    public bool CheckAndBreakSkill(SkillBase skillBase)
    {
        bool canUse = true;
        for (int i = 0; i < skillList.Count; i++)
        {
            SkillBase getSkill = skillList[i];
            bool check = getSkill.CheckSkillBreakAndParalle(skillBase.skillVo.breakType);
            if (check == false)
            {
                canUse = false;
            }
        }
        return canUse;
    }

    public bool CheckAndBreakByType(int breakType)
    {
        bool canUse = true;
        if (breakType == 999)
        {
            return true;
        }
        for (int i = 0; i < skillList.Count; i++)
        {
            SkillBase getSkill = skillList[i];
            bool check = getSkill.CheckSkillBreakAndParalle(breakType);
            if (check == false)
            {
                canUse = false;
            }
        }
        return canUse;
    }

    public virtual void OnUpdate(float dt)
    {
        for (int i = skillList.Count - 1; i >= 0; i--)
        {
            skillList[i].OnUpdate(dt);
        }
        effectMgr.OnUpdate(dt);
    }

    public virtual void FixedUpdate(float dt)
    {
        for (int i = skillList.Count - 1; i >= 0; i--)
        {
            skillList[i].FixedUpdate(dt);
        }
    }

    public virtual void LateUpdate(float dt)
    {
        for (int i = skillList.Count - 1; i >= 0; i--)
        {
            skillList[i].LateUpdate(dt);
        }
    }
    public bool UseSkill(int skillId)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillId == skillId)
            {
                return skillList[i].UseSkill();
                
            }
        }
        return false;
    }
    public bool UseSkillByType(SkillType type)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillVo.type == (int)type)
            {
                return skillList[i].UseSkill();
            }
        }
        return false;
    }
    public void DestroyAll()
    {
        for (int i = skillList.Count - 1; i >= 0; i--)
        {
            skillList[i].DestroyAll();
        }
        skillList.Clear();

        effectMgr.DestroyAll();
        effectMgr = null;
    }
}
public class BattleSkillBaseInfo
{
    public int skillId;
    public int skillLevel;
}
