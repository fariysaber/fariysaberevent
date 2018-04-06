using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1601 : Buff_damage
{
    public float damage;
    protected override void StartBuff(BuffBaseInfo info)
    {
        base.StartBuff(info);
        SetDamage(info);
    }
    private void SetDamage(BuffBaseInfo info)
    {
        damage = (float)info.data;
        BattlePlayerEntity baten = buffMgr.entity as BattlePlayerEntity;
        int bufDamage = -(int)(buffVo.param[1] * damage / 1000f);
        baten.attributeMgr.AddHp(bufDamage);
        float random = UnityEngine.Random.Range(0, 1000f);
        if (random < buffVo.param[0])
        {
            SetDamage(info);
        }
    }
}