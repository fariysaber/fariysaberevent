using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1711 : Buff_Add
{
    public float reduceCd;
    public float damage;
    protected override void StartBuff(BuffBaseInfo info)
    {
        base.StartBuff(info);
        damage = (float)info.data;
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        reduceCd += dt;
        while (reduceCd > (float)buffVo.param[0] / 1000f)
        {
            reduceCd -= (float)buffVo.param[0] / 1000f;
            BattlePlayerEntity baten = buffMgr.entity as BattlePlayerEntity;
            int bufDamage = -(int)(buffVo.param[1] * damage / 1000f);
            baten.attributeMgr.AddHp(bufDamage);
        }
    }
}