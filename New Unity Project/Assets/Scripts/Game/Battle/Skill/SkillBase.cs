using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillState
{
    none = 0,
    cast = 1,
    middle = 2,
    back = 3,
}
public class SkillBase
{
    public int skillId;
    public int skillLevel;
    public SKillVo skillVo;
    public float skilltime;

    public float skillStateTime;
    public SkillMgr skillMgr;
    public SkillState skillState;
    public BreakAndParalleMgr breakAndParalleMgr;

    public float middletime;
    public float backtime;
    public float casttime;
    public void Init(BattleSkillBaseInfo baseInfo,SkillMgr getskillMgr)
    {
        skillMgr = getskillMgr;
        skillId = baseInfo.skillId;
        skillLevel = baseInfo.skillLevel;
        skillVo = SkillConfig.GetData(skillId + "");
        skilltime = 99999f;
        skillStateTime = 0f;

        middletime = skillVo.middletime / 1000f;
        casttime = skillVo.casttime / 1000f;
        backtime = skillVo.backtime / 1000f;

        skillState = SkillState.none;
        breakAndParalleMgr = new BreakAndParalleMgr();
        breakAndParalleMgr.AddBreakeList((int)SkillState.cast, skillVo.castBreak);
        breakAndParalleMgr.AddBreakeList((int)SkillState.middle, skillVo.middleBreak);
        breakAndParalleMgr.AddBreakeList((int)SkillState.back, skillVo.backBreak);
        breakAndParalleMgr.AddParalle((int)SkillState.cast, skillVo.castparallel);
        breakAndParalleMgr.AddParalle((int)SkillState.middle, skillVo.middleparallel);
        breakAndParalleMgr.AddParalle((int)SkillState.back, skillVo.backparallel);
        InitInfo(baseInfo);
    }
    public virtual void InitInfo(BattleSkillBaseInfo baseInfo)
    {

    }

    public virtual bool UseSkill()
    {
        Debugger.Log("点击使用技能" + skillId);
        if (CheckAndBreak() == false)
        {
            return false;
        }
        Debugger.Log("使用技能成功" + skillId);
        Cost();
        skilltime = 0f;
        CastSkill();
        return true;
    }

    /// <summary>
    /// 前摇相关
    /// </summary>
    protected virtual void CastSkill()
    {
        skillState = SkillState.cast;
        skillStateTime = 0f;
        StartCastAction();
        CreateCastEffect();
    }

    protected virtual void StartCastAction()
    {
        if (skillVo.castaction.Equals(""))
        {
            return;
        }
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.animCtrl.PlayAnimByStateName(skillVo.castaction);
    }

    protected virtual void CreateCastEffect()
    {
    }

    protected virtual void CastSkillUpdate(float dt)
    {

    }

    protected virtual void CastSkillEnd()
    {
        Debugger.Log("进入前摇期结束" + skillId);
    }

    /// <summary>
    /// 持续期相关
    /// </summary>
    protected virtual void MiddleSkill()
    {
        Debugger.Log("进入技能持续期成功" + skillId);
        skillState = SkillState.middle;
        skillStateTime = 0f;
        StartMiddleAction();
        CreateMiddleEffect();
    }

    protected virtual void CreateMiddleEffect()
    {
    }

    protected virtual void StartMiddleAction()
    {
        if (skillVo.middleaction.Equals(""))
        {
            return;
        }
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.animCtrl.PlayAnimByStateName(skillVo.middleaction);
    }

    protected virtual void MiddleSkillUpdate(float dt)
    {

    }

    protected virtual void MiddleSkillEnd()
    {
        Debugger.Log("进入持续期结束" + skillId);
    }

    /// <summary>
    /// 后摇相关
    /// </summary>
    protected virtual void BackSkill()
    {
        Debugger.Log("进入后摇期" + skillId);
        skillState = SkillState.back;
        skillStateTime = 0f;
        StartBackAction();
        CreateBackEffect();
    }

    protected virtual void CreateBackEffect()
    {
    }

    protected virtual void StartBackAction()
    {
        if (skillVo.backaction.Equals(""))
        {
            return;
        }
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        playermodel.animCtrl.PlayAnimByStateName(skillVo.backaction);
    }

    protected virtual void BackSkillUpdate(float dt)
    {

    }

    protected virtual void BackSkillend()
    {
        Debugger.Log("进入后摇期结束" + skillId);
    }


    /// <summary>
    /// 返回true为不满足,false为满足
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckAndBreak()
    {
        if (skillMgr.entity.IsDead)
        {
            Debugger.Log("死亡无法释放" + skillId);
            return false;
        }
        if (CheckCd() == false)
        {
            Debugger.Log("cd未到" + skillId);
            return false;
        }
        if (IsLockSkill())
        {
            Debugger.Log("被锁住技能" + skillId);
            return false;
        }
        if (CheckCost() == false)
        {
            //不满足消耗
            Debugger.Log("不满足消耗" + skillId);
            return false;
        }
        if (CheckAndBreakBuff() == false)
        {
            Debugger.Log("buff阻挡" + skillId,Color.red);
            return false;
        }
        if (CheckAndBreakSkill() == false)
        {
            Debugger.Log("技能阻挡" + skillId, Color.red);
            return false;
        }
        return true;
    }

