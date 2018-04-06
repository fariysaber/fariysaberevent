using System;
using System.Collections.Generic;
using UnityEngine;

//叶子节点
//是否单位不存在或者dead的节点
public class IsDeadCodiNode : CoditionNode
{
    protected override void ConditionNode()
    {
        if (batentity == null)
        {
            SetCallback(false);
            return;
        }
        if (batentity != null)
        {
            if (batentity.IsDead || batentity.IsDispose)
            {
                SetCallback(false);
                return;
            }
        }
        SetCallback(true);
    }
}