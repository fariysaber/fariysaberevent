using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//仇恨在范围内的节点
public class InRotateCodiNode : CoditionNode
{
    //度数
    public float inangle = 30f;
    public bool isInOrOut = true;
    public float angle;
    protected override void ConditionNode()
    {
        BattlePlayerEntity chouhenentity = batentity.chouhenEntity;
        Vector3 toVector = chouhenentity.model.Pos - batentity.model.Pos;
        angle = Vector3.Angle(batentity.model.Forward, toVector);
        bool value;
        if (angle < inangle)
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