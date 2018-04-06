using System;
using System.Collections.Generic;
using UnityEngine;
//复合节点
//顺序节点（按顺序从左至右，当所有子节点返回true时候返回true，如果有一个为false，则返回false，后面的不执行）
public class SequencerNode : CompositeNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        nodeIndex = 0;
        childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
    }
    public void CallChilcBack(bool result)
    {
        if (behaviorLoad == BehaviorLoad.destroy)
        {
            return;
        }
        if (result == false)
        {
            SetCallback(false);
            return;
        }
        nodeIndex ++;
        if (nodeIndex >= childNodes.Count)
        {
            SetCallback(true);
            return;
        }
        else
        {
            childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
        }
    }
}