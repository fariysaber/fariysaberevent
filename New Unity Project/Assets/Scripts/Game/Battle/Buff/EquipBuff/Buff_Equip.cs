using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Equip : BuffBase
{
    protected float buffChufaTime = -1;
    protected List<BuffEquipEntity> dicIds = new List<BuffEquipEntity>();
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (buffChufaTime > 0)
        {
            buffChufaTime -= dt;
        }
        for (int i = dicIds.Count - 1; i >= 0; i--)
        {
            dicIds[i].time -= dt;
            if (dicIds[i].time <= 0)
            {
                dicIds.RemoveAt(i);
            }
        }
    }
    public bool IsInEntityCd(string key)
    {
        for (int i = dicIds.Count - 1; i >= 0; i--)
        {
            if (dicIds[i].key.Equals(key))
            {
                return true;
            }
        }
        return false;
    }
    public void AddEntityCd(string key)
    {
        if (buffVo.param[2] < 0)
        {
            return;
        }
        BuffEquipEntity newEntityCd = new BuffEquipEntity();
        newEntityCd.key = key;
        newEntityCd.time = (float)buffVo.param[2] / 1000f;
        dicIds.Add(newEntityCd);
    }
}
public class BuffEquipEntity
{
    public string key;
    public float time;
}