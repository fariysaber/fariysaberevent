using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_damage : Buff_Add
{
    public float damage;
    protected override void StartBuff(BuffBaseInfo info)
    {
        base.StartBuff(info);
        damage = (float)info.data;
        BattlePlayerEntity baten = buffMgr.entity as BattlePlayerEntity;
        int bufDamage = -(int)(buffVo.param[0] * damage / 1000f);
        baten.attributeMgr.AddHp(bufDamage);
    }
}