using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//仇恨在范围内的节点
public class InStartRangeCodiNode : CoditionNode
{
    public float range = 10f;
    public float distance;
    protected override void ConditionNode()
    {
        BattlePlayerEntity chouhenentity = batentity.chouhenEntity;
        Vector3 pos = batentity.model.Pos;
        distance = (pos - batentity.model.bornPos).magnitude;
        if (distance < range)
        {
            SetCallback(true);
        }
        else
        {
            SetCallback(false);
        }
    }
}