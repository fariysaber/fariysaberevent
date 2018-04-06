using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//仇恨在范围内的节点
public class InChouhenRangeCodiNode : CoditionNode
{
    public float range = 5f;
    public bool isInOrOut = true;
    public float distance;
    protected override void ConditionNode()
    {
        BattlePlayerEntity chouhenentity = batentity.chouhenEntity;
        Vector3 chouhenpos = chouhenentity.model.Pos;
        Vector3 pos = batentity.model.Pos;
        distance = (pos - chouhenpos).magnitude;
        float batRadius = batentity.model.GetColliderLength();
        float chouhenRadius = chouhenentity.model.GetColliderLength();
        distance = distance - batRadius - chouhenRadius;

        bool value = true;
        if (distance < range)
        {
            value = true;
        }
        else
        {
            value = false;
        }
        if (!isInOrOut)
        {
            value = !value;
        }
        SetCallback(value);
    }
}