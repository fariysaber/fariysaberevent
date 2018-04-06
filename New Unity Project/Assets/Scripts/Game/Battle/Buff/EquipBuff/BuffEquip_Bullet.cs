using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffEquip_Bullet : Buff_Equip
{
    //参数0 概率
    //参数1 触发每次cd
    //参数2 触发敌方单位cd
    //参数3 触发是否需要暴击
    //参数4 触发敌方子弹id
    public override void SetShanghai(Bullet bullet, float finalShanghai, bool baoji, BattlePlayerEntity hitbatEntity)
    {
        if (buffChufaTime > 0)
        {
            return;
        }
        base.SetShanghai(bullet, finalShanghai, baoji, hitbatEntity);
        float random = UnityEngine.Random.Range(0, 1000f);
        int gailv = buffVo.param[0];
        bool chufa = random > gailv ? false : true;
        if (chufa && (baoji || buffVo.param[3] == 0) && !IsInEntityCd(hitbatEntity.UID))
        {
            AddEntityCd(hitbatEntity.UID);
            buffChufaTime = (float)buffVo.param[1] / 1000f;
            BattlePlayerEntity baten = buffMgr.entity as BattlePlayerEntity;
            object data = null;
            if (buffVo.ischufapos == 1)
            {
                data = hitbatEntity.model.Pos;
            }
            baten.bulletMgr.CreateBullet(buffVo.param[4], -1, 1, false, data);
        }
    }
}
