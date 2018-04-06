using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//仇恨单位存在节点
public class ChouhenCodiNode : CoditionNode
{
    protected override void ConditionNode()
    {
        BattlePlayerEntity chouhenentity = batentity.chouhenEntity;
        if (chouhenentity == null)
        {
            SetCallback(false);
            return;
        }
        if (chouhenentity != null)
        {
            if (chouhenentity.IsDead || chouhenentity.IsDispose)
            {
                SetCallback(false);
                return;
            }
        }
        SetCallback(true);
    }
}