    protected virtual bool IsLockSkill()
    {
        if (skillMgr != null)
        {
            if (skillMgr.entity != null)
            {
                if (skillMgr.entity.skillLocker != null && skillMgr.entity.skillLocker.HasLocker())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public virtual bool CheckCd()
    {
        Debugger.Log("cd为" + skillVo.cd + "已过去时间" + skilltime);
        if (skillVo.cd / 1000f <= 0)
        {
            return true;
        }
        if (skilltime >= skillVo.cd / 1000f)
        {
            return true;
        }
        return false;
    }

    //检测是否满足消耗条件
    protected virtual bool CheckCost()
    {
        List<List<int>> costParam = skillVo.costParam;
        if (costParam.Count <= 0)
        {
            return true;
        }
        for (int i = 0; i < skillVo.costParam.Count; i++)
        {
            if (skillVo.costParam[i].Count <= 0)
            {
                continue;
            }
            if (skillVo.costParam[i][0] == 1 && skillVo.costParam[i][1] > (skillMgr.entity as BattlePlayerEntity).attributeMgr.Energy)
            {
                return false;
            }
        }
        return true;
    }
    protected virtual void Cost()
    {
        for (int i = 0; i < skillVo.costParam.Count; i++)
        {
            if (skillVo.costParam[i].Count <= 0)
            {
                continue;
            }
            if (skillVo.costParam[i][0] == 1)
            {
                (skillMgr.entity as BattlePlayerEntity).attributeMgr.AddEnergy(-skillVo.costParam[i][1]);
            }
        }
    }

    protected virtual bool CheckAndBreakSkill()
    {
        return skillMgr.CheckAndBreakSkill(this);
    }

    protected virtual bool CheckAndBreakBuff()
    {
        BuffMgr buffMgr = (skillMgr.entity as BattlePlayerEntity).buffMgr;
        bool canUse = buffMgr.CheckAndBreakBuff(skillVo.breakType);
        
        return canUse;
    }

    public virtual void OnUpdate(float dt)
    {
        UpdateCd(dt);
        UpdateSkillState(dt);
    }

    protected virtual void UpdateSkillState(float dt)
    {
        if (skillState == SkillState.none)
        {
            return;
        }
        skillStateTime += dt;
        if (skillState == SkillState.cast)
        {
            if (skillStateTime >= casttime)
            {
                CastSkillUpdate(dt);
                CastSkillEnd();
                MiddleSkill();
            }
            else
            {
                CastSkillUpdate(dt);
                return;
            }
        }
        if (skillState == SkillState.middle)
        {
            if (skillStateTime >= middletime)
            {
                MiddleSkillUpdate(dt);
                MiddleSkillEnd();
                BackSkill();
            }
            else
            {
                MiddleSkillUpdate(dt);
                return;
            }
        }
        if (skillState == SkillState.back)
        {
            if (skillStateTime >= backtime)
            {
                BackSkillUpdate(dt);
                BackSkillend();
                EndSkill();
            }
            else
            {
                BackSkillUpdate(dt);
                return;
            }
        }
    }

    private void UpdateCd(float dt)
    {
        if (skillVo.cd <= 0)
        {
            return;
        }
        if (skillVo.cdhuansuan == 1 && skillState != SkillState.none)
        {
            return;
        }
        if (skillVo.cd <= skilltime)
        {
            return;
        }
        skilltime += dt;
    }

    public virtual void FixedUpdate(float dt)
    {
    }

    public virtual void LateUpdate(float dt)
    {
    }
    protected virtual void StopAction()
    {

    }
    protected virtual void StopEffect()
    {

    }
    protected virtual void DestroyEffect()
    {
        StopEffect();
    }
    public virtual void EndSkill()
    {
        Debugger.Log("结束技能" + skillId);
        if (skillState == SkillState.none)
        {
            return;
        }
        StopAction();
        StopEffect();
        skillState = SkillState.none;
        skillStateTime = 0f;
    }
    public virtual void BreakeSkill()
    {
        if (skillState == SkillState.none)
        {
            return;
        }
        EndSkill();
    }
    public virtual void DestroyAll()
    {
        BreakeSkill();
        StopAction();
        DestroyEffect();
    }
    //判断打断逻辑和并行逻辑
    public bool CheckSkillBreakAndParalle(int breakType)
    {
        if (skillState == SkillState.none)
        {
            return true;
        }
        bool canUse = false;
        if (breakAndParalleMgr.CheckCanParalle((int)skillState, breakType))
        {
            canUse = true;
        }
        if (breakAndParalleMgr.CheckCanBreake((int)skillState, breakType))
        {
            BreakeSkill();
            canUse = true;
        }
        return canUse;
    }
}
