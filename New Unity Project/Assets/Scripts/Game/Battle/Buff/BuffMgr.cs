using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffMgr
{
    public List<BuffBase> buffList;
    public PlayerEntity entity;
    public EffectMgr effectMgr;
    public BuffMgr(PlayerEntity getentity)
    {
        buffList = new List<BuffBase>();
        effectMgr = new EffectMgr();
        entity = getentity;
        AddBaseBuff();
    }
    public Effect GetEffect(string path, ResourceType resType, Vector3 localPos, Vector3 rotate, Vector3 scale, int layer, Transform parent,float time,
        bool resetPause = true)
    {
        Effect effect = effectMgr.GetEffect(path);
        effect.InitEffect(path, resType, localPos, rotate, scale, layer, parent, time);
        return effect;
    }
    public List<int> GetBuffIds()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].IsShowBuff())
            {
                ids.Add(buffList[i].buffid);
            }
        }
        return ids;
    }
    private void AddBaseBuff()
    {
        BuffBaseInfo info = new BuffBaseInfo();
        info.buffId = Globals.BaseBuffId;
        AddBuff(info);
    }
    public void AddBuff(int id,object data = null)
    {
        BuffBaseInfo info = new BuffBaseInfo();
        info.buffId = id;
        info.data = data;
        AddBuff(info);
    }
    public void AddBuff(BuffBaseInfo info)
    {
        Debug.Log("加入buff" + info.buffId);
        BuffVo buffvo = BuffConfig.GetData(info.buffId + "");
        if (CheckAndBreakBuff(buffvo.breaktype) == false)
        {
            return;
        }
        if ((entity as BattlePlayerEntity).skillMgr.CheckAndBreakByType(buffvo.breaktype) == false)
        {
            return;
        }
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffid == info.buffId)
            {
                if (buffList[i].canGetSame)
                {
                    buffList[i].AddNewBuffInfo(info);
                }
                return;
            }
        }
        Type getType = GameTools.GetType("Buff_" + info.buffId);
        BuffBase getBuff = Activator.CreateInstance(getType) as BuffBase;
        getBuff.Init(info, this);
        buffList.Add(getBuff);

        if (entity.EntityType == EntityTypes.BattlePlayerMy)
        {
            EventDispatcher.TriggerEvent(GlobalEvents.BATTLE_HEROBUFF);
        }
    }
    public bool CheckAndBreakBuff(int breakeType)
    {
        bool canUse = true;
        for (int i = 0; i < buffList.Count; i++)
        {
            BuffBase buffBase = buffList[i];
            bool check = buffBase.CheckAndBreakBuff(breakeType);
            if (check == false)
            {
                canUse = false;
            }
        }
        return canUse;
    }
    //下列百分比都是千分位
    public int GetAtkBuffNum()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetAtkBuffNum();
        }
        return num;
    }
    public int GetAtkAddPercent()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetAtkAddPercent();
        }
        return num;
    }
    public int GetChuantou()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetChuantou();
        }
        return num;
    }
    public int GetDefBuffNum()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetDefBuffNum();
        }
        return num;
    }
    public int GetDefAddpercent()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetDefAddpercent();
        }
        return num;
    }
    public int GetHpBuffNum()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetHpBuffNum();
        }
        return num;
    }
    public int GetHpAddpercent()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetHpAddpercent();
        }
        return num;
    }
    public float GetBaojiBuffNum()
    {
        float num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetBaojiBuffNum();
        }
        return num;
    }
    public int GetBaojiAddpercent()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetBaojiAddpercent();
        }
        return num;
    }
    public float GetBaoShangBuffNum()
    {
        float num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetBaoShangBuffNum();
        }
        return num;
    }
    //设置伤害情况下的事件
    public void SetShanghai(Bullet bullet, float finalShanghai, bool baoji, BattlePlayerEntity hitbatEntity)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            buffList[i].SetShanghai(bullet, finalShanghai, baoji, hitbatEntity);
        }
    }
    public int GetBaoShangAddpercent()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetBaoShangAddpercent();
        }
        return num;
    }
    public int GetAddDmg()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetAddDmg();
        }
        return num;
    }
    public int GetReduDmg()
    {
        int num = 0;
        for (int i = 0; i < buffList.Count; i++)
        {
            num += buffList[i].GetReduDmg();
        }
        return num;
    }
    public virtual void OnUpdate(float dt)
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].OnUpdate(dt);
        }
        effectMgr.OnUpdate(dt);
    }

    public virtual void FixedUpdate(float dt)
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].FixedUpdate(dt);
        }
    }

    public virtual void LateUpdate(float dt)
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].LateUpdate(dt);
        }
    }
    public virtual void RemoveBuff(int buffid)
    {
        Debug.Log("移除buff" + buffid);
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffid == buffid)
            {
                buffList[i].DestroyAll();
                buffList.RemoveAt(i);
                break;
            }
        }
        if (entity.EntityType == EntityTypes.BattlePlayerMy)
        {
            EventDispatcher.TriggerEvent(GlobalEvents.BATTLE_HEROBUFF);
        }
    }
    public void DestroyAll()
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            buffList[i].DestroyAll();
        }
        buffList.Clear();
        effectMgr.DestroyAll();
        effectMgr = null;
    }
}
public class BuffBaseInfo
{
    public int buffId;
    public string buffPlayer;
    public float buffTime = -1;
    public object data;
}
