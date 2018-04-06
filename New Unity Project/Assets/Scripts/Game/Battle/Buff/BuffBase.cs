using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    public int buffid;
    public BuffVo buffVo;
    public BuffMgr buffMgr;

    protected Effect buffEffect;
    public Vector3 buffOffeset = Vector3.zero;
    protected bool waitLoaded;
    public float startTime;
    public bool canGetSame;//判断是否可重复触发该buff
    public float cangetsameTime;
    public void Init(BuffBaseInfo info, BuffMgr buffMgr)
    {
        this.buffMgr = buffMgr;
        this.buffid = info.buffId;
        buffVo = BuffConfig.GetData(buffid + "");
        canGetSame = buffVo.cd == 0 ? true : false;
        cangetsameTime = 0f;

        startTime = 0f;
        InitInfo(info);
    }
    protected virtual void InitInfo(BuffBaseInfo info)
    {
        LoadEffect();
        StartBuff(info);
    }
    public virtual void AddNewBuffInfo(BuffBaseInfo info)
    {
        StartBuff(info);
    }
    protected virtual void StartBuff(BuffBaseInfo info)
    {

    }
    public virtual int GetAtkBuffNum()
    {
        return 0;
    }
    public virtual int GetAtkAddPercent()
    {
        return 0;
    }
    public virtual int GetChuantou()
    {
        return 0;
    }
    public virtual int GetDefBuffNum()
    {
        return 0;
    }
    public virtual int GetDefAddpercent()
    {
        return 0;
    }
    public virtual int GetHpBuffNum()
    {
        return 0;
    }
    public virtual int GetHpAddpercent()
    {
        return 0;
    }
    //小数
    public virtual float GetBaojiBuffNum()
    {
        return 0;
    }
    public virtual int GetBaojiAddpercent()
    {
        return 0;
    }
    public virtual float GetBaoShangBuffNum()
    {
        return 0;
    }
    public virtual int GetBaoShangAddpercent()
    {
        return 0;
    }
    public virtual int GetAddDmg()
    {
        return 0;
    }
    public virtual int GetReduDmg()
    {
        return 0;
    }
    public virtual void SetShanghai(Bullet bullet, float finalShanghai, bool baoji, BattlePlayerEntity hitbatEntity)
    {

    }

    protected virtual void LoadEffect()
    {
        if (buffVo.effect.Equals(""))
        {
            return;
        }
        if (buffEffect != null)
        {
            DestroyEffect();
            buffEffect = null;
        }
        if (buffMgr.entity.model != null)
        {
            switch (buffMgr.entity.model.LoadState)
            {
                case ModelLoadState.loaded:
                    AttachEffectObject(); break;
                case ModelLoadState.loading:
                    waitLoaded = true; break;
                case ModelLoadState.none:
                    waitLoaded = true; break;
                default:
                    break;
            }
        }
    }

    protected virtual void UpdateEffect(float dt)
    {
        if (waitLoaded == false)
        {
            return;
        }
        if (buffMgr.entity.model != null && buffMgr.entity.model.LoadState == ModelLoadState.destroy)
        {
            waitLoaded = false;
            return;
        }
        if (buffMgr.entity.model != null && buffMgr.entity.model.LoadState == ModelLoadState.loaded)
        {
            waitLoaded = false;
            AttachEffectObject();
            return;
        }
    }

    protected virtual void UpdateCd(float dt)
    {
        if (buffVo.time > 10000000)
        {
            return;
        }
        startTime += dt;
        if (startTime >= buffVo.time / 1000f)
        {
            buffMgr.RemoveBuff(buffid);
        }
    }

    protected virtual void UpdateCanGetSame(float dt)
    {
        if (buffVo.cd > 0 && canGetSame == false)
        {
            cangetsameTime += dt;
            if (cangetsameTime >= buffVo.cd)
            {
                canGetSame = true;
            }
        }
    }

    protected virtual void AttachEffectObject()
    {
        Transform parent = buffMgr.entity.model.GetEffectObjName();
        buffEffect = buffMgr.GetEffect(buffVo.effect, ResourceType.scene, buffOffeset,Vector3.zero,Vector3.one,Globals.normaleffectlayer,
            parent, -1f);
    }

    public virtual void BreakBuff()
    {

    }
    public virtual void OnUpdate(float dt)
    {
        UpdateEffect(dt);
        UpdateCd(dt);
        UpdateCanGetSame(dt);
    }

    public virtual void FixedUpdate(float dt)
    {

    }

    public virtual void LateUpdate(float dt)
    {
    }
    public virtual bool CheckAndBreakBuff(int breakeType)
    {
        List<int> parallList = buffVo.parallel;
        bool canUse = false;
        for (int i = 0; i < parallList.Count; i++)
        {
            if (parallList[i] == breakeType || parallList[i] == 999)
            {
                canUse = true;
            }
        }
        if (breakeType == 999)
        {
            canUse = true;
        }
        List<int> breakList = buffVo.breakList;
        for (int i = 0; i < breakList.Count; i++)
        {
            if (breakList[i] == breakeType)
            {
                BreakBuff();
                canUse = true;
                return canUse;
            }
        }
        return canUse;
    }
    public virtual void DestroyEffect()
    {
        buffMgr.effectMgr.Addtemp(buffEffect);
        buffEffect = null;
    }
    public virtual void DestroyAll()
    {
        DestroyEffect();
    }
    public virtual bool IsShowBuff()
    {
        if (buffVo.buffType == 99)
        {
            return false;
        }
        return true;
    }
}